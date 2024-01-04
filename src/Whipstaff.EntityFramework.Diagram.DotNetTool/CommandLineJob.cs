// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Whipstaff.CommandLine;
using Whipstaff.EntityFramework.Diagram.DotNetTool.CommandLine;

namespace Whipstaff.EntityFramework.Diagram.DotNetTool
{
    /// <summary>
    /// Command line job for handling the creation of the Entity Framework Diagram.
    /// </summary>
    public sealed class CommandLineJob : ICommandLineHandler<CommandLineArgModel>
    {
        private readonly CommandLineJobLogMessageActionsWrapper _commandLineJobLogMessageActionsWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineJob"/> class.
        /// </summary>
        /// <param name="commandLineJobLogMessageActionsWrapper">Wrapper for logging framework messages.</param>
        public CommandLineJob(CommandLineJobLogMessageActionsWrapper commandLineJobLogMessageActionsWrapper)
        {
            ArgumentNullException.ThrowIfNull(commandLineJobLogMessageActionsWrapper);

            _commandLineJobLogMessageActionsWrapper = commandLineJobLogMessageActionsWrapper;
        }

        /// <inheritdoc/>
        public Task<int> HandleCommand(CommandLineArgModel commandLineArgModel)
        {
            return Task.Run(() =>
            {
                _commandLineJobLogMessageActionsWrapper.StartingHandleCommand();
                var assembly = Assembly.LoadFrom(commandLineArgModel.AssemblyPath.FullName);
                var outputFilePath = commandLineArgModel.OutputFilePath;
                var dbContextName = commandLineArgModel.DbContextName;

                var dbContext = GetDesignTimeDbContextFactoryFromAssembly(
                                    assembly,
                                    dbContextName)
                                    ?? GetDbContextFromAssembly(
                                        assembly,
                                        dbContextName);

                if (dbContext == null)
                {
                    _commandLineJobLogMessageActionsWrapper.FailedToFindDbContext(dbContext);
                    return 1;
                }

                GenerateFromDbContext(
                    dbContext,
                    outputFilePath);

                return 0;
            });
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

        private static void GenerateFromDbContext(DbContext dbContext, FileInfo outputFilePath)
        {
            var dgml = dbContext.AsDgml();
            File.WriteAllText(
                outputFilePath.FullName,
                dgml,
                Encoding.UTF8);
        }

        private static DbContext? GetDesignTimeDbContextFactoryFromAssembly(Assembly assembly, string dbContextName)
        {
            var allTypes = assembly.GetTypes();

            var matchingType = allTypes.AsParallel()
                .FirstOrDefault(type => IsDesignTimeDbContextFactory(type, dbContextName));

            if (matchingType == null)
            {
                return null;
            }

            var dbContextType = Type.GetType(dbContextName);
            if (dbContextType == null)
            {
                return null;
            }

            var instance = Activator.CreateInstance(matchingType);
            var method = typeof(IDesignTimeDbContextFactory<>).MakeGenericType(dbContextType).GetMethod("CreateDbContext");
            var res = method!.Invoke(
                instance,
                new object[] { Array.Empty<string>() });

            return res as DbContext;
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

        private static DbContext? GetDbContextFromAssembly(Assembly assembly, string dbContextName)
        {
            var allTypes = assembly.GetTypes();

            var matchingType = allTypes.AsParallel().FirstOrDefault(type => IsDbContextType(type, dbContextName));
            if (matchingType == null)
            {
                return null;
            }

            var instance = Activator.CreateInstance(matchingType);

            // ReSharper disable once MergeConditionalExpression
            return instance != null ? (DbContext)instance : null;
        }
    }
}
