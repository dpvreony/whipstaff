// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Logging;

namespace Whipstaff.Core.Logging
{
    /// <summary>
    /// Factory Methods for Logger Messages. These are aimed at simplifying some use cases for logging.
    ///
    /// There are methods that allow passing in functions to the logging so that you can delay the generation \ evaluation
    /// of any output. This can help with avoiding expensive operations when a log level may not be enabled.
    ///
    /// There are also some common log message actions that are used in downstream code, including Roslyn Source generators
    /// sat in Nucleotide.
    /// </summary>
    /// <remarks>
    /// This code is based upon https://github.com/dotnet/runtime/blob/e8a85b78f804578729392acd9d6307918c3b23f5/src/libraries/Microsoft.Extensions.Logging.Abstractions/src/LoggerMessage.cs
    /// which carries the following license.
    ///
    /// Licensed to the .NET Foundation under one or more agreements.
    /// The .NET Foundation licenses this file to you under the MIT license.
    /// </remarks>
    public static class LoggerMessageFactory
    {
        /// <summary>
        /// Gets the Logger Message definition for the DbContext Save Result event.
        /// </summary>
        /// <returns>Log Message Action.</returns>
        public static Action<ILogger, int, Exception?> GetDbContextSaveResultLoggerMessageAction() => LoggerMessage.Define<int>(
            LogLevel.Debug,
            WhipstaffEventIdFactory.DbContextSaveResultEventId(),
            formatString: "DbContext Save Result: {SaveResult}");

        /// <summary>
        /// Gets the Logger Message definition for "No Mediator handlers registered for type" event.
        /// </summary>
        /// <returns>Log Message Action.</returns>
        public static Action<ILogger, Type, Exception?> GetNoMediatorHandlersRegisteredForTypeLoggerMessageAction() => LoggerMessage.Define<Type>(
            LogLevel.Debug,
            WhipstaffEventIdFactory.NoMediatorHandlersRegisteredForTypeEventId(),
            formatString: "No Mediator {Type} handlers registered.");

