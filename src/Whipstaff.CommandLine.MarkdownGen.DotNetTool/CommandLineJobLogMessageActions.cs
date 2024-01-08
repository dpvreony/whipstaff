// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;
using Whipstaff.Core.Logging;

namespace Whipstaff.CommandLine.MarkdownGen.DotNetTool
{
    /// <summary>
    /// Log message actions for <see cref="CommandLineJob"/>.
    /// </summary>
    public class CommandLineJobLogMessageActions : ILogMessageActions<CommandLineJob>
    {
        private readonly Action<ILogger, Exception?> _startingHandleCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineJobLogMessageActions"/> class.
        /// </summary>
        public CommandLineJobLogMessageActions()
        {
            _startingHandleCommand = LoggerMessage.Define(
                LogLevel.Information,
                JobEventIdFactory.StartingHandleCommand(),
                "Starting execution of CLI job handler");
        }

        internal void StartingHandleCommand(ILogger<CommandLineJob> logger)
        {
            _startingHandleCommand(logger, null);
        }
    }
}
