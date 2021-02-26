// Copyright (c) 2020 DHGMS Solutions and Contributors. All rights reserved.
// DHGMS Solutions and Contributors licenses this file to you under the MIT license.
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
        /// <summary>
        /// Write a trace event if the log level is enabled.
        /// </summary>
        /// <param name="logger">Logging instance.</param>
        /// <param name="messageFunc">Message producing func to evaluate if log level enabled.</param>
        public static void TraceIfEnabled(
            this ILogger logger,
            Func<string> messageFunc)
        {
            logger.LogIfEnabled(LogLevel.Trace, messageFunc);
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
            logger.LogIfEnabled(LogLevel.Trace, exception, messageFunc);
        }

        /// <summary>
        /// Traces the method entry.
        /// </summary>
        /// <param name="logger">Logging instance.</param>
        /// <param name="callerMemberName">Name of the method.</param>
        public static void TraceMethodEntry(
            this ILogger logger,
            [CallerMemberName] string callerMemberName = null)
        {
            logger.TraceIfEnabled(() => $"Method Entry: {callerMemberName}");
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
            [CallerMemberName] string callerMemberName = null)
        {
            logger.TraceIfEnabled(exception, () => $"Method Entry: {callerMemberName}");
        }

        /// <summary>
        /// Traces the method exit.
        /// </summary>
        /// <param name="logger">Logging instance.</param>
        /// <param name="callerMemberName">Name of the method.</param>
        public static void TraceMethodExit(
            this ILogger logger,
            [CallerMemberName] string callerMemberName = null)
        {
            logger.TraceIfEnabled(() => $"Method Exit: {callerMemberName}");
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
            logger.LogIfEnabled(LogLevel.Warning, messageFunc);
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
            logger.LogIfEnabled(LogLevel.Warning, exception, messageFunc);
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
            logger.LogIfEnabled(LogLevel.Error, messageFunc);
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
            logger.LogIfEnabled(LogLevel.Information, messageFunc);
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
            logger.LogIfEnabled(LogLevel.Debug, messageFunc);
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
            logger.LogIfEnabled(LogLevel.Critical, messageFunc);
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
            logger.Log(logLevel, exception, message);
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
            logger.Log(logLevel, message);
        }
    }
}