        /// <summary>
        /// Gets the Logger Message definition for the "Number of Mediator handlers registered for type" event.
        /// </summary>
        /// <returns>Log Message Action.</returns>
        public static Action<ILogger, Type, int, Exception?> GetCountOfMediatorHandlersRegisteredLoggerMessageAction() => LoggerMessage.Define<Type, int>(
            LogLevel.Debug,
            WhipstaffEventIdFactory.CountOfMediatorHandlersRegisteredEventId(),
            formatString: "Number of Mediator {Type} handlers registered: {Count}");

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
        public static Action<ILogger, Func<string>, Exception?> GetCriticalBasicLoggerMessageActionForEventIdAndFunc(EventId eventId) =>
            GetBasicLoggerMessageActionForLogLevelAndEventIdAndFunc(
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
        /// Gets a basic debug logger message action for an event id. Useful for basic logging of events where there is only
        /// ever a basic message.
        /// </summary>
        /// <param name="eventId">The event id to define a log message action for.</param>
        /// <returns>Log Message Action.</returns>
        public static Action<ILogger, Func<string>, Exception?> GetDebugBasicLoggerMessageActionForEventIdAndFunc(EventId eventId) =>
            GetBasicLoggerMessageActionForLogLevelAndEventIdAndFunc(
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
        /// Gets a basic error logger message action for an event id. Useful for basic logging of events where there is only
        /// ever a basic message.
        /// </summary>
        /// <param name="eventId">The event id to define a log message action for.</param>
        /// <returns>Log Message Action.</returns>
        public static Action<ILogger, Func<string>, Exception?> GetErrorBasicLoggerMessageActionForEventIdAndFunc(EventId eventId) =>
            GetBasicLoggerMessageActionForLogLevelAndEventIdAndFunc(
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
        public static Action<ILogger, Func<string>, Exception?> GetInformationBasicLoggerMessageActionForEventIdAndFunc(EventId eventId) =>
            GetBasicLoggerMessageActionForLogLevelAndEventIdAndFunc(
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
        /// Gets a basic information logger message action for an event id. Useful for basic logging of events where there is only
        /// ever a basic message.
        /// </summary>
        /// <param name="eventId">The event id to define a log message action for.</param>
        /// <returns>Log Message Action.</returns>
        public static Action<ILogger, Func<string>, Exception?> GetWarningBasicLoggerMessageActionForEventIdAndFunc(EventId eventId) =>
            GetBasicLoggerMessageActionForLogLevelAndEventIdAndFunc(
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

        /// <summary>
        /// Gets a basic log message action where there will only ever be a basic message.
        /// </summary>
        /// <param name="logLevel">The logging level.</param>
        /// <param name="eventId">The event id.</param>
        /// <returns>Log Message Action.</returns>
        public static Action<ILogger, Func<string>, Exception?> GetBasicLoggerMessageActionForLogLevelAndEventIdAndFunc(
            LogLevel logLevel,
            EventId eventId) =>
            DefineForFunc<string>(
                logLevel,
                eventId,
                "{Message}");

        /// <summary>
        /// Creates a delegate which can be invoked for logging a message.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter passed to the named format string.</typeparam>
        /// <param name="logLevel">The <see cref="LogLevel"/>.</param>
        /// <param name="eventId">The event id.</param>
        /// <param name="formatString">The named format string.</param>
        /// <returns>A delegate which when invoked creates a log message.</returns>
        public static Action<ILogger, Func<T1>, Exception?> DefineForFunc<T1>(LogLevel logLevel, EventId eventId, string formatString)
            => DefineForFunc<T1>(logLevel, eventId, formatString, options: null);

        /// <summary>
        /// Creates a delegate which can be invoked for logging a message.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter passed to the named format string.</typeparam>
        /// <param name="logLevel">The <see cref="LogLevel"/>.</param>
        /// <param name="eventId">The event id.</param>
        /// <param name="formatString">The named format string.</param>
        /// <param name="options">The <see cref="LogDefineOptions"/>.</param>
        /// <returns>A delegate which when invoked creates a log message.</returns>
        public static Action<ILogger, Func<T1>, Exception?> DefineForFunc<T1>(LogLevel logLevel, EventId eventId, string formatString, LogDefineOptions? options)
        {
            LogValuesFormatter formatter = CreateLogValuesFormatter(formatString, expectedNamedParameterCount: 1);

            void Log(ILogger logger, Func<T1> arg1, Exception? exception)
            {
                logger.Log(logLevel, eventId, new LogValues<T1>(formatter, arg1()), exception, LogValues<T1>.Callback);
            }

            if (options != null && options.SkipEnabledCheck)
            {
                return Log;
            }

            return (logger, arg1, exception) =>
            {
                if (logger.IsEnabled(logLevel))
                {
                    Log(logger, arg1, exception);
                }
            };
        }

        /// <summary>
        /// Creates a delegate which can be invoked for logging a message.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter passed to the named format string.</typeparam>
        /// <typeparam name="T2">The type of the second parameter passed to the named format string.</typeparam>
        /// <param name="logLevel">The <see cref="LogLevel"/>.</param>
        /// <param name="eventId">The event id.</param>
        /// <param name="formatString">The named format string.</param>
        /// <returns>A delegate which when invoked creates a log message.</returns>
        public static Action<ILogger, Func<T1>, Func<T2>, Exception?> DefineForFunc<T1, T2>(LogLevel logLevel, EventId eventId, string formatString)
            => DefineForFunc<T1, T2>(logLevel, eventId, formatString, options: null);

        /// <summary>
        /// Creates a delegate which can be invoked for logging a message.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter passed to the named format string.</typeparam>
        /// <typeparam name="T2">The type of the second parameter passed to the named format string.</typeparam>
        /// <param name="logLevel">The <see cref="LogLevel"/>.</param>
        /// <param name="eventId">The event id.</param>
        /// <param name="formatString">The named format string.</param>
        /// <param name="options">The <see cref="LogDefineOptions"/>.</param>
        /// <returns>A delegate which when invoked creates a log message.</returns>
        public static Action<ILogger, Func<T1>, Func<T2>, Exception?> DefineForFunc<T1, T2>(LogLevel logLevel, EventId eventId, string formatString, LogDefineOptions? options)
        {
            LogValuesFormatter formatter = CreateLogValuesFormatter(formatString, expectedNamedParameterCount: 2);

            void Log(ILogger logger, Func<T1> arg1, Func<T2> arg2, Exception? exception)
            {
                logger.Log(logLevel, eventId, new LogValues<T1, T2>(formatter, arg1(), arg2()), exception, LogValues<T1, T2>.Callback);
            }

            if (options != null && options.SkipEnabledCheck)
            {
                return Log;
            }

            return (logger, arg1, arg2, exception) =>
            {
                if (logger.IsEnabled(logLevel))
                {
                    Log(logger, arg1, arg2, exception);
                }
            };
        }

        /// <summary>
        /// Creates a delegate which can be invoked for logging a message.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter passed to the named format string.</typeparam>
        /// <typeparam name="T2">The type of the second parameter passed to the named format string.</typeparam>
        /// <typeparam name="T3">The type of the third parameter passed to the named format string.</typeparam>
        /// <param name="logLevel">The <see cref="LogLevel"/>.</param>
        /// <param name="eventId">The event id.</param>
        /// <param name="formatString">The named format string.</param>
        /// <returns>A delegate which when invoked creates a log message.</returns>
        public static Action<ILogger, Func<T1>, Func<T2>, Func<T3>, Exception?> DefineForFunc<T1, T2, T3>(LogLevel logLevel, EventId eventId, string formatString)
            => DefineForFunc<T1, T2, T3>(logLevel, eventId, formatString, options: null);

        /// <summary>
        /// Creates a delegate which can be invoked for logging a message.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter passed to the named format string.</typeparam>
        /// <typeparam name="T2">The type of the second parameter passed to the named format string.</typeparam>
        /// <typeparam name="T3">The type of the third parameter passed to the named format string.</typeparam>
        /// <param name="logLevel">The <see cref="LogLevel"/>.</param>
        /// <param name="eventId">The event id.</param>
        /// <param name="formatString">The named format string.</param>
        /// <param name="options">The <see cref="LogDefineOptions"/>.</param>
        /// <returns>A delegate which when invoked creates a log message.</returns>
        public static Action<ILogger, Func<T1>, Func<T2>, Func<T3>, Exception?> DefineForFunc<T1, T2, T3>(LogLevel logLevel, EventId eventId, string formatString, LogDefineOptions? options)
        {
            LogValuesFormatter formatter = CreateLogValuesFormatter(formatString, expectedNamedParameterCount: 3);

            void Log(ILogger logger, Func<T1> arg1, Func<T2> arg2, Func<T3> arg3, Exception? exception)
            {
                logger.Log(logLevel, eventId, new LogValues<T1, T2, T3>(formatter, arg1(), arg2(), arg3()), exception, LogValues<T1, T2, T3>.Callback);
            }

            if (options != null && options.SkipEnabledCheck)
            {
                return Log;
            }

            return (logger, arg1, arg2, arg3, exception) =>
            {
                if (logger.IsEnabled(logLevel))
                {
                    Log(logger, arg1, arg2, arg3, exception);
                }
            };
        }

        /// <summary>
        /// Creates a delegate which can be invoked for logging a message.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter passed to the named format string.</typeparam>
        /// <typeparam name="T2">The type of the second parameter passed to the named format string.</typeparam>
        /// <typeparam name="T3">The type of the third parameter passed to the named format string.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter passed to the named format string.</typeparam>
        /// <param name="logLevel">The <see cref="LogLevel"/>.</param>
        /// <param name="eventId">The event id.</param>
        /// <param name="formatString">The named format string.</param>
        /// <returns>A delegate which when invoked creates a log message.</returns>
        public static Action<ILogger, Func<T1>, Func<T2>, Func<T3>, Func<T4>, Exception?> DefineForFunc<T1, T2, T3, T4>(LogLevel logLevel, EventId eventId, string formatString)
            => DefineForFunc<T1, T2, T3, T4>(logLevel, eventId, formatString, options: null);

        /// <summary>
        /// Creates a delegate which can be invoked for logging a message.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter passed to the named format string.</typeparam>
        /// <typeparam name="T2">The type of the second parameter passed to the named format string.</typeparam>
        /// <typeparam name="T3">The type of the third parameter passed to the named format string.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter passed to the named format string.</typeparam>
        /// <param name="logLevel">The <see cref="LogLevel"/>.</param>
        /// <param name="eventId">The event id.</param>
        /// <param name="formatString">The named format string.</param>
        /// <param name="options">The <see cref="LogDefineOptions"/>.</param>
        /// <returns>A delegate which when invoked creates a log message.</returns>
        public static Action<ILogger, Func<T1>, Func<T2>, Func<T3>, Func<T4>, Exception?> DefineForFunc<T1, T2, T3, T4>(LogLevel logLevel, EventId eventId, string formatString, LogDefineOptions? options)
        {
            LogValuesFormatter formatter = CreateLogValuesFormatter(formatString, expectedNamedParameterCount: 4);

            void Log(ILogger logger, Func<T1> arg1, Func<T2> arg2, Func<T3> arg3, Func<T4> arg4, Exception? exception)
            {
                logger.Log(logLevel, eventId, new LogValues<T1, T2, T3, T4>(formatter, arg1(), arg2(), arg3(), arg4()), exception, LogValues<T1, T2, T3, T4>.Callback);
            }

            if (options != null && options.SkipEnabledCheck)
            {
                return Log;
            }

            return (logger, arg1, arg2, arg3, arg4, exception) =>
            {
                if (logger.IsEnabled(logLevel))
                {
                    Log(logger, arg1, arg2, arg3, arg4, exception);
                }
            };
        }

        /// <summary>
        /// Creates a delegate which can be invoked for logging a message.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter passed to the named format string.</typeparam>
        /// <typeparam name="T2">The type of the second parameter passed to the named format string.</typeparam>
        /// <typeparam name="T3">The type of the third parameter passed to the named format string.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter passed to the named format string.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter passed to the named format string.</typeparam>
        /// <param name="logLevel">The <see cref="LogLevel"/>.</param>
        /// <param name="eventId">The event id.</param>
        /// <param name="formatString">The named format string.</param>
        /// <returns>A delegate which when invoked creates a log message.</returns>
        public static Action<ILogger, Func<T1>, Func<T2>, Func<T3>, Func<T4>, Func<T5>, Exception?> DefineForFunc<T1, T2, T3, T4, T5>(LogLevel logLevel, EventId eventId, string formatString)
            => DefineForFunc<T1, T2, T3, T4, T5>(logLevel, eventId, formatString, options: null);

        /// <summary>
        /// Creates a delegate which can be invoked for logging a message.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter passed to the named format string.</typeparam>
        /// <typeparam name="T2">The type of the second parameter passed to the named format string.</typeparam>
        /// <typeparam name="T3">The type of the third parameter passed to the named format string.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter passed to the named format string.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter passed to the named format string.</typeparam>
        /// <param name="logLevel">The <see cref="LogLevel"/>.</param>
        /// <param name="eventId">The event id.</param>
        /// <param name="formatString">The named format string.</param>
        /// <param name="options">The <see cref="LogDefineOptions"/>.</param>
        /// <returns>A delegate which when invoked creates a log message.</returns>
        public static Action<ILogger, Func<T1>, Func<T2>, Func<T3>, Func<T4>, Func<T5>, Exception?> DefineForFunc<T1, T2, T3, T4, T5>(LogLevel logLevel, EventId eventId, string formatString, LogDefineOptions? options)
        {
            LogValuesFormatter formatter = CreateLogValuesFormatter(formatString, expectedNamedParameterCount: 5);

            void Log(ILogger logger, Func<T1> arg1, Func<T2> arg2, Func<T3> arg3, Func<T4> arg4, Func<T5> arg5, Exception? exception)
            {
                logger.Log(logLevel, eventId, new LogValues<T1, T2, T3, T4, T5>(formatter, arg1(), arg2(), arg3(), arg4(), arg5()), exception, LogValues<T1, T2, T3, T4, T5>.Callback);
            }

            if (options != null && options.SkipEnabledCheck)
            {
                return Log;
            }

            return (logger, arg1, arg2, arg3, arg4, arg5, exception) =>
            {
                if (logger.IsEnabled(logLevel))
                {
                    Log(logger, arg1, arg2, arg3, arg4, arg5, exception);
                }
            };
        }

        /// <summary>
        /// Creates a delegate which can be invoked for logging a message.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter passed to the named format string.</typeparam>
        /// <typeparam name="T2">The type of the second parameter passed to the named format string.</typeparam>
        /// <typeparam name="T3">The type of the third parameter passed to the named format string.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter passed to the named format string.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter passed to the named format string.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter passed to the named format string.</typeparam>
        /// <param name="logLevel">The <see cref="LogLevel"/>.</param>
        /// <param name="eventId">The event id.</param>
        /// <param name="formatString">The named format string.</param>
        /// <returns>A delegate which when invoked creates a log message.</returns>
        public static Action<ILogger, Func<T1>, Func<T2>, Func<T3>, Func<T4>, Func<T5>, Func<T6>, Exception?> DefineForFunc<T1, T2, T3, T4, T5, T6>(LogLevel logLevel, EventId eventId, string formatString)
            => DefineForFunc<T1, T2, T3, T4, T5, T6>(logLevel, eventId, formatString, options: null);

        /// <summary>
        /// Creates a delegate which can be invoked for logging a message.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter passed to the named format string.</typeparam>
        /// <typeparam name="T2">The type of the second parameter passed to the named format string.</typeparam>
        /// <typeparam name="T3">The type of the third parameter passed to the named format string.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter passed to the named format string.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter passed to the named format string.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter passed to the named format string.</typeparam>
        /// <param name="logLevel">The <see cref="LogLevel"/>.</param>
        /// <param name="eventId">The event id.</param>
        /// <param name="formatString">The named format string.</param>
        /// <param name="options">The <see cref="LogDefineOptions"/>.</param>
        /// <returns>A delegate which when invoked creates a log message.</returns>
        public static Action<ILogger, Func<T1>, Func<T2>, Func<T3>, Func<T4>, Func<T5>, Func<T6>, Exception?> DefineForFunc<T1, T2, T3, T4, T5, T6>(LogLevel logLevel, EventId eventId, string formatString, LogDefineOptions? options)
        {
            LogValuesFormatter formatter = CreateLogValuesFormatter(formatString, expectedNamedParameterCount: 6);

            void Log(ILogger logger, Func<T1> arg1, Func<T2> arg2, Func<T3> arg3, Func<T4> arg4, Func<T5> arg5, Func<T6> arg6, Exception? exception)
            {
                logger.Log(logLevel, eventId, new LogValues<T1, T2, T3, T4, T5, T6>(formatter, arg1(), arg2(), arg3(), arg4(), arg5(), arg6()), exception, LogValues<T1, T2, T3, T4, T5, T6>.Callback);
            }

            if (options != null && options.SkipEnabledCheck)
            {
                return Log;
            }

            return (logger, arg1, arg2, arg3, arg4, arg5, arg6, exception) =>
            {
                if (logger.IsEnabled(logLevel))
                {
                    Log(logger, arg1, arg2, arg3, arg4, arg5, arg6, exception);
                }
            };
        }

        private static LogValuesFormatter CreateLogValuesFormatter(string formatString, int expectedNamedParameterCount)
        {
            var logValuesFormatter = new LogValuesFormatter(formatString);

            int actualCount = logValuesFormatter.ValueNames.Count;
            if (actualCount != expectedNamedParameterCount)
            {
                throw new ArgumentException(
                    SR.Format(CultureInfo.InvariantCulture, "The format string '{0}' does not have the expected number of named parameters. Expected {1} parameter(s) but found {2} parameter(s).", formatString, expectedNamedParameterCount, actualCount));
            }

            return logValuesFormatter;
        }

        private readonly struct LogValues<T0> : IReadOnlyList<KeyValuePair<string, object?>>
        {
            public static readonly Func<LogValues<T0>, Exception?, string> Callback = (state, exception) => state.ToString();

            private readonly LogValuesFormatter _formatter;
            private readonly T0 _value0;

            public LogValues(LogValuesFormatter formatter, T0 value0)
            {
                _formatter = formatter;
                _value0 = value0;
            }

            public KeyValuePair<string, object?> this[int index]
            {
                get
                {
                    switch (index)
                    {
                        case 0:
                            return new KeyValuePair<string, object?>(_formatter.ValueNames[0], _value0);
                        case 1:
                            return new KeyValuePair<string, object?>("{OriginalFormat}", _formatter.OriginalFormat);
                        default:
#pragma warning disable CA2201 // Do not raise reserved exception types
#pragma warning disable S112 // General or reserved exceptions should never be thrown
                            throw new IndexOutOfRangeException(nameof(index));
#pragma warning restore S112 // General or reserved exceptions should never be thrown
#pragma warning restore CA2201 // Do not raise reserved exception types
                    }
                }
            }

#pragma warning disable SA1201 // Elements should appear in the correct order
            public int Count => 2;
#pragma warning restore SA1201 // Elements should appear in the correct order

            public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
            {
                for (int i = 0; i < Count; ++i)
                {
                    yield return this[i];
                }
            }

            public override string ToString() => _formatter.Format(_value0);

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private readonly struct LogValues<T0, T1> : IReadOnlyList<KeyValuePair<string, object?>>
        {
            public static readonly Func<LogValues<T0, T1>, Exception?, string> Callback = (state, exception) => state.ToString();

            private readonly LogValuesFormatter _formatter;
            private readonly T0 _value0;
            private readonly T1 _value1;

            public LogValues(LogValuesFormatter formatter, T0 value0, T1 value1)
            {
                _formatter = formatter;
                _value0 = value0;
                _value1 = value1;
            }

            public KeyValuePair<string, object?> this[int index]
            {
                get
                {
                    switch (index)
                    {
                        case 0:
                            return new KeyValuePair<string, object?>(_formatter.ValueNames[0], _value0);
                        case 1:
                            return new KeyValuePair<string, object?>(_formatter.ValueNames[1], _value1);
                        case 2:
                            return new KeyValuePair<string, object?>("{OriginalFormat}", _formatter.OriginalFormat);
                        default:
#pragma warning disable CA2201 // Do not raise reserved exception types
#pragma warning disable S112
                            throw new IndexOutOfRangeException(nameof(index));
#pragma warning restore S112
#pragma warning restore CA2201 // Do not raise reserved exception types
                    }
                }
            }

#pragma warning disable SA1201 // Elements should appear in the correct order
            public int Count => 3;
#pragma warning restore SA1201 // Elements should appear in the correct order

            public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
            {
                for (int i = 0; i < Count; ++i)
                {
                    yield return this[i];
                }
            }

            public override string ToString() => _formatter.Format(_value0, _value1);

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private readonly struct LogValues<T0, T1, T2> : IReadOnlyList<KeyValuePair<string, object?>>
        {
            public static readonly Func<LogValues<T0, T1, T2>, Exception?, string> Callback = (state, exception) => state.ToString();

            private readonly LogValuesFormatter _formatter;
            private readonly T0 _value0;
            private readonly T1 _value1;
            private readonly T2 _value2;

            public int Count => 4;

            public KeyValuePair<string, object?> this[int index]
            {
                get
                {
                    switch (index)
                    {
                        case 0:
                            return new KeyValuePair<string, object?>(_formatter.ValueNames[0], _value0);
                        case 1:
                            return new KeyValuePair<string, object?>(_formatter.ValueNames[1], _value1);
                        case 2:
                            return new KeyValuePair<string, object?>(_formatter.ValueNames[2], _value2);
                        case 3:
                            return new KeyValuePair<string, object?>("{OriginalFormat}", _formatter.OriginalFormat);
                        default:
#pragma warning disable CA2201 // Do not raise reserved exception types
#pragma warning disable S112 // General or reserved exceptions should never be thrown
                            throw new IndexOutOfRangeException(nameof(index));
#pragma warning restore S112 // General or reserved exceptions should never be thrown
#pragma warning restore CA2201 // Do not raise reserved exception types
                    }
                }
            }

#pragma warning disable SA1201 // Elements should appear in the correct order
            public LogValues(LogValuesFormatter formatter, T0 value0, T1 value1, T2 value2)
#pragma warning restore SA1201 // Elements should appear in the correct order
            {
                _formatter = formatter;
                _value0 = value0;
                _value1 = value1;
                _value2 = value2;
            }

            public override string ToString() => _formatter.Format(_value0, _value1, _value2);

            public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
            {
                for (int i = 0; i < Count; ++i)
                {
                    yield return this[i];
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private readonly struct LogValues<T0, T1, T2, T3> : IReadOnlyList<KeyValuePair<string, object?>>
        {
            public static readonly Func<LogValues<T0, T1, T2, T3>, Exception?, string> Callback = (state, exception) => state.ToString();

            private readonly LogValuesFormatter _formatter;
            private readonly T0 _value0;
            private readonly T1 _value1;
            private readonly T2 _value2;
            private readonly T3 _value3;

            public int Count => 5;

            public KeyValuePair<string, object?> this[int index]
            {
                get
                {
                    switch (index)
                    {
                        case 0:
                            return new KeyValuePair<string, object?>(_formatter.ValueNames[0], _value0);
                        case 1:
                            return new KeyValuePair<string, object?>(_formatter.ValueNames[1], _value1);
                        case 2:
                            return new KeyValuePair<string, object?>(_formatter.ValueNames[2], _value2);
                        case 3:
                            return new KeyValuePair<string, object?>(_formatter.ValueNames[3], _value3);
                        case 4:
                            return new KeyValuePair<string, object?>("{OriginalFormat}", _formatter.OriginalFormat);
                        default:
#pragma warning disable CA2201 // Do not raise reserved exception types
#pragma warning disable S112 // General or reserved exceptions should never be thrown
                            throw new IndexOutOfRangeException(nameof(index));
#pragma warning restore S112 // General or reserved exceptions should never be thrown
#pragma warning restore CA2201 // Do not raise reserved exception types
                    }
                }
            }

#pragma warning disable SA1201 // Elements should appear in the correct order
            public LogValues(LogValuesFormatter formatter, T0 value0, T1 value1, T2 value2, T3 value3)
#pragma warning restore SA1201 // Elements should appear in the correct order
            {
                _formatter = formatter;
                _value0 = value0;
                _value1 = value1;
                _value2 = value2;
                _value3 = value3;
            }

            private object?[] ToArray() => new object?[] { _value0, _value1, _value2, _value3 };

#pragma warning disable SA1202 // Elements should be ordered by access
            public override string ToString() => _formatter.FormatWithOverwrite(ToArray());
#pragma warning restore SA1202 // Elements should be ordered by access

            public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
            {
                for (int i = 0; i < Count; ++i)
                {
                    yield return this[i];
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private readonly struct LogValues<T0, T1, T2, T3, T4> : IReadOnlyList<KeyValuePair<string, object?>>
        {
            public static readonly Func<LogValues<T0, T1, T2, T3, T4>, Exception?, string> Callback = (state, exception) => state.ToString();

            private readonly LogValuesFormatter _formatter;
            private readonly T0 _value0;
            private readonly T1 _value1;
            private readonly T2 _value2;
            private readonly T3 _value3;
            private readonly T4 _value4;

            public int Count => 6;

            public KeyValuePair<string, object?> this[int index]
            {
                get
                {
                    switch (index)
                    {
                        case 0:
                            return new KeyValuePair<string, object?>(_formatter.ValueNames[0], _value0);
                        case 1:
                            return new KeyValuePair<string, object?>(_formatter.ValueNames[1], _value1);
                        case 2:
                            return new KeyValuePair<string, object?>(_formatter.ValueNames[2], _value2);
                        case 3:
                            return new KeyValuePair<string, object?>(_formatter.ValueNames[3], _value3);
                        case 4:
                            return new KeyValuePair<string, object?>(_formatter.ValueNames[4], _value4);
                        case 5:
                            return new KeyValuePair<string, object?>("{OriginalFormat}", _formatter.OriginalFormat);
                        default:
#pragma warning disable CA2201 // Do not raise reserved exception types
#pragma warning disable S112
                            throw new IndexOutOfRangeException(nameof(index));
#pragma warning restore S112
#pragma warning restore CA2201 // Do not raise reserved exception types
                    }
                }
            }

#pragma warning disable SA1201 // Elements should appear in the correct order
            public LogValues(LogValuesFormatter formatter, T0 value0, T1 value1, T2 value2, T3 value3, T4 value4)
#pragma warning restore SA1201 // Elements should appear in the correct order
            {
                _formatter = formatter;
                _value0 = value0;
                _value1 = value1;
                _value2 = value2;
                _value3 = value3;
                _value4 = value4;
            }

            private object?[] ToArray() => new object?[] { _value0, _value1, _value2, _value3, _value4 };

#pragma warning disable SA1202 // Elements should be ordered by access
            public override string ToString() => _formatter.FormatWithOverwrite(ToArray());
#pragma warning restore SA1202 // Elements should be ordered by access

            public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
            {
                for (int i = 0; i < Count; ++i)
                {
                    yield return this[i];
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private readonly struct LogValues<T0, T1, T2, T3, T4, T5> : IReadOnlyList<KeyValuePair<string, object?>>
        {
            public static readonly Func<LogValues<T0, T1, T2, T3, T4, T5>, Exception?, string> Callback = (state, exception) => state.ToString();

            private readonly LogValuesFormatter _formatter;
            private readonly T0 _value0;
            private readonly T1 _value1;
            private readonly T2 _value2;
            private readonly T3 _value3;
            private readonly T4 _value4;
            private readonly T5 _value5;

            public int Count => 7;

            public KeyValuePair<string, object?> this[int index]
            {
                get
                {
                    switch (index)
                    {
                        case 0:
                            return new KeyValuePair<string, object?>(_formatter.ValueNames[0], _value0);
                        case 1:
                            return new KeyValuePair<string, object?>(_formatter.ValueNames[1], _value1);
                        case 2:
                            return new KeyValuePair<string, object?>(_formatter.ValueNames[2], _value2);
                        case 3:
                            return new KeyValuePair<string, object?>(_formatter.ValueNames[3], _value3);
                        case 4:
                            return new KeyValuePair<string, object?>(_formatter.ValueNames[4], _value4);
                        case 5:
                            return new KeyValuePair<string, object?>(_formatter.ValueNames[5], _value5);
                        case 6:
                            return new KeyValuePair<string, object?>("{OriginalFormat}", _formatter.OriginalFormat);
                        default:
#pragma warning disable CA2201 // Do not raise reserved exception types
#pragma warning disable S112 // General or reserved exceptions should never be thrown
                            throw new IndexOutOfRangeException(nameof(index));
#pragma warning restore S112 // General or reserved exceptions should never be thrown
#pragma warning restore CA2201 // Do not raise reserved exception types
                    }
                }
            }

#pragma warning disable SA1201 // Elements should appear in the correct order
            public LogValues(LogValuesFormatter formatter, T0 value0, T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
#pragma warning restore SA1201 // Elements should appear in the correct order
            {
                _formatter = formatter;
                _value0 = value0;
                _value1 = value1;
                _value2 = value2;
                _value3 = value3;
                _value4 = value4;
                _value5 = value5;
            }

            private object?[] ToArray() => new object?[] { _value0, _value1, _value2, _value3, _value4, _value5 };

#pragma warning disable SA1202 // Elements should be ordered by access
            public override string ToString() => _formatter.FormatWithOverwrite(ToArray());
#pragma warning restore SA1202 // Elements should be ordered by access

            public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
            {
                for (int i = 0; i < Count; ++i)
                {
                    yield return this[i];
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
