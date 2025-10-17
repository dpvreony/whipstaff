// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;
using Whipstaff.Core.Logging;
using Whipstaff.Core.Logging.MessageActionWrappers;

namespace Whipstaff.Testing.CommandLine
{
    /// <summary>
    /// LoggerFactory Message Actions Wrapper for the Fake Command Line Handler.
    /// </summary>
    public sealed class FakeCommandLineHandlerLogMessageActionsWrapper : IWrapLogMessageActionUnhandledException, ILogMessageActionsWrapper<FakeCommandLineHandler>
    {
        private readonly Action<ILogger, Exception?> _unhandledException;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeCommandLineHandlerLogMessageActionsWrapper"/> class.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
#pragma warning disable S6672 // Generic logger injection should match enclosing type
        public FakeCommandLineHandlerLogMessageActionsWrapper(ILogger<FakeCommandLineHandler> logger)
#pragma warning restore S6672 // Generic logger injection should match enclosing type
        {
            ArgumentNullException.ThrowIfNull(logger);
            Logger = logger;
            _unhandledException = LoggerMessage.Define(LogLevel.Error, new EventId(1, nameof(UnhandledException)), "Unhandled Exception");
        }

        /// <inheritdoc />
        public ILogger<FakeCommandLineHandler> Logger
        {
            get;
        }

        /// <inheritdoc />
        public void UnhandledException(Exception exception)
        {
            _unhandledException(Logger, exception);
        }
    }
}
