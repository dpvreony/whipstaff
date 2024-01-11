// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;
using Whipstaff.Core.Logging;
using Xunit;
using Xunit.Abstractions;

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
        public sealed class TraceMethodEntryMethod : Foundatio.Xunit.TestWithLoggingBase
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
                _logger.TraceMethodEntry();
                _ = Assert.Single(Log.LogEntries);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LogExtensions.TraceMethodExit"/>.
        /// </summary>
        public sealed class TraceMethodExitMethod : Foundatio.Xunit.TestWithLoggingBase
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
                _logger.TraceMethodExit();
                _ = Assert.Single(Log.LogEntries);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LogExtensions.TraceIfEnabled(ILogger, Func{string})"/>.
        /// </summary>
        public sealed class TraceIfEnabledMethod : Foundatio.Xunit.TestWithLoggingBase
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
                _logger.TraceIfEnabled(() => "TEST");
                _ = Assert.Single(Log.LogEntries);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LogExtensions.TraceIfEnabled(ILogger, Exception, Func{string})"/>.
        /// </summary>
        public sealed class TraceIfEnabledWithExceptionMethod : Foundatio.Xunit.TestWithLoggingBase
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
                var exception = new InvalidOperationException("Some test exception");
                _logger.TraceIfEnabled(exception, () => "TEST");
                _ = Assert.Single(Log.LogEntries);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LogExtensions.TraceIfEnabled(ILogger, Exception, Func{string})"/>.
        /// </summary>
        public sealed class TraceMethodExceptionEnabledMethod : Foundatio.Xunit.TestWithLoggingBase
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
                var exception = new InvalidOperationException("Some test exception");
                _logger.TraceMethodException(exception);
                _ = Assert.Single(Log.LogEntries);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LogExtensions.WarningIfEnabled(ILogger, Func{string})"/>.
        /// </summary>
        public sealed class WarningIfEnabledMethod : Foundatio.Xunit.TestWithLoggingBase
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
                _logger.WarningIfEnabled(() => "TEST");
                _ = Assert.Single(Log.LogEntries);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LogExtensions.WarningIfEnabled(ILogger, Exception, Func{string})"/>.
        /// </summary>
        public sealed class WarningIfEnabledWithExceptionMethod : Foundatio.Xunit.TestWithLoggingBase
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
                _logger.WarningIfEnabled(exception, () => "TEST");
                _ = Assert.Single(Log.LogEntries);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LogExtensions.ErrorIfEnabled(ILogger, Func{string})"/>.
        /// </summary>
        public sealed class ErrorIfEnabledMethod : Foundatio.Xunit.TestWithLoggingBase
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
                _logger.ErrorIfEnabled(() => "TEST");
                _ = Assert.Single(Log.LogEntries);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LogExtensions.ErrorIfEnabled(ILogger, Exception, Func{string})"/>.
        /// </summary>
        public sealed class ErrorIfEnabledWithExceptionMethod : Foundatio.Xunit.TestWithLoggingBase
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
                _logger.ErrorIfEnabled(exception, () => "TEST");
                _ = Assert.Single(Log.LogEntries);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LogExtensions.InformationIfEnabled(ILogger, Func{string})"/>.
        /// </summary>
        public sealed class InformationIfEnabledMethod : Foundatio.Xunit.TestWithLoggingBase
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
                _logger.InformationIfEnabled(() => "TEST");
                _ = Assert.Single(Log.LogEntries);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LogExtensions.ErrorIfEnabled(ILogger, Exception, Func{string})"/>.
        /// </summary>
        public sealed class InformationIfEnabledWithExceptionMethod : Foundatio.Xunit.TestWithLoggingBase
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
                _logger.InformationIfEnabled(exception, () => "TEST");
                _ = Assert.Single(Log.LogEntries);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LogExtensions.DebugIfEnabled(ILogger, Func{string})"/>.
        /// </summary>
        public sealed class DebugIfEnabledMethod : Foundatio.Xunit.TestWithLoggingBase
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
                _logger.DebugIfEnabled(() => "TEST");
                _ = Assert.Single(Log.LogEntries);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LogExtensions.ErrorIfEnabled(ILogger, Exception, Func{string})"/>.
        /// </summary>
        public sealed class DebugIfEnabledWithExceptionMethod : Foundatio.Xunit.TestWithLoggingBase
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
                var exception = new InvalidOperationException("Some test exception");
                _logger.DebugIfEnabled(exception, () => "TEST");
                _ = Assert.Single(Log.LogEntries);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LogExtensions.CriticalIfEnabled(ILogger, Func{string})"/>.
        /// </summary>
        public sealed class CriticalIfEnabledMethod : Foundatio.Xunit.TestWithLoggingBase
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
                _logger.CriticalIfEnabled(() => "TEST");
                _ = Assert.Single(Log.LogEntries);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LogExtensions.CriticalIfEnabled(ILogger, Exception, Func{string})"/>.
        /// </summary>
        public sealed class CriticalIfEnabledWithExceptionMethod : Foundatio.Xunit.TestWithLoggingBase
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
                _logger.CriticalIfEnabled(exception, () => "TEST");
                _ = Assert.Single(Log.LogEntries);
            }
        }
    }
}
