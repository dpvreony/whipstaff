// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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
    internal sealed class CommandLineJob : AbstractCommandLineHandler<CommandLineArgModel, CommandLineJobLogMessageActionsWrapper>
    {
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineJob"/> class.
        /// </summary>
        /// <param name="commandLineJobLogMessageActionsWrapper">Wrapper for logging framework messages.</param>
        /// <param name="fileSystem">File System abstraction.</param>
        public CommandLineJob(
            CommandLineJobLogMessageActionsWrapper commandLineJobLogMessageActionsWrapper,
            IFileSystem fileSystem)
            : base(commandLineJobLogMessageActionsWrapper)
        {
            ArgumentNullException.ThrowIfNull(fileSystem);

            _fileSystem = fileSystem;
        }

        /// <inheritdoc/>
        protected override Task<int> OnHandleCommand(CommandLineArgModel commandLineArgModel, CancellationToken cancellationToken)
        {
            return Task.Run(
                () =>
                {
                    var appDomain = AppDomain.CurrentDomain;
                    var loadedAssemblies = new List<string>();
                    appDomain.AssemblyResolve += (_, args) =>
                    {
                        var assemblyName = new AssemblyName(args.Name);

                        if (args.RequestingAssembly?.Location.Equals(commandLineArgModel.AssemblyPath.FullName, StringComparison.Ordinal) != true
                            && loadedAssemblies.Exists(la => la.Equals(args.RequestingAssembly?.Location, StringComparison.Ordinal)))
                        {
                            return null;
                        }

                        var assemblyPath = _fileSystem.Path.Combine(commandLineArgModel.AssemblyPath.DirectoryName!, $"{assemblyName.Name}.dll");

                        if (_fileSystem.File.Exists(assemblyPath))
                        {
                            loadedAssemblies.Add(assemblyPath);
    #pragma warning disable S3885
                            return Assembly.LoadFrom(assemblyPath);
    #pragma warning restore S3885
                        }

                        return null;
                    };

                    LogMessageActionsWrapper.StartingHandleCommand();

    #pragma warning disable S3885
                    var assembly = Assembly.LoadFrom(commandLineArgModel.AssemblyPath.FullName);
    #pragma warning restore S3885

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
                        LogMessageActionsWrapper.FailedToFindDbContext(dbContextName);
                        return 1;
                    }

                    GenerateFromDbContext(
                        dbContext,
                        _fileSystem,
                        outputFilePath);

                    return 0;
                },
                cancellationToken);
        }

        private static void GenerateFromDbContext(DbContext dbContext, IFileSystem fileSystem, FileInfo outputFilePath)
        {
            var dgml = dbContext.AsDgml();
            _ = fileSystem.Directory.CreateDirectory(outputFilePath.DirectoryName!);
            fileSystem.File.WriteAllText(
                outputFilePath.FullName,
                dgml,
                Encoding.UTF8);
        }
    }
}
