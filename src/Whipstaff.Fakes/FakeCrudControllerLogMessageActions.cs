using System;
using Microsoft.Extensions.Logging;
using Whipstaff.AspNetCore.Features.Logging;
using Whipstaff.Core.Logging;

namespace Whipstaff.Testing
{
    /// <summary>
    /// Log Message Actions for the Fake CRUD controller.
    /// </summary>
    public sealed class FakeCrudControllerLogMessageActions : ICrudControllerLogMessageActions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeCrudControllerLogMessageActions"/> class.
        /// </summary>
        public FakeCrudControllerLogMessageActions()
        {
            AddEventLogMessageAction = LoggerMessageFactory.GetDebugBasicLoggerMessageActionForEventId(new EventId(1));
            DeleteEventLogMessageAction = LoggerMessageFactory.GetDebugBasicLoggerMessageActionForEventId(new EventId(2));
            ListEventLogMessageAction = LoggerMessageFactory.GetDebugBasicLoggerMessageActionForEventId(new EventId(3));
            UpdateEventLogMessageAction = LoggerMessageFactory.GetDebugBasicLoggerMessageActionForEventId(new EventId(4));
            ViewEventLogMessageAction = LoggerMessageFactory.GetDebugBasicLoggerMessageActionForEventId(new EventId(5));
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
