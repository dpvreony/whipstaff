// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;
using Whipstaff.AspNetCore.Features.Logging;
using Whipstaff.Core.Logging;

namespace Whipstaff.Testing
{
    /// <summary>
    /// LoggerFactory Message Actions for the Fake CRUD controller.
    /// </summary>
    public sealed class FakeCrudControllerLogMessageActions : ICrudControllerLogMessageActions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeCrudControllerLogMessageActions"/> class.
        /// </summary>
        public FakeCrudControllerLogMessageActions()
        {
#pragma warning disable GR0012 // Constructors should minimise work and not execute methods
            AddEventLogMessageAction = LoggerMessageFactory.GetDebugBasicLoggerMessageActionForEventId(new EventId(1));
            DeleteEventLogMessageAction = LoggerMessageFactory.GetDebugBasicLoggerMessageActionForEventId(new EventId(2));
            ListEventLogMessageAction = LoggerMessageFactory.GetDebugBasicLoggerMessageActionForEventId(new EventId(3));
            UpdateEventLogMessageAction = LoggerMessageFactory.GetDebugBasicLoggerMessageActionForEventId(new EventId(4));
            ViewEventLogMessageAction = LoggerMessageFactory.GetDebugBasicLoggerMessageActionForEventId(new EventId(5));
#pragma warning restore GR0012 // Constructors should minimise work and not execute methods
        }

        /// <inheritdoc />
        public Action<ILogger, string, Exception?> ListEventLogMessageAction { get; init; }

        /// <inheritdoc />
        public Action<ILogger, string, Exception?> ViewEventLogMessageAction { get; init; }

        /// <inheritdoc />
        public Action<ILogger, string, Exception?> AddEventLogMessageAction { get; init; }

        /// <inheritdoc />
        public Action<ILogger, string, Exception?> DeleteEventLogMessageAction { get; init; }

        /// <inheritdoc />
        public Action<ILogger, string, Exception?> UpdateEventLogMessageAction { get; init; }
    }
}
