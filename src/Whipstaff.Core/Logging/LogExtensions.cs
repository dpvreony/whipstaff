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
            LoggerMessage.Define<string>(LogLevel.Trace, EventIdFactory.MethodEntryEventId(), "Method Entry: {MethodName}");

        private static readonly Action<ILogger, string, Exception?> _traceMethodExitAction =
            LoggerMessage.Define<string>(LogLevel.Trace, EventIdFactory.MethodEntryEventId(), "Method Exit: {MethodName}");

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
    }
}
