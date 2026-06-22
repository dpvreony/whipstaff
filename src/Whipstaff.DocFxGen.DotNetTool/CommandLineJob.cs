// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Whipstaff.CommandLine;
using Whipstaff.CommandLine.DocumentationGenerator;
using Whipstaff.DocFxGen.DotNetTool.CommandLine;

namespace Whipstaff.DocFxGen.DotNetTool
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

                    return 0;
                },
                cancellationToken);
        }
    }
}
