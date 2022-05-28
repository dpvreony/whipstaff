// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;

namespace Whipstaff.Core.Logging
{
    /// <summary>
    /// Factory Methods for Logger Messages.
    /// </summary>
    public static class LoggerMessageFactory
    {
        /// <summary>
        /// Gets the Logger Message definition for the DbContext Save Result event.
        /// </summary>
        /// <returns>Log Message Action.</returns>
        public static Action<ILogger, int, Exception?> GetDbContextSaveResultLoggerMessageAction() => LoggerMessage.Define<int>(
            LogLevel.Debug,
            EventIdFactory.DbContextSaveResultEventId(),
        formatString: "DbContext Save Result: {SaveResult}");

        /// <summary>
        /// Gets the Logger Message definition for "No MediatR handlers registered for type" event.
        /// </summary>
        /// <returns>Log Message Action.</returns>
        public static Action<ILogger, Type, Exception?> GetNoMediatRHandlersRegisteredForTypeLoggerMessageAction() => LoggerMessage.Define<Type>(
            LogLevel.Debug,
            EventIdFactory.NoMediatRHandlersRegisteredForTypeEventId(),
            formatString: "No MediatR {Type} handlers registered.");

        /// <summary>
        /// Gets the Logger Message definition for the "Number of MediatR handlers registered for type" event.
        /// </summary>
        /// <returns>Log Message Action.</returns>
        public static Action<ILogger, Type, int, Exception?> GetCountOfMediatRHandlersRegisteredLoggerMessageAction() => LoggerMessage.Define<Type, int>(
            LogLevel.Debug,
            EventIdFactory.CountOfMediatRHandlersRegisteredEventId(),
            formatString: "Number of MediatR {Type} handlers registered: {Count}");

        /// <summary>
        /// Gets a basic debug logger message action for an event id. Useful for basic logging of events where there is only
        /// ever a basic message.
        /// </summary>
        /// <param name="eventId">The event id to define a log message action for.</param>
        /// <returns>Log Message Action.</returns>
        public static Action<ILogger, string, Exception?> GetCriticalBasicLoggerMessageActionForEventId(EventId eventId) =>
            GetBasicLoggerMessageActionForLogLevelAndEventId(
                LogLevel.Critical,
                eventId);

        /// <summary>
        /// Gets a basic debug logger message action for an event id. Useful for basic logging of events where there is only
        /// ever a basic message.
        /// </summary>
        /// <param name="eventId">The event id to define a log message action for.</param>
        /// <returns>Log Message Action.</returns>
        public static Action<ILogger, string, Exception?> GetDebugBasicLoggerMessageActionForEventId(EventId eventId) =>
            GetBasicLoggerMessageActionForLogLevelAndEventId(
                LogLevel.Debug,
                eventId);

        /// <summary>
        /// Gets a basic error logger message action for an event id. Useful for basic logging of events where there is only
        /// ever a basic message.
        /// </summary>
        /// <param name="eventId">The event id to define a log message action for.</param>
        /// <returns>Log Message Action.</returns>
        public static Action<ILogger, string, Exception?> GetErrorBasicLoggerMessageActionForEventId(EventId eventId) =>
            GetBasicLoggerMessageActionForLogLevelAndEventId(
                LogLevel.Error,
                eventId);
        
        /// <summary>
        /// Gets a basic information logger message action for an event id. Useful for basic logging of events where there is only
        /// ever a basic message.
        /// </summary>
        /// <param name="eventId">The event id to define a log message action for.</param>
        /// <returns>Log Message Action.</returns>
        public static Action<ILogger, string, Exception?> GetInformationBasicLoggerMessageActionForEventId(EventId eventId) =>
            GetBasicLoggerMessageActionForLogLevelAndEventId(
                LogLevel.Information,
                eventId);

        /// <summary>
        /// Gets a basic information logger message action for an event id. Useful for basic logging of events where there is only
        /// ever a basic message.
        /// </summary>
        /// <param name="eventId">The event id to define a log message action for.</param>
        /// <returns>Log Message Action.</returns>
        public static Action<ILogger, string, Exception?> GetWarningBasicLoggerMessageActionForEventId(EventId eventId) =>
            GetBasicLoggerMessageActionForLogLevelAndEventId(
                LogLevel.Warning,
                eventId);

        /// <summary>
        /// Gets a basic log message action where there will only ever be a basic message.
        /// </summary>
        /// <param name="logLevel">The logging level.</param>
        /// <param name="eventId">The event id.</param>
        /// <returns>Log Message Action.</returns>
        public static Action<ILogger, string, Exception?> GetBasicLoggerMessageActionForLogLevelAndEventId(
            LogLevel logLevel,
            EventId eventId) =>
            LoggerMessage.Define<string>(
                logLevel,
                eventId,
                "{Message}");
    }
}
