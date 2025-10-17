// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;
using Whipstaff.Core.Logging;
using Whipstaff.Core.Logging.MessageActionWrappers;

namespace Whipstaff.EntityFramework.Diagram.DotNetTool
{
    /// <summary>
    /// Log Message actions wrapper for <see cref="CommandLineJob" />.
    /// </summary>
    public sealed class CommandLineJobLogMessageActionsWrapper : ILogMessageActionsWrapper<CommandLineJob>, IWrapLogMessageActionUnhandledException
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
            ArgumentNullException.ThrowIfNull(commandLineJobLogMessageActions);
            ArgumentNullException.ThrowIfNull(logger);

            _commandLineJobLogMessageActions = commandLineJobLogMessageActions;
            Logger = logger;
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
        /// Log message action for when we failed to find the requested db context.
        /// </summary>
        /// <param name="dbContextName">The name of the db context.</param>
        public void FailedToFindDbContext(string dbContextName)
        {
            _commandLineJobLogMessageActions.FailedToFindDbContext(
                Logger,
                dbContextName);
        }

        /// <inheritdoc />
        public void UnhandledException(Exception exception)
        {
            _commandLineJobLogMessageActions.UnhandledException(Logger, exception);
        }
    }
}
