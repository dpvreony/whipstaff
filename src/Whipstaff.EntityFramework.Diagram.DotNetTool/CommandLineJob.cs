// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
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
        private JobLogMessageActionsWrapper _jobLogMessageActionsWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineJob"/> class.
        /// </summary>
        /// <param name="jobLogMessageActionsWrapper">Wrapper for logging framework messages.</param>
        public CommandLineJob(JobLogMessageActionsWrapper jobLogMessageActionsWrapper)
        {
            ArgumentNullException.ThrowIfNull(jobLogMessageActionsWrapper);

            _jobLogMessageActionsWrapper = jobLogMessageActionsWrapper;
        }

        /// <inheritdoc/>
        public Task<int> HandleCommand(CommandLineArgModel commandLineArgModel)
        {
            return Task.Run(() =>
            {
                try
                {
                    var assembly = Assembly.LoadFrom(commandLineArgModel.AssemblyPath.FullName);
                    var outputFilePath = commandLineArgModel.OutputFilePath;

                    GenerateFromAssembly(
                        assembly,
                        commandLineArgModel.DbContextName,
                        outputFilePath);

                    return 0;
                }
                catch (Exception e)
                {
                    _jobLogMessageActionsWrapper.LogError(e.ToString());
                    return int.MaxValue;
                }
            });
        }

        /// <summary>
        /// Generates DGML output for each <see cref="DbContext"/> found in an assembly.
        /// </summary>
        /// <param name="assembly">Assembly to check.</param>
        /// <param name="outputFilePath"></param>
        public void GenerateFromAssembly(
            Assembly assembly,
            string dbContextName,
            string outputFilePath)
        {
            ArgumentNullException.ThrowIfNull(assembly);

            var dbContextType = GetDesignTimeDbContextFactoryFromAssembly(assembly, dbContextName)
                ?? GetDbContextFromAssembly(assembly);

            if (dbContextType != null)
            {
                GenerateFromDbContext(dbContextType, outputFilePath);
            }
        }

        private DbContext? GetDesignTimeDbContextFactoryFromAssembly(Assembly assembly, string dbContextName)
        {
            throw new NotImplementedException();
        }

        private static bool IsDbContextType(Type type)
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

            return true;
        }

        private void GenerateFromType(Type dbContextType, string outputFilePath)
        {
            try
            {
                var instance = Activator.CreateInstance(dbContextType);
                var dbContext = instance as DbContext;

                GenerateFromDbContext(dbContext, outputFilePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static void GenerateFromDbContext(DbContext dbContext, string outputFilePath)
        {
            var dgml = dbContext.AsDgml();
            File.WriteAllText(
                outputFilePath,
                dgml,
                Encoding.UTF8);
        }

        private DbContext? GetDesignTimeDbContextFactoryFromAssembly(Assembly assembly, string dbContextName)
        {
            var allTypes = assembly.GetTypes();

            var matchingType = allTypes.AsParallel()
                .FirstOrDefault(type => IsDesignTimeDbContextFactory(type, dbContextName));

            if (matchingType == null)
            {
                return null;
            }

            var instance = Activator.CreateInstance(matchingType);
            var method = typeof(IDesignTimeDbContextFactory<>).GetMethod("CreateDbContext");
            var res = method!.Invoke(
                instance,
                new object[] { Array.Empty<string>() });

            return res as DbContext;
        }

        private bool IsDesignTimeDbContextFactory(Type type, string dbContextName)
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

        private DbContext? GetDbContextFromAssembly(Assembly assembly, string dbContextName)
        {
            var allTypes = assembly.GetTypes();

            return allTypes.AsParallel().FirstOrDefault(type => IsDbContextType(type, dbContextName));
        }
    }
}
