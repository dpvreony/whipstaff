// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;
using Whipstaff.Core.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Whipstaff.UnitTests.Core.Logging
{
    /// <summary>
    /// Unit Tests for <see cref="LoggerMessageFactory"/>.
    /// </summary>
    public static class LoggerMessageFactoryTests
    {
        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.GetDbContextSaveResultLoggerMessageAction"/>.
        /// </summary>
        public sealed class GetDbContextSaveResultLoggerMessageActionMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetDbContextSaveResultLoggerMessageActionMethod"/> class.
            /// </summary>
                        /// <param name="output">XUnit test output helper instance.</param>
            public GetDbContextSaveResultLoggerMessageActionMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure a log message action instance is created.
            /// </summary>
            [Fact]
            public void ReturnsLogMessageAction()
            {
                var instance = LoggerMessageFactory.GetDbContextSaveResultLoggerMessageAction();
                Assert.NotNull(instance);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.GetNoMediatRHandlersRegisteredForTypeLoggerMessageAction"/>.
        /// </summary>
        public sealed class GetNoMediatRHandlersRegisteredForTypeLoggerMessageActionMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetNoMediatRHandlersRegisteredForTypeLoggerMessageActionMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public GetNoMediatRHandlersRegisteredForTypeLoggerMessageActionMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure a log message action instance is created.
            /// </summary>
            [Fact]
            public void ReturnsLogMessageAction()
            {
                var instance = LoggerMessageFactory.GetDbContextSaveResultLoggerMessageAction();
                Assert.NotNull(instance);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.GetCountOfMediatRHandlersRegisteredLoggerMessageAction"/>.
        /// </summary>
        public sealed class GetCountOfMediatRHandlersRegisteredLoggerMessageActionMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetCountOfMediatRHandlersRegisteredLoggerMessageActionMethod"/> class.
            /// </summary>
                        /// <param name="output">XUnit test output helper instance.</param>
            public GetCountOfMediatRHandlersRegisteredLoggerMessageActionMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure a log message action instance is created.
            /// </summary>
            [Fact]
            public void ReturnsLogMessageAction()
            {
                var instance = LoggerMessageFactory.GetDbContextSaveResultLoggerMessageAction();
                Assert.NotNull(instance);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.GetCriticalBasicLoggerMessageActionForEventId"/>.
        /// </summary>
        public sealed class GetCriticalBasicLoggerMessageActionForEventIdMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetCriticalBasicLoggerMessageActionForEventIdMethod"/> class.
            /// </summary>
                        /// <param name="output">XUnit test output helper instance.</param>
            public GetCriticalBasicLoggerMessageActionForEventIdMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure a log message action instance is created.
            /// </summary>
            [Fact]
            public void ReturnsLogMessageAction()
            {
                var instance = LoggerMessageFactory.GetDbContextSaveResultLoggerMessageAction();
                Assert.NotNull(instance);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.GetCriticalBasicLoggerMessageActionForEventIdWithFunc"/>.
        /// </summary>
        public sealed class GetCriticalBasicLoggerMessageActionForEventIdWithFuncMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetCriticalBasicLoggerMessageActionForEventIdWithFuncMethod"/> class.
            /// </summary>
                        /// <param name="output">XUnit test output helper instance.</param>
            public GetCriticalBasicLoggerMessageActionForEventIdWithFuncMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure a log message action instance is created.
            /// </summary>
            [Fact]
            public void ReturnsLogMessageAction()
            {
                var instance = LoggerMessageFactory.GetDbContextSaveResultLoggerMessageAction();
                Assert.NotNull(instance);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.GetDebugBasicLoggerMessageActionForEventId"/>.
        /// </summary>
        public sealed class GetDebugBasicLoggerMessageActionForEventIdMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetDebugBasicLoggerMessageActionForEventIdMethod"/> class.
            /// </summary>
                        /// <param name="output">XUnit test output helper instance.</param>
            public GetDebugBasicLoggerMessageActionForEventIdMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure a log message action instance is created.
            /// </summary>
            [Fact]
            public void ReturnsLogMessageAction()
            {
                var instance = LoggerMessageFactory.GetDbContextSaveResultLoggerMessageAction();
                Assert.NotNull(instance);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.GetDebugBasicLoggerMessageActionForEventIdAndFunc"/>.
        /// </summary>
        public sealed class GetDebugBasicLoggerMessageActionForEventIdAndFuncMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetDebugBasicLoggerMessageActionForEventIdAndFuncMethod"/> class.
            /// </summary>
                        /// <param name="output">XUnit test output helper instance.</param>
            public GetDebugBasicLoggerMessageActionForEventIdAndFuncMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure a log message action instance is created.
            /// </summary>
            [Fact]
            public void ReturnsLogMessageAction()
            {
                var instance = LoggerMessageFactory.GetDbContextSaveResultLoggerMessageAction();
                Assert.NotNull(instance);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.GetErrorBasicLoggerMessageActionForEventId"/>.
        /// </summary>
        public sealed class GetErrorBasicLoggerMessageActionForEventIdMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetErrorBasicLoggerMessageActionForEventIdMethod"/> class.
            /// </summary>
                        /// <param name="output">XUnit test output helper instance.</param>
            public GetErrorBasicLoggerMessageActionForEventIdMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure a log message action instance is created.
            /// </summary>
            [Fact]
            public void ReturnsLogMessageAction()
            {
                var instance = LoggerMessageFactory.GetDbContextSaveResultLoggerMessageAction();
                Assert.NotNull(instance);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.GetErrorBasicLoggerMessageActionForEventIdAndFunc"/>.
        /// </summary>
        public sealed class GetErrorBasicLoggerMessageActionForEventIdAndFuncMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetErrorBasicLoggerMessageActionForEventIdAndFuncMethod"/> class.
            /// </summary>
                        /// <param name="output">XUnit test output helper instance.</param>
            public GetErrorBasicLoggerMessageActionForEventIdAndFuncMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure a log message action instance is created.
            /// </summary>
            [Fact]
            public void ReturnsLogMessageAction()
            {
                var instance = LoggerMessageFactory.GetDbContextSaveResultLoggerMessageAction();
                Assert.NotNull(instance);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.GetInformationBasicLoggerMessageActionForEventId"/>.
        /// </summary>
        public sealed class GetInformationBasicLoggerMessageActionForEventIdMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetInformationBasicLoggerMessageActionForEventIdMethod"/> class.
            /// </summary>
                        /// <param name="output">XUnit test output helper instance.</param>
            public GetInformationBasicLoggerMessageActionForEventIdMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure a log message action instance is created.
            /// </summary>
            [Fact]
            public void ReturnsLogMessageAction()
            {
                var instance = LoggerMessageFactory.GetDbContextSaveResultLoggerMessageAction();
                Assert.NotNull(instance);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.GetInformationBasicLoggerMessageActionForEventIdAndFunc"/>.
        /// </summary>
        public sealed class GetInformationBasicLoggerMessageActionForEventIdAndFuncMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetInformationBasicLoggerMessageActionForEventIdAndFuncMethod"/> class.
            /// </summary>
                        /// <param name="output">XUnit test output helper instance.</param>
            public GetInformationBasicLoggerMessageActionForEventIdAndFuncMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure a log message action instance is created.
            /// </summary>
            [Fact]
            public void ReturnsLogMessageAction()
            {
                var instance = LoggerMessageFactory.GetDbContextSaveResultLoggerMessageAction();
                Assert.NotNull(instance);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.GetWarningBasicLoggerMessageActionForEventId"/>.
        /// </summary>
        public sealed class GetWarningBasicLoggerMessageActionForEventIdMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetWarningBasicLoggerMessageActionForEventIdMethod"/> class.
            /// </summary>
                        /// <param name="output">XUnit test output helper instance.</param>
            public GetWarningBasicLoggerMessageActionForEventIdMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure a log message action instance is created.
            /// </summary>
            [Fact]
            public void ReturnsLogMessageAction()
            {
                var instance = LoggerMessageFactory.GetDbContextSaveResultLoggerMessageAction();
                Assert.NotNull(instance);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.GetWarningBasicLoggerMessageActionForEventIdAndFunc"/>.
        /// </summary>
        public sealed class GetWarningBasicLoggerMessageActionForEventIdAndFuncMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetWarningBasicLoggerMessageActionForEventIdAndFuncMethod"/> class.
            /// </summary>
                        /// <param name="output">XUnit test output helper instance.</param>
            public GetWarningBasicLoggerMessageActionForEventIdAndFuncMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure a log message action instance is created.
            /// </summary>
            [Fact]
            public void ReturnsLogMessageAction()
            {
                var instance = LoggerMessageFactory.GetDbContextSaveResultLoggerMessageAction();
                Assert.NotNull(instance);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.GetBasicLoggerMessageActionForLogLevelAndEventId"/>.
        /// </summary>
        public sealed class GetBasicLoggerMessageActionForLogLevelAndEventIdMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetBasicLoggerMessageActionForLogLevelAndEventIdMethod"/> class.
            /// </summary>
                        /// <param name="output">XUnit test output helper instance.</param>
            public GetBasicLoggerMessageActionForLogLevelAndEventIdMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure a log message action instance is created.
            /// </summary>
            [Fact]
            public void ReturnsLogMessageAction()
            {
                var instance = LoggerMessageFactory.GetBasicLoggerMessageActionForLogLevelAndEventId(LogLevel.Information, new EventId(1, "Event"));
                Assert.NotNull(instance);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.GetBasicLoggerMessageActionForLogLevelAndEventIdAndFunc"/>.
        /// </summary>
        public sealed class GetBasicLoggerMessageActionForLogLevelAndEventIdAndFuncMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetBasicLoggerMessageActionForLogLevelAndEventIdAndFuncMethod"/> class.
            /// </summary>
                        /// <param name="output">XUnit test output helper instance.</param>
            public GetBasicLoggerMessageActionForLogLevelAndEventIdAndFuncMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure a log message action instance is created.
            /// </summary>
            [Fact]
            public void ReturnsLogMessageAction()
            {
                var instance = LoggerMessageFactory.GetBasicLoggerMessageActionForLogLevelAndEventIdAndFunc(LogLevel.Information, new EventId(1, "Event"));
                Assert.NotNull(instance);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.DefineForFunc{T1}(LogLevel, EventId, string)"/>.
        /// </summary>
        public sealed class DefineForFuncT1Method : Foundatio.Xunit.TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DefineForFuncT1Method"/> class.
            /// </summary>
                        /// <param name="output">XUnit test output helper instance.</param>
            public DefineForFuncT1Method(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure a log message action instance is created.
            /// </summary>
            [Fact]
            public void ReturnsLogMessageAction()
            {
                var instance = LoggerMessageFactory.DefineForFunc<int>(
                    LogLevel.Information,
                    new EventId(1, "Event"),
                    "Some Log Message. {Arg}");

                Assert.NotNull(instance);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.DefineForFunc{T1}(LogLevel, EventId, string, LogDefineOptions)"/>.
        /// </summary>
        public sealed class DefineForFuncT1WithOptionsMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DefineForFuncT1WithOptionsMethod"/> class.
            /// </summary>
                        /// <param name="output">XUnit test output helper instance.</param>
            public DefineForFuncT1WithOptionsMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure a log message action instance is created.
            /// </summary>
            [Fact]
            public void ReturnsLogMessageAction()
            {
                var instance = LoggerMessageFactory.DefineForFunc<int>(
                    LogLevel.Information,
                    new EventId(1, "Event"),
                    "Some Log Message. {Arg}",
                    new LogDefineOptions());
                Assert.NotNull(instance);
            }
        }
    }
}
