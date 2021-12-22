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
        /// <returns></returns>
        public static Action<ILogger, int, Exception?> GetDbContextSaveResultLoggerMessageAction() => LoggerMessage.Define<int>(
            LogLevel.Debug,
            EventIdFactory.DbContextSaveResultEventId(),
        formatString: "DbContext Save Result: {SaveResult}");

        /// <summary>
        /// Gets the Logger Message definition for "No MediatR handlers registered for type" event.
        /// </summary>
        /// <returns></returns>
        public static Action<ILogger, Type, Exception?> GetNoMediatRHandlersRegisteredForTypeLoggerMessageAction() => LoggerMessage.Define<Type>(
            LogLevel.Debug,
            EventIdFactory.NoMediatRHandlersRegisteredForTypeEventId(),
            formatString: "No MediatR {Type} handlers registered.");

        /// <summary>
        /// Gets the Logger Message definition for the "Number of MediatR handlers registered for type" event.
        /// </summary>
        /// <returns></returns>
        public static Action<ILogger, Type, int, Exception?> GetCountOfMediatRHandlersRegisteredLoggerMessageAction() => LoggerMessage.Define<Type, int>(
            LogLevel.Debug,
            EventIdFactory.CountOfMediatRHandlersRegisteredEventId(),
            formatString: "Number of MediatR {Type} handlers registered: {Count}");
    }
}
