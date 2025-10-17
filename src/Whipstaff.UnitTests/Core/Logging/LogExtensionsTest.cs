// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;
using Whipstaff.Core.Logging;
using Whipstaff.Testing.Logging;
using Xunit;

namespace Whipstaff.UnitTests.Core.Logging
{
    /// <summary>
    /// Unit Tests for <see cref="LogExtensions"/>.
    /// </summary>
    public static class LogExtensionsTest
    {
        /// <summary>
        /// Unit Tests for <see cref="LogExtensions.TraceMethodEntry"/>.
        /// </summary>
        public sealed class TraceMethodEntryMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TraceMethodEntryMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public TraceMethodEntryMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure the log event occurs.
            /// </summary>
            [Fact]
            public void LogsMessage()
            {
                LoggerFactory.DefaultMinimumLevel = LogLevel.Trace;
                Logger.TraceMethodEntry();
                _ = Assert.Single(LoggerFactory.LogEntries);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LogExtensions.TraceMethodExit"/>.
        /// </summary>
        public sealed class TraceMethodExitMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TraceMethodExitMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public TraceMethodExitMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure the log event occurs.
            /// </summary>
            [Fact]
            public void LogsMessage()
            {
                LoggerFactory.DefaultMinimumLevel = LogLevel.Trace;
                Logger.TraceMethodExit();
                _ = Assert.Single(LoggerFactory.LogEntries);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LogExtensions.TraceIfEnabled(ILogger, Func{string})"/>.
        /// </summary>
        public sealed class TraceIfEnabledMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TraceIfEnabledMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public TraceIfEnabledMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure the log event occurs.
            /// </summary>
            [Fact]
            public void LogsMessage()
            {
                LoggerFactory.DefaultMinimumLevel = LogLevel.Trace;
                Logger.TraceIfEnabled(() => "TEST");
                _ = Assert.Single(LoggerFactory.LogEntries);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LogExtensions.TraceIfEnabled(ILogger, Exception, Func{string})"/>.
        /// </summary>
        public sealed class TraceIfEnabledWithExceptionMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TraceIfEnabledWithExceptionMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public TraceIfEnabledWithExceptionMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure the log event occurs.
            /// </summary>
            [Fact]
            public void LogsMessage()
            {
                LoggerFactory.DefaultMinimumLevel = LogLevel.Trace;
                var exception = new InvalidOperationException("Some test exception");
                Logger.TraceIfEnabled(exception, () => "TEST");
                _ = Assert.Single(LoggerFactory.LogEntries);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LogExtensions.TraceIfEnabled(ILogger, Exception, Func{string})"/>.
        /// </summary>
        public sealed class TraceMethodExceptionEnabledMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TraceMethodExceptionEnabledMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public TraceMethodExceptionEnabledMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure the log event occurs.
            /// </summary>
            [Fact]
            public void LogsMessage()
            {
                LoggerFactory.DefaultMinimumLevel = LogLevel.Trace;
                var exception = new InvalidOperationException("Some test exception");
                Logger.TraceMethodException(exception);
                _ = Assert.Single(LoggerFactory.LogEntries);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LogExtensions.WarningIfEnabled(ILogger, Func{string})"/>.
        /// </summary>
        public sealed class WarningIfEnabledMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="WarningIfEnabledMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public WarningIfEnabledMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure the log event occurs.
            /// </summary>
            [Fact]
            public void LogsMessage()
            {
                Logger.WarningIfEnabled(() => "TEST");
                _ = Assert.Single(LoggerFactory.LogEntries);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LogExtensions.WarningIfEnabled(ILogger, Exception, Func{string})"/>.
        /// </summary>
        public sealed class WarningIfEnabledWithExceptionMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="WarningIfEnabledWithExceptionMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public WarningIfEnabledWithExceptionMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure the log event occurs.
            /// </summary>
            [Fact]
            public void LogsMessage()
            {
                var exception = new InvalidOperationException("Some test exception");
                Logger.WarningIfEnabled(exception, () => "TEST");
                _ = Assert.Single(LoggerFactory.LogEntries);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LogExtensions.ErrorIfEnabled(ILogger, Func{string})"/>.
        /// </summary>
        public sealed class ErrorIfEnabledMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ErrorIfEnabledMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public ErrorIfEnabledMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure the log event occurs.
            /// </summary>
            [Fact]
            public void LogsMessage()
            {
                Logger.ErrorIfEnabled(() => "TEST");
                _ = Assert.Single(LoggerFactory.LogEntries);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LogExtensions.ErrorIfEnabled(ILogger, Exception, Func{string})"/>.
        /// </summary>
        public sealed class ErrorIfEnabledWithExceptionMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ErrorIfEnabledWithExceptionMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public ErrorIfEnabledWithExceptionMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure the log event occurs.
            /// </summary>
            [Fact]
            public void LogsMessage()
            {
                var exception = new InvalidOperationException("Some test exception");
                Logger.ErrorIfEnabled(exception, () => "TEST");
                _ = Assert.Single(LoggerFactory.LogEntries);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LogExtensions.InformationIfEnabled(ILogger, Func{string})"/>.
        /// </summary>
        public sealed class InformationIfEnabledMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="InformationIfEnabledMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public InformationIfEnabledMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure the log event occurs.
            /// </summary>
            [Fact]
            public void LogsMessage()
            {
                Logger.InformationIfEnabled(() => "TEST");
                _ = Assert.Single(LoggerFactory.LogEntries);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LogExtensions.ErrorIfEnabled(ILogger, Exception, Func{string})"/>.
        /// </summary>
        public sealed class InformationIfEnabledWithExceptionMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="InformationIfEnabledWithExceptionMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public InformationIfEnabledWithExceptionMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure the log event occurs.
            /// </summary>
            [Fact]
            public void LogsMessage()
            {
                var exception = new InvalidOperationException("Some test exception");
                Logger.InformationIfEnabled(exception, () => "TEST");
                _ = Assert.Single(LoggerFactory.LogEntries);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LogExtensions.DebugIfEnabled(ILogger, Func{string})"/>.
        /// </summary>
        public sealed class DebugIfEnabledMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DebugIfEnabledMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public DebugIfEnabledMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure the log event occurs.
            /// </summary>
            [Fact]
            public void LogsMessage()
            {
                LoggerFactory.DefaultMinimumLevel = LogLevel.Debug;
                Logger.DebugIfEnabled(() => "TEST");
                _ = Assert.Single(LoggerFactory.LogEntries);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LogExtensions.ErrorIfEnabled(ILogger, Exception, Func{string})"/>.
        /// </summary>
        public sealed class DebugIfEnabledWithExceptionMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DebugIfEnabledWithExceptionMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public DebugIfEnabledWithExceptionMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure the log event occurs.
            /// </summary>
            [Fact]
            public void LogsMessage()
            {
                LoggerFactory.DefaultMinimumLevel = LogLevel.Debug;
                var exception = new InvalidOperationException("Some test exception");
                Logger.DebugIfEnabled(exception, () => "TEST");
                _ = Assert.Single(LoggerFactory.LogEntries);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LogExtensions.CriticalIfEnabled(ILogger, Func{string})"/>.
        /// </summary>
        public sealed class CriticalIfEnabledMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CriticalIfEnabledMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public CriticalIfEnabledMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure the log event occurs.
            /// </summary>
            [Fact]
            public void LogsMessage()
            {
                Logger.CriticalIfEnabled(() => "TEST");
                _ = Assert.Single(LoggerFactory.LogEntries);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LogExtensions.CriticalIfEnabled(ILogger, Exception, Func{string})"/>.
        /// </summary>
        public sealed class CriticalIfEnabledWithExceptionMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CriticalIfEnabledWithExceptionMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public CriticalIfEnabledWithExceptionMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure the log event occurs.
            /// </summary>
            [Fact]
            public void LogsMessage()
            {
                var exception = new InvalidOperationException("Some test exception");
                Logger.CriticalIfEnabled(exception, () => "TEST");
                _ = Assert.Single(LoggerFactory.LogEntries);
            }
        }
    }
}
