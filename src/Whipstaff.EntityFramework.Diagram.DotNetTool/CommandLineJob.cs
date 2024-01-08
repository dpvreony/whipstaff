// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Whipstaff.CommandLine;
using Whipstaff.EntityFramework.Diagram.DotNetTool.CommandLine;
using Whipstaff.EntityFramework.Reflection;

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

                var dbContext = ReflectedDbContextFactory.GetDesignTimeDbContextFactoryFromAssembly(
                                    assembly,
                                    dbContextName)
                                ?? ReflectedDbContextFactory.GetDbContextFromAssembly(
                                    assembly,
                                    dbContextName);

                if (dbContext == null)
                {
                    _commandLineJobLogMessageActionsWrapper.FailedToFindDbContext(dbContextName);
                    return 1;
                }

                GenerateFromDbContext(
                    dbContext,
                    outputFilePath);

                return 0;
            });
        }

        private static void GenerateFromDbContext(DbContext dbContext, FileInfo outputFilePath)
        {
            var dgml = dbContext.AsDgml();
            File.WriteAllText(
                outputFilePath.FullName,
                dgml,
                Encoding.UTF8);
        }
    }
}
