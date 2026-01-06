// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO.Abstractions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Whipstaff.CommandLine.DocumentationGenerator;
using Whipstaff.CommandLine.MarkdownGen.DotNetTool.CommandLine;

namespace Whipstaff.CommandLine.MarkdownGen.DotNetTool
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
        /// <param name="fileSystem">File System abstraction.</param>
        /// <param name="commandLineJobLogMessageActionsWrapper">Wrapper for logging framework messages.</param>
        public CommandLineJob(
            IFileSystem fileSystem,
            CommandLineJobLogMessageActionsWrapper commandLineJobLogMessageActionsWrapper)
            : base(commandLineJobLogMessageActionsWrapper)
        {
            ArgumentNullException.ThrowIfNull(fileSystem);

            _fileSystem = fileSystem;
        }

        /// <inheritdoc/>
        protected override Task<int> OnHandleCommandAsync(CommandLineArgModel commandLineArgModel, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(commandLineArgModel);

            return Task.Run(
                () =>
                {
                    LogMessageActionsWrapper.StartingHandleCommand();
    #pragma warning disable S3885
                    var assembly = Assembly.LoadFile(commandLineArgModel.AssemblyPath.FullName);
    #pragma warning restore S3885
                    var outputFilePath = commandLineArgModel.OutputFilePath;

                    var rootCommand = ReflectionHelpers.GetRootCommand(assembly);

                    if (rootCommand == null)
                    {
                        LogMessageActionsWrapper.FailedToFindRootCommand();
                        return 1;
                    }

                    var markdown = MarkdownDocumentationGenerator.GenerateDocumentation(rootCommand);
                    _ = _fileSystem.Directory.CreateDirectory(outputFilePath.DirectoryName!);
                    _fileSystem.File.WriteAllText(
                        outputFilePath.FullName,
                        markdown,
                        Encoding.UTF8);

                    return 0;
                },
                cancellationToken);
        }
    }
}
