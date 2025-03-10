using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Sdk;

namespace Whipstaff.Testing.Logging
{
    public sealed class TestLogger : ILoggerFactory
    {
        private readonly Dictionary<string, LogLevel> _logLevels = new();
        private readonly Queue<LogEntry> _logEntries = new();
        private int _logEntriesWritten;

        public TestLogger(Action<TestLoggerOptions>? configure = null)
        {
            Options = new TestLoggerOptions();
            configure?.Invoke(Options);
        }

        public TestLogger(ITestOutputHelper output, Action<TestLoggerOptions>? configure = null)
        {
            Options = new TestLoggerOptions
            {
                WriteLogEntryFunc = logEntry =>
                {
                    output.WriteLine(logEntry.ToString(false));
                }
            };

            configure?.Invoke(Options);
        }

        public TestLogger(TestLoggerOptions options)
        {
            Options = options ?? new TestLoggerOptions();
        }

        public TestLoggerOptions Options { get; }

        public LogLevel DefaultMinimumLevel
        {
            get => Options.DefaultMinimumLevel;
            set => Options.DefaultMinimumLevel = value;
        }

        public int MaxLogEntriesToStore
        {
            get => Options.MaxLogEntriesToStore;
            set => Options.MaxLogEntriesToStore = value;
        }

        public int MaxLogEntriesToWrite
        {
            get => Options.MaxLogEntriesToWrite;
            set => Options.MaxLogEntriesToWrite = value;
        }

        public IReadOnlyList<LogEntry> LogEntries => _logEntries.ToArray();


        public void Reset()
        {
            lock (_logEntries)
            {
                _logEntries.Clear();
                _logLevels.Clear();
                Interlocked.Exchange(ref _logEntriesWritten, 0);
            }
        }

        internal void AddLogEntry(LogEntry logEntry)
        {
            lock (_logEntries)
            {
                _logEntries.Enqueue(logEntry);

                if (_logEntries.Count > Options.MaxLogEntriesToStore)
                    _logEntries.Dequeue();
            }

            if (Options.WriteLogEntryFunc == null || _logEntriesWritten >= Options.MaxLogEntriesToWrite)
                return;

#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                Options.WriteLogEntry(logEntry);
                Interlocked.Increment(ref _logEntriesWritten);
            }
            catch (Exception)
            {
                // ignored
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new TestLoggerLogger(categoryName, this);
        }

        public void AddProvider(ILoggerProvider loggerProvider) { }

        public bool IsEnabled(string category, LogLevel logLevel)
        {
            if (_logLevels.TryGetValue(category, out var categoryLevel))
                return logLevel >= categoryLevel;

            return logLevel >= Options.DefaultMinimumLevel;
        }

        public void SetLogLevel(string category, LogLevel minLogLevel)
        {
            _logLevels[category] = minLogLevel;
        }

        public void SetLogLevel<T>(LogLevel minLogLevel)
        {
            SetLogLevel(typeof(T).Name, minLogLevel);
        }

        public void Dispose() { }
    }
}
