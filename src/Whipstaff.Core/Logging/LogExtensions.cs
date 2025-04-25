// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace Whipstaff.Core.Logging
{
    /// <summary>
    /// Extensions for the Microsoft Logging Extensions.
    /// </summary>
    public static class LogExtensions
    {
        private static readonly Action<ILogger, string, Exception?> _traceMethodEntryAction =
            LoggerMessage.Define<string>(LogLevel.Trace, WhipstaffEventIdFactory.MethodEntryEventId(), "Method Entry: {MethodName}");

        private static readonly Action<ILogger, string, Exception?> _traceMethodExitAction =
            LoggerMessage.Define<string>(LogLevel.Trace, WhipstaffEventIdFactory.MethodEntryEventId(), "Method Exit: {MethodName}");

        private static readonly Action<ILogger, string, Exception?> _traceMethodExceptionAction =
            LoggerMessage.Define<string>(LogLevel.Trace, WhipstaffEventIdFactory.MethodEntryEventId(), "Method Exception: {MethodName}");

        /// <summary>
        /// Traces the method entry.
        /// </summary>
        /// <param name="logger">Logging instance.</param>
        /// <param name="callerMemberName">Name of the method.</param>
        public static void TraceMethodEntry(
            this ILogger logger,
            [CallerMemberName] string? callerMemberName = null)
        {
            if (string.IsNullOrWhiteSpace(callerMemberName))
            {
                return;
            }

            _traceMethodEntryAction(logger, callerMemberName, null);
        }

        /// <summary>
        /// Traces the method exit.
        /// </summary>
        /// <param name="logger">Logging instance.</param>
        /// <param name="exception">Exception, if any.</param>
        /// <param name="callerMemberName">Name of the method.</param>
        public static void TraceMethodExit(
            this ILogger logger,
            Exception? exception = null,
            [CallerMemberName] string? callerMemberName = null)
        {
            if (string.IsNullOrWhiteSpace(callerMemberName))
            {
                return;
            }

            _traceMethodExitAction(logger, callerMemberName, exception);
        }

        /// <summary>
        /// Write a trace event if the log level is enabled.
        /// </summary>
        /// <param name="logger">Logging instance.</param>
        /// <param name="messageFunc">Message producing func to evaluate if log level enabled.</param>
        public static void TraceIfEnabled(
            this ILogger logger,
            Func<string> messageFunc)
        {
#pragma warning disable CA1062 // Validate arguments of public methods
            logger.LogIfEnabled(LogLevel.Trace, messageFunc);
#pragma warning restore CA1062 // Validate arguments of public methods
        }

        /// <summary>
        /// Write a trace event and exception if the log level is enabled.
        /// </summary>
        /// <param name="logger">Logging instance.</param>
        /// <param name="exception">Exception that occurred.</param>
        /// <param name="messageFunc">Message producing func to evaluate if log level enabled.</param>
        public static void TraceIfEnabled(
            this ILogger logger,
            Exception exception,
            Func<string> messageFunc)
        {
#pragma warning disable CA1062 // Validate arguments of public methods
            logger.LogIfEnabled(LogLevel.Trace, exception, messageFunc);
#pragma warning restore CA1062 // Validate arguments of public methods
        }

        /// <summary>
        /// Traces an exception in a method.
        /// </summary>
        /// <param name="logger">Logging instance.</param>
        /// <param name="exception">Exception that occurred.</param>
        /// <param name="callerMemberName">Name of the method.</param>
        public static void TraceMethodException(
            this ILogger logger,
            Exception exception,
            [CallerMemberName] string? callerMemberName = null)
        {
            if (string.IsNullOrWhiteSpace(callerMemberName))
            {
                return;
            }

            _traceMethodExceptionAction(logger, callerMemberName, exception);
        }

        /// <summary>
        /// Write a warning event if the log level is enabled.
        /// </summary>
        /// <param name="logger">Logging instance.</param>
        /// <param name="messageFunc">Message producing func to evaluate if log level enabled.</param>
        public static void WarningIfEnabled(
            this ILogger logger,
            Func<string> messageFunc)
        {
#pragma warning disable CA1062 // Validate arguments of public methods
            logger.LogIfEnabled(LogLevel.Warning, messageFunc);
#pragma warning restore CA1062 // Validate arguments of public methods
        }

        /// <summary>
        /// Write a warn event and exception if the log level is enabled.
        /// </summary>
        /// <param name="logger">Logging instance.</param>
        /// <param name="exception">Exception that occurred.</param>
        /// <param name="messageFunc">Message producing func to evaluate if log level enabled.</param>
        public static void WarningIfEnabled(
            this ILogger logger,
            Exception exception,
            Func<string> messageFunc)
        {
#pragma warning disable CA1062 // Validate arguments of public methods
            logger.LogIfEnabled(LogLevel.Warning, exception, messageFunc);
#pragma warning restore CA1062 // Validate arguments of public methods
        }

        /// <summary>
        /// Write a error event if the log level is enabled.
        /// </summary>
        /// <param name="logger">Logging instance.</param>
        /// <param name="messageFunc">Message producing func to evaluate if log level enabled.</param>
        public static void ErrorIfEnabled(
            this ILogger logger,
            Func<string> messageFunc)
        {
#pragma warning disable CA1062 // Validate arguments of public methods
            logger.LogIfEnabled(LogLevel.Error, messageFunc);
#pragma warning restore CA1062 // Validate arguments of public methods
        }

        /// <summary>
        /// Write a error event if the log level is enabled.
        /// </summary>
        /// <param name="logger">Logging instance.</param>
        /// <param name="exception">Exception that occurred.</param>
        /// <param name="messageFunc">Message producing func to evaluate if log level enabled.</param>
        public static void ErrorIfEnabled(
            this ILogger logger,
            Exception exception,
            Func<string> messageFunc)
        {
#pragma warning disable CA1062 // Validate arguments of public methods
            logger.LogIfEnabled(LogLevel.Error, exception, messageFunc);
#pragma warning restore CA1062 // Validate arguments of public methods
        }

        /// <summary>
        /// Write a information event if the log level is enabled.
        /// </summary>
        /// <param name="logger">Logging instance.</param>
        /// <param name="messageFunc">Message producing func to evaluate if log level enabled.</param>
        public static void InformationIfEnabled(
            this ILogger logger,
            Func<string> messageFunc)
        {
#pragma warning disable CA1062 // Validate arguments of public methods
            logger.LogIfEnabled(LogLevel.Information, messageFunc);
#pragma warning restore CA1062 // Validate arguments of public methods
        }

        /// <summary>
        /// Write a information event if the log level is enabled.
        /// </summary>
        /// <param name="logger">Logging instance.</param>
        /// <param name="exception">Exception that occurred.</param>
        /// <param name="messageFunc">Message producing func to evaluate if log level enabled.</param>
        public static void InformationIfEnabled(
            this ILogger logger,
            Exception exception,
            Func<string> messageFunc)
        {
#pragma warning disable CA1062 // Validate arguments of public methods
            logger.LogIfEnabled(LogLevel.Information, exception, messageFunc);
#pragma warning restore CA1062 // Validate arguments of public methods
        }

        /// <summary>
        /// Write a debug event if the log level is enabled.
        /// </summary>
        /// <param name="logger">Logging instance.</param>
        /// <param name="messageFunc">Message producing func to evaluate if log level enabled.</param>
        public static void DebugIfEnabled(
            this ILogger logger,
            Func<string> messageFunc)
        {
#pragma warning disable CA1062 // Validate arguments of public methods
            logger.LogIfEnabled(LogLevel.Debug, messageFunc);
#pragma warning restore CA1062 // Validate arguments of public methods
        }

        /// <summary>
        /// Write a debug event if the log level is enabled.
        /// </summary>
        /// <param name="logger">Logging instance.</param>
        /// <param name="exception">Exception that occurred.</param>
        /// <param name="messageFunc">Message producing func to evaluate if log level enabled.</param>
        public static void DebugIfEnabled(
            this ILogger logger,
            Exception exception,
            Func<string> messageFunc)
        {
#pragma warning disable CA1062 // Validate arguments of public methods
            logger.LogIfEnabled(LogLevel.Debug, exception, messageFunc);
#pragma warning restore CA1062 // Validate arguments of public methods
        }

        /// <summary>
        /// Write a critical event if the log level is enabled.
        /// </summary>
        /// <param name="logger">Logging instance.</param>
        /// <param name="messageFunc">Message producing func to evaluate if log level enabled.</param>
        public static void CriticalIfEnabled(
            this ILogger logger,
            Func<string> messageFunc)
        {
#pragma warning disable CA1062 // Validate arguments of public methods
            logger.LogIfEnabled(LogLevel.Critical, messageFunc);
#pragma warning restore CA1062 // Validate arguments of public methods
        }

        /// <summary>
        /// Write a critical event if the log level is enabled.
        /// </summary>
        /// <param name="logger">Logging instance.</param>
        /// <param name="exception">Exception that occurred.</param>
        /// <param name="messageFunc">Message producing func to evaluate if log level enabled.</param>
        public static void CriticalIfEnabled(
            this ILogger logger,
            Exception exception,
            Func<string> messageFunc)
        {
#pragma warning disable CA1062 // Validate arguments of public methods
            logger.LogIfEnabled(LogLevel.Critical, exception, messageFunc);
#pragma warning restore CA1062 // Validate arguments of public methods
        }

        private static void LogIfEnabled(
            this ILogger logger,
            LogLevel logLevel,
            Exception exception,
            Func<string> messageFunc)
        {
            if (!logger.IsEnabled(logLevel))
            {
                return;
            }

            var message = messageFunc();
#pragma warning disable CA1848 // Use the LoggerMessage delegates
#pragma warning disable CA2254 // Template should be a static expression
            logger.Log(logLevel, exception, message);
#pragma warning restore CA2254 // Template should be a static expression
#pragma warning restore CA1848 // Use the LoggerMessage delegates
        }

        private static void LogIfEnabled(
            this ILogger logger,
            LogLevel logLevel,
            Func<string> messageFunc)
        {
            if (!logger.IsEnabled(logLevel))
            {
                return;
            }

            var message = messageFunc();
#pragma warning disable CA1848 // Use the LoggerMessage delegates
#pragma warning disable CA2254 // Template should be a static expression
            logger.Log(logLevel, message);
#pragma warning restore CA2254 // Template should be a static expression
#pragma warning restore CA1848 // Use the LoggerMessage delegates
        }
    }
}
