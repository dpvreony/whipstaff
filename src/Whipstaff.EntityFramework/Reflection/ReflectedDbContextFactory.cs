// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Whipstaff.Runtime.Extensions;

namespace Whipstaff.EntityFramework.Reflection
{
    /// <summary>
    /// DbContext factory for creating a DbContext from an assembly via reflection.
    /// </summary>
    /// <remarks>
    /// These are intended to be used for support scenarios such as diagram and documentation generation. They are not high performance
    /// and should not be used for normal data access.
    /// </remarks>
    public static class ReflectedDbContextFactory
    {
        /// <summary>
        /// Gets a DbContext from an assembly via reflection by searching for a <see cref="IDesignTimeDbContextFactory{TContext}"/>.
        /// </summary>
        /// <param name="assembly">The assembly to search.</param>
        /// <param name="dbContextName">The name of the DB Context the design time factory should represent.</param>
        /// <returns>DbContext instance, if factory was found and can be created.</returns>
        public static DbContext? GetDesignTimeDbContextFactoryFromAssembly(Assembly assembly, string dbContextName)
        {
            ArgumentNullException.ThrowIfNull(assembly);

            var allTypes = assembly.GetTypes();

            var matchingType = allTypes.AsParallel()
                .FirstOrDefault(type => IsDesignTimeDbContextFactory(type, dbContextName));

            if (matchingType == null)
            {
                return null;
            }

            var ctor = matchingType.GetParameterlessConstructor();
            if (ctor == null)
            {
                return null;
            }

            var instance = ctor.Invoke(null);
            var method = matchingType.GetMethod("CreateDbContext");
            var res = method!.Invoke(
                instance,
                [Array.Empty<string>()]);

            return res as DbContext;
        }

        /// <summary>
        /// Gets a DbContext from an assembly via reflection by searching for a <see cref="DbContext"/>.
        /// </summary>
        /// <param name="assembly">The assembly to search.</param>
        /// <param name="dbContextName">The name of the DB Context to search for.</param>
        /// <returns>Instance of db context, if found.</returns>
        public static DbContext? GetDbContextFromAssembly(Assembly assembly, string dbContextName)
        {
            ArgumentNullException.ThrowIfNull(assembly);

            var allTypes = assembly.GetTypes();

            var matchingType = allTypes.AsParallel().FirstOrDefault(type => IsDbContextType(type, dbContextName));
            if (matchingType == null)
            {
                return null;
            }

            var ctor = matchingType.GetParameterlessConstructor();
            if (ctor == null)
            {
                return null;
            }

            var instance = ctor.Invoke(null);

            return (DbContext)instance;
        }

        private static bool IsDbContextType(Type type, string dbContextName)
        {
            if (!type.IsClass)
            {
                return false;
            }

            if (!type.IsPublic)
            {
                return false;
            }

            if (type.IsAbstract)
            {
                return false;
            }

            if (type.ContainsGenericParameters)
            {
                return false;
            }

            if (!type.IsSubclassOf(typeof(DbContext)))
            {
                return false;
            }

            return type.FullName != null
                   && type.FullName.Equals(dbContextName, StringComparison.Ordinal);
        }

        private static bool IsDesignTimeDbContextFactory(Type type, string dbContextName)
        {
            if (!type.IsClass)
            {
                return false;
            }

            if (!type.IsPublic)
            {
                return false;
            }

            if (type.IsAbstract)
            {
                return false;
            }

            if (type.ContainsGenericParameters)
            {
                return false;
            }

            var matchingInterface = type.GetInterface("IDesignTimeDbContextFactory`1");
            if (matchingInterface == null)
            {
                return false;
            }

            return matchingInterface.GetGenericArguments().Any(
                arg => arg.FullName != null
                       && arg.FullName.Equals(dbContextName, StringComparison.Ordinal));
        }
    }
}
