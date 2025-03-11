// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Whipstaff.Testing.Logging
{
    /// <summary>
    /// Options for configuring a test logger.
    /// </summary>
    public class TestLoggerOptions
    {
        /// <summary>
        /// Gets or sets the default minimum log level.
        /// </summary>
        public LogLevel DefaultMinimumLevel { get; set; } = LogLevel.Information;

        /// <summary>
        /// Gets or sets the maximum number of log entries to store.
        /// </summary>
        public int MaxLogEntriesToStore { get; set; } = 100;

        /// <summary>
        /// Gets or sets the maximum number of log entries to write.
        /// </summary>
        public int MaxLogEntriesToWrite { get; set; } = 1000;

        /// <summary>
        /// Gets or sets a value indicating whether to include scopes in the log entries.
        /// </summary>
        public bool IncludeScopes { get; set; } = true;

        /// <summary>
        /// Gets or sets the time provider.
        /// </summary>
        public TimeProvider TimeProvider { get; set; } = TimeProvider.System;

        /// <summary>
        /// Gets or sets the function to write a log entry.
        /// </summary>
        public Action<LogEntry>? WriteLogEntryFunc { get; set; }

        /// <summary>
        /// Gets or sets the function to get the current date and time.
        /// </summary>
        public Func<DateTimeOffset>? NowFunc { get; set; }

        /// <summary>
        /// Sets the output helper to use for writing log entries.
        /// </summary>
        /// <param name="getOutputHelper">Output helper to use.</param>
        /// <param name="formatLogEntry">LoggerFactory entry to format and write.</param>
        public void UseOutputHelper(Func<ITestOutputHelper> getOutputHelper, Func<LogEntry, string>? formatLogEntry = null)
        {
            formatLogEntry ??= logEntry => logEntry.ToString(false);
            WriteLogEntryFunc = logEntry =>
            {
                getOutputHelper?.Invoke()?.WriteLine(formatLogEntry(logEntry));
            };
        }

        internal void WriteLogEntry(LogEntry logEntry) => WriteLogEntryFunc?.Invoke(logEntry);

        internal DateTimeOffset GetNow() => NowFunc?.Invoke() ?? TimeProvider.GetUtcNow();
    }
}
