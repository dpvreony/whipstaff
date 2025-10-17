// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;
using Whipstaff.Core.Logging;

namespace Whipstaff.EntityFramework.Diagram.DotNetTool
{
    /// <summary>
    /// Log message actions for <see cref="CommandLineJob"/>.
    /// </summary>
    public class CommandLineJobLogMessageActions : ILogMessageActions<CommandLineJob>
    {
        private readonly Action<ILogger, Exception?> _startingHandleCommand;
        private readonly Action<ILogger, string, Exception?> _failedToFindDbContext;
        private readonly Action<ILogger, Exception?> _unhandledException;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineJobLogMessageActions"/> class.
        /// </summary>
        public CommandLineJobLogMessageActions()
        {
            _startingHandleCommand = LoggerMessage.Define(
                LogLevel.Information,
                JobEventIdFactory.StartingHandleCommand(),
                "Starting execution of CLI job handler");

            _failedToFindDbContext = LoggerMessage.Define<string>(
                LogLevel.Information,
                JobEventIdFactory.FailedToFindDbContext(),
                "Failed to find Db Context: {DbContextName}");

            _unhandledException = LoggerMessage.Define(
                LogLevel.Error,
                JobEventIdFactory.UnhandledException(),
                "Unhandled Exception.");
        }

        internal void StartingHandleCommand(ILogger<CommandLineJob> logger)
        {
            _startingHandleCommand(logger, null);
        }

        internal void FailedToFindDbContext(ILogger<CommandLineJob> logger, string dbContextName)
        {
            _failedToFindDbContext(logger, dbContextName, null);
        }

        internal void UnhandledException(ILogger<CommandLineJob> logger, Exception exception)
        {
            _unhandledException(logger, exception);
        }
    }
}
