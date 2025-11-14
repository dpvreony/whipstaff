// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;
using Whipstaff.Core.Logging;
using Whipstaff.Core.Logging.MessageActionWrappers;

namespace Whipstaff.CommandLine.MarkdownGen.DotNetTool
{
    /// <summary>
    /// Log Message actions wrapper for <see cref="CommandLineJob" />.
    /// </summary>
    internal sealed class CommandLineJobLogMessageActionsWrapper : ILogMessageActionsWrapper<CommandLineJob>, IWrapLogMessageActionUnhandledException
    {
        private readonly CommandLineJobLogMessageActions _commandLineJobLogMessageActions;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineJobLogMessageActionsWrapper"/> class.
        /// </summary>
        /// <param name="commandLineJobLogMessageActions">Log Message actions for <see cref="CommandLineJob" />.</param>
        /// <param name="logger">Logging framework instance.</param>
        public CommandLineJobLogMessageActionsWrapper(
            CommandLineJobLogMessageActions commandLineJobLogMessageActions,
#pragma warning disable S6672
            ILogger<CommandLineJob> logger)
#pragma warning restore S6672
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

        /// <summary>
        /// Log message action for when the command line job fails to find the root command.
        /// </summary>
        public void FailedToFindRootCommand()
        {
            _commandLineJobLogMessageActions.FailedToFindRootCommand(Logger);
        }

        /// <inheritdoc />
        public void UnhandledException(Exception exception)
        {
            _commandLineJobLogMessageActions.UnhandledException(Logger, exception);
        }
    }
}
