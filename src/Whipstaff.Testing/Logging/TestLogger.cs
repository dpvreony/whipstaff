// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Whipstaff.Testing.Logging
{
    /// <summary>
    /// XUnit Test Logger Factory.
    /// </summary>
    public sealed class TestLogger : ILoggerFactory
    {
        private readonly Dictionary<string, LogLevel> _logLevels = new();
        private readonly Queue<LogEntry> _logEntries = new();
        private int _logEntriesWritten;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestLogger"/> class.
        /// </summary>
        /// <param name="configure">Action to invoke when configuring logging options.</param>
        public TestLogger(Action<TestLoggerOptions>? configure = null)
        {
            Options = new TestLoggerOptions();
#pragma warning disable GR0012 // Constructors should minimise work and not execute methods
            configure?.Invoke(Options);
#pragma warning restore GR0012 // Constructors should minimise work and not execute methods
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestLogger"/> class.
        /// </summary>
        /// <param name="output">XUnit output helper.</param>
        /// <param name="configure">Action to run when configuring logging options.</param>
        public TestLogger(ITestOutputHelper output, Action<TestLoggerOptions>? configure = null)
        {
            Options = new TestLoggerOptions
            {
                WriteLogEntryFunc = logEntry =>
                {
                    output.WriteLine(logEntry.ToString(false));
                }
            };

#pragma warning disable GR0012 // Constructors should minimise work and not execute methods
            configure?.Invoke(Options);
#pragma warning restore GR0012 // Constructors should minimise work and not execute methods
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestLogger"/> class.
        /// </summary>
        /// <param name="options">Settings for the logging configuration.</param>
        public TestLogger(TestLoggerOptions options)
        {
            Options = options ?? new TestLoggerOptions();
        }

        /// <summary>
        /// Gets the options.
        /// </summary>
        public TestLoggerOptions Options { get; }

        /// <summary>
        /// Gets or sets the default minimum log level.
        /// </summary>
        public LogLevel DefaultMinimumLevel
        {
            get => Options.DefaultMinimumLevel;
            set => Options.DefaultMinimumLevel = value;
        }

        /// <summary>
        /// Gets or sets the maximum log entries to store.
        /// </summary>
        public int MaxLogEntriesToStore
        {
            get => Options.MaxLogEntriesToStore;
            set => Options.MaxLogEntriesToStore = value;
        }

        /// <summary>
        /// Gets or sets the maximum log entries to write.
        /// </summary>
        public int MaxLogEntriesToWrite
        {
            get => Options.MaxLogEntriesToWrite;
            set => Options.MaxLogEntriesToWrite = value;
        }

        /// <summary>
        /// Gets the log entries.
        /// </summary>
        public IReadOnlyList<LogEntry> LogEntries
        {
            get
            {
                lock (_logEntries)
                {
                    return _logEntries.ToArray();
                }
            }
        }

        /// <summary>
        /// Resets the log entries.
        /// </summary>
        public void Reset()
        {
            lock (_logEntries)
            {
                _logEntries.Clear();
                _logLevels.Clear();
                _ = Interlocked.Exchange(ref _logEntriesWritten, 0);
            }
        }

        /// <inheritdoc/>
        public ILogger CreateLogger(string categoryName)
        {
            return new TestLoggerLogger(categoryName, this);
        }

        /// <inheritdoc/>
        public void AddProvider(ILoggerProvider provider)
        {
        }

        /// <summary>
        /// Checks if logging is enabled for the category at a specific log level.
        /// </summary>
        /// <param name="category">The category to check.</param>
        /// <param name="logLevel">The log level to check.</param>
        /// <returns>Whether the log level is enabled.</returns>
        public bool IsEnabled(string category, LogLevel logLevel)
        {
            if (_logLevels.TryGetValue(category, out var categoryLevel))
            {
                return logLevel >= categoryLevel;
            }

            return logLevel >= Options.DefaultMinimumLevel;
        }

        /// <summary>
        /// Sets the log level for a category.
        /// </summary>
        /// <param name="category">Category to set.</param>
        /// <param name="minLogLevel">LoggerFactory level to set.</param>
        public void SetLogLevel(string category, LogLevel minLogLevel)
        {
            _logLevels[category] = minLogLevel;
        }

        /// <summary>
        /// Sets the log level for a category based on the type.
        /// </summary>
        /// <typeparam name="T">The type to base the category off.</typeparam>
        /// <param name="minLogLevel">LoggerFactory level to set.</param>
        public void SetLogLevel<T>(LogLevel minLogLevel)
        {
            SetLogLevel(nameof(T), minLogLevel);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
        }

        internal void AddLogEntry(LogEntry logEntry)
        {
            lock (_logEntries)
            {
                _logEntries.Enqueue(logEntry);

                if (_logEntries.Count > Options.MaxLogEntriesToStore)
                {
                    _ = _logEntries.Dequeue();
                }
            }

            if (Options.WriteLogEntryFunc == null || _logEntriesWritten >= Options.MaxLogEntriesToWrite)
            {
                return;
            }

#pragma warning disable CA1031 // Do not catch general exception types
#pragma warning disable RCS1075 // Avoid empty catch clause that catches System.Exception
            try
            {
                Options.WriteLogEntry(logEntry);
                _ = Interlocked.Increment(ref _logEntriesWritten);
            }
            catch (Exception)
            {
                // ignored
            }
#pragma warning restore RCS1075 // Avoid empty catch clause that catches System.Exception
#pragma warning restore CA1031 // Do not catch general exception types
        }
    }
}
