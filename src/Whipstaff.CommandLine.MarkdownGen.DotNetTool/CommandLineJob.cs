// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.IO.Abstractions;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Whipstaff.CommandLine.DocumentationGenerator;
using Whipstaff.CommandLine.MarkdownGen.DotNetTool.CommandLine;
using Whipstaff.Runtime.Extensions;

namespace Whipstaff.CommandLine.MarkdownGen.DotNetTool
{
    /// <summary>
    /// Command line job for handling the creation of the Entity Framework Diagram.
    /// </summary>
    public sealed class CommandLineJob : ICommandLineHandler<CommandLineArgModel>
    {
        private readonly CommandLineJobLogMessageActionsWrapper _commandLineJobLogMessageActionsWrapper;
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineJob"/> class.
        /// </summary>
        /// <param name="commandLineJobLogMessageActionsWrapper">Wrapper for logging framework messages.</param>
        /// <param name="fileSystem">File System abstraction.</param>
        public CommandLineJob(
            CommandLineJobLogMessageActionsWrapper commandLineJobLogMessageActionsWrapper,
            IFileSystem fileSystem)
        {
            ArgumentNullException.ThrowIfNull(commandLineJobLogMessageActionsWrapper);
            ArgumentNullException.ThrowIfNull(fileSystem);

            _commandLineJobLogMessageActionsWrapper = commandLineJobLogMessageActionsWrapper;
            _fileSystem = fileSystem;
        }

        /// <inheritdoc/>
        public Task<int> HandleCommand(CommandLineArgModel commandLineArgModel)
        {
            ArgumentNullException.ThrowIfNull(commandLineArgModel);

            return Task.Run(() =>
            {
                _commandLineJobLogMessageActionsWrapper.StartingHandleCommand();
#pragma warning disable S3885
                var assembly = Assembly.LoadFile(commandLineArgModel.AssemblyPath.FullName);
#pragma warning restore S3885
                var outputFilePath = commandLineArgModel.OutputFilePath;

                var rootCommand = ReflectionHelpers.GetRootCommand(assembly);

                if (rootCommand == null)
                {
                    _commandLineJobLogMessageActionsWrapper.FailedToFindRootCommand();
                    return 1;
                }

                var markdown = MarkdownDocumentationGenerator.GenerateDocumentation(rootCommand);
                _ = _fileSystem.Directory.CreateDirectory(outputFilePath.DirectoryName!);
                _fileSystem.File.WriteAllText(
                    outputFilePath.FullName,
                    markdown,
                    Encoding.UTF8);

                return 0;
            });
        }
    }
}
