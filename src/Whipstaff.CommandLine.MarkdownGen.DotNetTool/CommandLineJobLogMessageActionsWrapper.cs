// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;
using Whipstaff.Core.Logging;

namespace Whipstaff.CommandLine.MarkdownGen.DotNetTool
{
    /// <summary>
    /// Log Message actions wrapper for <see cref="CommandLineJob" />.
    /// </summary>
    public sealed class CommandLineJobLogMessageActionsWrapper : ILogMessageActionsWrapper<CommandLineJob>
    {
        private readonly CommandLineJobLogMessageActions _commandLineJobLogMessageActions;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineJobLogMessageActionsWrapper"/> class.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        /// <param name="commandLineJobLogMessageActions">Log Message actions for <see cref="CommandLineJob" />.</param>
        public CommandLineJobLogMessageActionsWrapper(
            ILogger<CommandLineJob> logger,
            CommandLineJobLogMessageActions commandLineJobLogMessageActions)
        {
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(commandLineJobLogMessageActions);

            Logger = logger;
            _commandLineJobLogMessageActions = commandLineJobLogMessageActions;
        }

        /// <inheritdoc/>
        public ILogger<CommandLineJob> Logger
        {
            get;
        }

        /// <summary>
        /// Log message action for when the command line job is starting.
        /// </summary>
        public void StartingHandleCommand()
        {
            _commandLineJobLogMessageActions.StartingHandleCommand(Logger);
        }
    }
}
