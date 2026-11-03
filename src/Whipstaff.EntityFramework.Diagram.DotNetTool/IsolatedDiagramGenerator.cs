// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO.Abstractions;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace Whipstaff.EntityFramework.Diagram.DotNetTool
{
    /// <summary>
    /// Performs isolated diagram generation for a target assembly.
    /// </summary>
    internal static class IsolatedDiagramGenerator
    {
        /// <summary>
        /// Generates a diagram file from a target db context in the active isolated load context.
        /// </summary>
        /// <param name="assemblyPath">Path to assembly containing the db context.</param>
        /// <param name="dbContextName">Full name of db context type to load.</param>
        /// <param name="outputFilePath">Output file path for generated diagram.</param>
        /// <returns>0 for success, non-zero for failure.</returns>
        public static int Generate(string assemblyPath, string dbContextName, string outputFilePath)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(assemblyPath);
            ArgumentException.ThrowIfNullOrWhiteSpace(dbContextName);
            ArgumentException.ThrowIfNullOrWhiteSpace(outputFilePath);

#pragma warning disable S3885
            var targetAssembly = Assembly.LoadFrom(assemblyPath);
#pragma warning restore S3885

            var dbContext = GetDesignTimeDbContextFactoryObjectFromAssembly(targetAssembly, dbContextName)
                            ?? GetDbContextObjectFromAssembly(targetAssembly, dbContextName);

            if (dbContext == null)
            {
                return 1;
            }

            var dgml = GenerateDgmlFromDbContextObject(dbContext);
            var fileSystem = new FileSystem();
            var outputDirectoryPath = fileSystem.Path.GetDirectoryName(outputFilePath);

            if (!string.IsNullOrWhiteSpace(outputDirectoryPath))
            {
                _ = fileSystem.Directory.CreateDirectory(outputDirectoryPath);
            }

            fileSystem.File.WriteAllText(outputFilePath, dgml, Encoding.UTF8);
            return 0;
        }

        private static object? GetDesignTimeDbContextFactoryObjectFromAssembly(Assembly assembly, string dbContextName)
        {
            ArgumentNullException.ThrowIfNull(assembly);

            var allTypes = assembly.GetTypes();

#pragma warning disable S6602 // "Find" method should be used instead of the "FirstOrDefault" extension
            var matchingType = allTypes.FirstOrDefault(type => IsDesignTimeDbContextFactory(type, dbContextName));
#pragma warning restore S6602 // "Find" method should be used instead of the "FirstOrDefault" extension

            if (matchingType == null)
            {
                return null;
            }

            var ctor = matchingType.GetConstructor(Type.EmptyTypes);
            if (ctor == null)
            {
                return null;
            }

            var factory = ctor.Invoke(null);
            var createDbContextMethod = matchingType.GetMethod("CreateDbContext");
            return createDbContextMethod?.Invoke(factory, [Array.Empty<string>()]);
        }

        private static object? GetDbContextObjectFromAssembly(Assembly assembly, string dbContextName)
        {
            ArgumentNullException.ThrowIfNull(assembly);

            var allTypes = assembly.GetTypes();
            var matchingType = allTypes.AsParallel().FirstOrDefault(type => IsDbContextType(type, dbContextName));

            if (matchingType == null)
            {
                return null;
            }

            var ctor = matchingType.GetConstructor(Type.EmptyTypes);
            return ctor?.Invoke(null);
        }

        private static string GenerateDgmlFromDbContextObject(object dbContext)
        {
            ArgumentNullException.ThrowIfNull(dbContext);

            var dbContextType = dbContext.GetType();
            var asDgmlMethod = FindAsDgmlMethod(dbContextType);
            if (asDgmlMethod == null)
            {
                throw new InvalidOperationException($"Could not locate AsDgml extension method for DbContext type {dbContextType.FullName}.");
            }

            var dgml = asDgmlMethod.Invoke(null, [dbContext]);
            return dgml as string
                   ?? throw new InvalidOperationException("AsDgml did not return a string.");
        }

        private static MethodInfo? FindAsDgmlMethod(Type dbContextType)
        {
            ArgumentNullException.ThrowIfNull(dbContextType);

            var loadContext = AssemblyLoadContext.GetLoadContext(typeof(IsolatedDiagramGenerator).Assembly);
            var assemblies = loadContext?.Assemblies.ToArray() ?? AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                foreach (var candidateType in GetLoadableTypes(assembly))
                {
                    var method = candidateType.GetMethod(
                        "AsDgml",
                        BindingFlags.Public | BindingFlags.Static,
                        binder: null,
                        [dbContextType],
                        modifiers: null);

                    if (method != null && method.ReturnType == typeof(string))
                    {
                        return method;
                    }
                }
            }

            return null;
        }

        private static Type[] GetLoadableTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException exception)
            {
                return exception.Types
                    .Where(type => type != null)
                    .Cast<Type>()
                    .ToArray();
            }
        }

        private static bool IsDbContextType(Type type, string dbContextName)
        {
            if (!type.IsClass || type.IsAbstract)
            {
                return false;
            }

            if (!string.Equals(type.FullName, dbContextName, StringComparison.Ordinal))
            {
                return false;
            }

            return InheritsFromDbContext(type);
        }

        private static bool IsDesignTimeDbContextFactory(Type type, string dbContextName)
        {
            if (!type.IsClass || type.IsAbstract || type.ContainsGenericParameters)
            {
                return false;
            }

            var matchingInterface = type.GetInterface("IDesignTimeDbContextFactory`1");
            if (matchingInterface == null)
            {
                return false;
            }

            return Array.Exists(
                matchingInterface.GetGenericArguments(),
                arg => string.Equals(arg.FullName, dbContextName, StringComparison.Ordinal));
        }

        private static bool InheritsFromDbContext(Type type)
        {
            Type? currentType = type;
            while (currentType != null)
            {
                if (string.Equals(currentType.FullName, "Microsoft.EntityFrameworkCore.DbContext", StringComparison.Ordinal))
                {
                    return true;
                }

                currentType = currentType.BaseType;
            }

            return false;
        }
    }
}
