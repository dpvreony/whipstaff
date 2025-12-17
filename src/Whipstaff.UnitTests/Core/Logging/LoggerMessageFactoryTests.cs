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
    /// Unit Tests for <see cref="LoggerMessageFactory"/>.
    /// </summary>
    public static class LoggerMessageFactoryTests
    {
        private const string FormatString1 = "Some LoggerFactory Message. {Arg}";
        private const string FormatString2 = "Some LoggerFactory Message. {Arg1} {Arg2}";
        private const string FormatString3 = "Some LoggerFactory Message. {Arg1} {Arg2} {Arg3}";
        private const string FormatString4 = "Some LoggerFactory Message. {Arg1} {Arg2} {Arg3} {Arg4}";
        private const string FormatString5 = "Some LoggerFactory Message. {Arg1} {Arg2} {Arg3} {Arg4} {Arg5}";
        private const string FormatString6 = "Some LoggerFactory Message. {Arg1} {Arg2} {Arg3} {Arg4} {Arg5} {Arg6}";

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.GetDbContextSaveResultLoggerMessageAction"/>.
        /// </summary>
        public sealed class GetDbContextSaveResultLoggerMessageActionMethod : TestWithLoggingBase
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
        /// Unit Tests for <see cref="LoggerMessageFactory.GetNoMediatorHandlersRegisteredForTypeLoggerMessageAction"/>.
        /// </summary>
        public sealed class GetNoMediatorHandlersRegisteredForTypeLoggerMessageActionMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetNoMediatorHandlersRegisteredForTypeLoggerMessageActionMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public GetNoMediatorHandlersRegisteredForTypeLoggerMessageActionMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure a log message action instance is created.
            /// </summary>
            [Fact]
            public void ReturnsLogMessageAction()
            {
                var instance = LoggerMessageFactory.GetNoMediatorHandlersRegisteredForTypeLoggerMessageAction();
                Assert.NotNull(instance);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.GetCountOfMediatorHandlersRegisteredLoggerMessageAction"/>.
        /// </summary>
        public sealed class GetCountOfMediatRHandlersRegisteredLoggerMessageActionMethod : TestWithLoggingBase
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
                var instance = LoggerMessageFactory.GetCountOfMediatorHandlersRegisteredLoggerMessageAction();
                Assert.NotNull(instance);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.GetCriticalBasicLoggerMessageActionForEventId"/>.
        /// </summary>
        public sealed class GetCriticalBasicLoggerMessageActionForEventIdMethod : TestWithLoggingBase
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
                var instance = LoggerMessageFactory.GetCriticalBasicLoggerMessageActionForEventId(new EventId(1, "Event"));
                Assert.NotNull(instance);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.GetCriticalBasicLoggerMessageActionForEventIdAndFunc"/>.
        /// </summary>
        public sealed class GetCriticalBasicLoggerMessageActionForEventIdAndFuncMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetCriticalBasicLoggerMessageActionForEventIdAndFuncMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public GetCriticalBasicLoggerMessageActionForEventIdAndFuncMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure a log message action instance is created.
            /// </summary>
            [Fact]
            public void ReturnsLogMessageAction()
            {
                var instance = LoggerMessageFactory.GetCriticalBasicLoggerMessageActionForEventIdAndFunc(new EventId(1, "Event"));
                Assert.NotNull(instance);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.GetDebugBasicLoggerMessageActionForEventId"/>.
        /// </summary>
        public sealed class GetDebugBasicLoggerMessageActionForEventIdMethod : TestWithLoggingBase
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
                var instance = LoggerMessageFactory.GetDebugBasicLoggerMessageActionForEventId(new EventId(1, "Event"));
                Assert.NotNull(instance);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.GetDebugBasicLoggerMessageActionForEventIdAndFunc"/>.
        /// </summary>
        public sealed class GetDebugBasicLoggerMessageActionForEventIdAndFuncMethod : TestWithLoggingBase
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
                var instance = LoggerMessageFactory.GetCriticalBasicLoggerMessageActionForEventIdAndFunc(new EventId(1, "Event"));
                Assert.NotNull(instance);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.GetErrorBasicLoggerMessageActionForEventId"/>.
        /// </summary>
        public sealed class GetErrorBasicLoggerMessageActionForEventIdMethod : TestWithLoggingBase
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
                var instance = LoggerMessageFactory.GetErrorBasicLoggerMessageActionForEventId(new EventId(1, "Event"));
                Assert.NotNull(instance);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.GetErrorBasicLoggerMessageActionForEventIdAndFunc"/>.
        /// </summary>
        public sealed class GetErrorBasicLoggerMessageActionForEventIdAndFuncMethod : TestWithLoggingBase
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
                var instance = LoggerMessageFactory.GetErrorBasicLoggerMessageActionForEventIdAndFunc(new EventId(1, "Event"));
                Assert.NotNull(instance);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.GetInformationBasicLoggerMessageActionForEventId"/>.
        /// </summary>
        public sealed class GetInformationBasicLoggerMessageActionForEventIdMethod : TestWithLoggingBase
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
                var instance = LoggerMessageFactory.GetInformationBasicLoggerMessageActionForEventId(new EventId(1, "Event"));
                Assert.NotNull(instance);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.GetInformationBasicLoggerMessageActionForEventIdAndFunc"/>.
        /// </summary>
        public sealed class GetInformationBasicLoggerMessageActionForEventIdAndFuncMethod : TestWithLoggingBase
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
                var instance = LoggerMessageFactory.GetInformationBasicLoggerMessageActionForEventIdAndFunc(new EventId(1, "Event"));
                Assert.NotNull(instance);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.GetWarningBasicLoggerMessageActionForEventId"/>.
        /// </summary>
        public sealed class GetWarningBasicLoggerMessageActionForEventIdMethod : TestWithLoggingBase
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
                var instance = LoggerMessageFactory.GetWarningBasicLoggerMessageActionForEventId(new EventId(1, "Event"));
                Assert.NotNull(instance);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.GetWarningBasicLoggerMessageActionForEventIdAndFunc"/>.
        /// </summary>
        public sealed class GetWarningBasicLoggerMessageActionForEventIdAndFuncMethod : TestWithLoggingBase
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
                var instance = LoggerMessageFactory.GetWarningBasicLoggerMessageActionForEventIdAndFunc(new EventId(1, "Event"));
                Assert.NotNull(instance);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.GetBasicLoggerMessageActionForLogLevelAndEventId"/>.
        /// </summary>
        public sealed class GetBasicLoggerMessageActionForLogLevelAndEventIdMethod : TestWithLoggingBase
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
        public sealed class GetBasicLoggerMessageActionForLogLevelAndEventIdAndFuncMethod : TestWithLoggingBase
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
        /// Abstraction of unit tests for logger message define calls.
        /// </summary>
        /// <typeparam name="TLogMessageAction">The action signature for the log message action.</typeparam>
        public abstract class AbstractDefineForFuncMethod<TLogMessageAction> : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="AbstractDefineForFuncMethod{TAction}"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            protected AbstractDefineForFuncMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure a log message action instance is created.
            /// </summary>
            [Fact]
            public void ReturnsLogMessageAction()
            {
                var instance = GetLoggerMessageAction();

                Assert.NotNull(instance);
            }

            /// <summary>
            /// Test to ensure a message is logged when the log level allows.
            /// </summary>
            [Fact]
            public void LogsMessage()
            {
                var instance = GetLoggerMessageAction();

                var count = LoggerFactory.LogEntries.Count;
                var callCount = 0;

                InvokeLogMessageAction(
                    instance,
                    () =>
                    {
                        callCount++;
                        return 1;
                    });

                Assert.True(LoggerFactory.LogEntries.Count > count);
                Assert.Equal(1, callCount);
            }

            /// <summary>
            /// Test to ensure a log message action instance is created.
            /// </summary>
            [Fact]
            public void SkipsMessage()
            {
                var instance = GetLoggerMessageAction();

                LoggerFactory.DefaultMinimumLevel = LogLevel.Error;

                var count = LoggerFactory.LogEntries.Count;
                var callCount = 0;

                InvokeLogMessageAction(
                    instance,
                    () =>
                    {
                        callCount++;
                        return 1;
                    });

                Assert.Equal(LoggerFactory.LogEntries.Count, count);
                Assert.Equal(0, callCount);
            }

            /// <summary>
            /// Logger Message Action to test.
            /// </summary>
            /// <returns>Logger Message Action instance.</returns>
            protected abstract TLogMessageAction GetLoggerMessageAction();

            /// <summary>
            /// Called to allow the implementing test class to fire off the logger with the correct number of args.
            /// </summary>
            /// <param name="instance">Logger Message Action instance.</param>
            /// <param name="trackingFunc">The func to pass into arg 1 to track execution counts.</param>
            protected abstract void InvokeLogMessageAction(TLogMessageAction instance, Func<int> trackingFunc);
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.DefineForFunc{T1}(LogLevel, EventId, string)"/>.
        /// </summary>
        public sealed class DefineForFuncT1Method : AbstractDefineForFuncMethod<Action<ILogger, Func<int>, Exception?>>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DefineForFuncT1Method"/> class.
            /// </summary>
                        /// <param name="output">XUnit test output helper instance.</param>
            public DefineForFuncT1Method(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            protected override Action<ILogger, Func<int>, Exception?> GetLoggerMessageAction()
            {
                return LoggerMessageFactory.DefineForFunc<int>(
                    LogLevel.Information,
                    new EventId(1, "Event"),
                    FormatString1);
            }

            /// <inheritdoc/>
            protected override void InvokeLogMessageAction(Action<ILogger, Func<int>, Exception?> instance, Func<int> trackingFunc)
            {
                instance(
                    Logger,
                    trackingFunc,
                    null);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.DefineForFunc{T1}(LogLevel, EventId, string, LogDefineOptions)"/>.
        /// </summary>
        public sealed class DefineForFuncT1WithOptionsMethod : AbstractDefineForFuncMethod<Action<ILogger, Func<int>, Exception?>>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DefineForFuncT1WithOptionsMethod"/> class.
            /// </summary>
                        /// <param name="output">XUnit test output helper instance.</param>
            public DefineForFuncT1WithOptionsMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            protected override Action<ILogger, Func<int>, Exception?> GetLoggerMessageAction()
            {
                return LoggerMessageFactory.DefineForFunc<int>(
                    LogLevel.Information,
                    new EventId(1, "Event"),
                    FormatString1,
                    new LogDefineOptions());
            }

            /// <inheritdoc/>
            protected override void InvokeLogMessageAction(Action<ILogger, Func<int>, Exception?> instance, Func<int> trackingFunc)
            {
                instance(
                    Logger,
                    trackingFunc,
                    null);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.DefineForFunc{T1, T2}(LogLevel, EventId, string)"/>.
        /// </summary>
        public sealed class DefineForFuncT2Method : AbstractDefineForFuncMethod<Action<ILogger, Func<int>, Func<int>, Exception?>>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DefineForFuncT2Method"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public DefineForFuncT2Method(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            protected override Action<ILogger, Func<int>, Func<int>, Exception?> GetLoggerMessageAction()
            {
                return LoggerMessageFactory.DefineForFunc<int, int>(
                    LogLevel.Information,
                    new EventId(1, "Event"),
                    FormatString2);
            }

            /// <inheritdoc/>
            protected override void InvokeLogMessageAction(Action<ILogger, Func<int>, Func<int>, Exception?> instance, Func<int> trackingFunc)
            {
                instance(
                    Logger,
                    trackingFunc,
                    () => 2,
                    null);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.DefineForFunc{T1, T2}(LogLevel, EventId, string, LogDefineOptions)"/>.
        /// </summary>
        public sealed class DefineForFuncT2WithOptionsMethod : AbstractDefineForFuncMethod<Action<ILogger, Func<int>, Func<int>, Exception?>>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DefineForFuncT2WithOptionsMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public DefineForFuncT2WithOptionsMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            protected override Action<ILogger, Func<int>, Func<int>, Exception?> GetLoggerMessageAction()
            {
                return LoggerMessageFactory.DefineForFunc<int, int>(
                    LogLevel.Information,
                    new EventId(1, "Event"),
                    FormatString2,
                    new LogDefineOptions());
            }

            /// <inheritdoc/>
            protected override void InvokeLogMessageAction(Action<ILogger, Func<int>, Func<int>, Exception?> instance, Func<int> trackingFunc)
            {
                instance(
                    Logger,
                    trackingFunc,
                    () => 2,
                    null);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.DefineForFunc{T1, T2, T3}(LogLevel, EventId, string)"/>.
        /// </summary>
        public sealed class DefineForFuncT3Method : AbstractDefineForFuncMethod<Action<ILogger, Func<int>, Func<int>, Func<int>, Exception?>>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DefineForFuncT3Method"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public DefineForFuncT3Method(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            protected override Action<ILogger, Func<int>, Func<int>, Func<int>, Exception?> GetLoggerMessageAction()
            {
                return LoggerMessageFactory.DefineForFunc<int, int, int>(
                    LogLevel.Information,
                    new EventId(1, "Event"),
                    FormatString3);
            }

            /// <inheritdoc/>
            protected override void InvokeLogMessageAction(Action<ILogger, Func<int>, Func<int>, Func<int>, Exception?> instance, Func<int> trackingFunc)
            {
                instance(
                    Logger,
                    trackingFunc,
                    () => 2,
                    () => 3,
                    null);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.DefineForFunc{T1, T2, T3}(LogLevel, EventId, string, LogDefineOptions)"/>.
        /// </summary>
        public sealed class DefineForFuncT3WithOptionsMethod : AbstractDefineForFuncMethod<Action<ILogger, Func<int>, Func<int>, Func<int>, Exception?>>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DefineForFuncT3WithOptionsMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public DefineForFuncT3WithOptionsMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            protected override Action<ILogger, Func<int>, Func<int>, Func<int>, Exception?> GetLoggerMessageAction()
            {
                return LoggerMessageFactory.DefineForFunc<int, int, int>(
                    LogLevel.Information,
                    new EventId(1, "Event"),
                    FormatString3,
                    new LogDefineOptions());
            }

            /// <inheritdoc/>
            protected override void InvokeLogMessageAction(Action<ILogger, Func<int>, Func<int>, Func<int>, Exception?> instance, Func<int> trackingFunc)
            {
                instance(
                    Logger,
                    trackingFunc,
                    () => 2,
                    () => 3,
                    null);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.DefineForFunc{T1, T2, T3, T4}(LogLevel, EventId, string)"/>.
        /// </summary>
        public sealed class DefineForFuncT4Method : AbstractDefineForFuncMethod<Action<ILogger, Func<int>, Func<int>, Func<int>, Func<int>, Exception?>>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DefineForFuncT4Method"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public DefineForFuncT4Method(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            protected override Action<ILogger, Func<int>, Func<int>, Func<int>, Func<int>, Exception?> GetLoggerMessageAction()
            {
                return LoggerMessageFactory.DefineForFunc<int, int, int, int>(
                    LogLevel.Information,
                    new EventId(1, "Event"),
                    FormatString4);
            }

            /// <inheritdoc/>
            protected override void InvokeLogMessageAction(Action<ILogger, Func<int>, Func<int>, Func<int>, Func<int>, Exception?> instance, Func<int> trackingFunc)
            {
                instance(
                    Logger,
                    trackingFunc,
                    () => 2,
                    () => 3,
                    () => 4,
                    null);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.DefineForFunc{T1, T2, T3, T4}(LogLevel, EventId, string, LogDefineOptions)"/>.
        /// </summary>
        public sealed class DefineForFuncT4WithOptionsMethod : AbstractDefineForFuncMethod<Action<ILogger, Func<int>, Func<int>, Func<int>, Func<int>, Exception?>>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DefineForFuncT4WithOptionsMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public DefineForFuncT4WithOptionsMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            protected override Action<ILogger, Func<int>, Func<int>, Func<int>, Func<int>, Exception?> GetLoggerMessageAction()
            {
                return LoggerMessageFactory.DefineForFunc<int, int, int, int>(
                    LogLevel.Information,
                    new EventId(1, "Event"),
                    FormatString4,
                    new LogDefineOptions());
            }

            /// <inheritdoc/>
            protected override void InvokeLogMessageAction(Action<ILogger, Func<int>, Func<int>, Func<int>, Func<int>, Exception?> instance, Func<int> trackingFunc)
            {
                instance(
                    Logger,
                    trackingFunc,
                    () => 2,
                    () => 3,
                    () => 4,
                    null);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.DefineForFunc{T1, T2, T3, T4, T5}(LogLevel, EventId, string)"/>.
        /// </summary>
        public sealed class DefineForFuncT5Method : AbstractDefineForFuncMethod<Action<ILogger, Func<int>, Func<int>, Func<int>, Func<int>, Func<int>, Exception?>>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DefineForFuncT5Method"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public DefineForFuncT5Method(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            protected override Action<ILogger, Func<int>, Func<int>, Func<int>, Func<int>, Func<int>, Exception?> GetLoggerMessageAction()
            {
                return LoggerMessageFactory.DefineForFunc<int, int, int, int, int>(
                    LogLevel.Information,
                    new EventId(1, "Event"),
                    FormatString5);
            }

            /// <inheritdoc/>
            protected override void InvokeLogMessageAction(Action<ILogger, Func<int>, Func<int>, Func<int>, Func<int>, Func<int>, Exception?> instance, Func<int> trackingFunc)
            {
                instance(
                    Logger,
                    trackingFunc,
                    () => 2,
                    () => 3,
                    () => 4,
                    () => 5,
                    null);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.DefineForFunc{T1, T2, T3, T4, T5}(LogLevel, EventId, string, LogDefineOptions)"/>.
        /// </summary>
        public sealed class DefineForFuncT5WithOptionsMethod : AbstractDefineForFuncMethod<Action<ILogger, Func<int>, Func<int>, Func<int>, Func<int>, Func<int>, Exception?>>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DefineForFuncT5WithOptionsMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public DefineForFuncT5WithOptionsMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            protected override Action<ILogger, Func<int>, Func<int>, Func<int>, Func<int>, Func<int>, Exception?> GetLoggerMessageAction()
            {
                return LoggerMessageFactory.DefineForFunc<int, int, int, int, int>(
                    LogLevel.Information,
                    new EventId(1, "Event"),
                    FormatString5,
                    new LogDefineOptions());
            }

            /// <inheritdoc/>
            protected override void InvokeLogMessageAction(Action<ILogger, Func<int>, Func<int>, Func<int>, Func<int>, Func<int>, Exception?> instance, Func<int> trackingFunc)
            {
                instance(
                    Logger,
                    trackingFunc,
                    () => 2,
                    () => 3,
                    () => 4,
                    () => 5,
                    null);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.DefineForFunc{T1, T2, T3, T4, T5, T6}(LogLevel, EventId, string)"/>.
        /// </summary>
        public sealed class DefineForFuncT6Method : AbstractDefineForFuncMethod<Action<ILogger, Func<int>, Func<int>, Func<int>, Func<int>, Func<int>, Func<int>, Exception?>>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DefineForFuncT6Method"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public DefineForFuncT6Method(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            protected override Action<ILogger, Func<int>, Func<int>, Func<int>, Func<int>, Func<int>, Func<int>, Exception?> GetLoggerMessageAction()
            {
                return LoggerMessageFactory.DefineForFunc<int, int, int, int, int, int>(
                    LogLevel.Information,
                    new EventId(1, "Event"),
                    FormatString6);
            }

            /// <inheritdoc/>
            protected override void InvokeLogMessageAction(Action<ILogger, Func<int>, Func<int>, Func<int>, Func<int>, Func<int>, Func<int>, Exception?> instance, Func<int> trackingFunc)
            {
                instance(
                    Logger,
                    trackingFunc,
                    () => 2,
                    () => 3,
                    () => 4,
                    () => 5,
                    () => 6,
                    null);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="LoggerMessageFactory.DefineForFunc{T1, T2, T3, T4, T5, T6}(LogLevel, EventId, string, LogDefineOptions)"/>.
        /// </summary>
        public sealed class DefineForFuncT6WithOptionsMethod : AbstractDefineForFuncMethod<Action<ILogger, Func<int>, Func<int>, Func<int>, Func<int>, Func<int>, Func<int>, Exception?>>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DefineForFuncT6WithOptionsMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public DefineForFuncT6WithOptionsMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            protected override Action<ILogger, Func<int>, Func<int>, Func<int>, Func<int>, Func<int>, Func<int>, Exception?> GetLoggerMessageAction()
            {
                return LoggerMessageFactory.DefineForFunc<int, int, int, int, int, int>(
                    LogLevel.Information,
                    new EventId(1, "Event"),
                    FormatString6,
                    new LogDefineOptions());
            }

            /// <inheritdoc/>
            protected override void InvokeLogMessageAction(Action<ILogger, Func<int>, Func<int>, Func<int>, Func<int>, Func<int>, Func<int>, Exception?> instance, Func<int> trackingFunc)
            {
                instance(
                    Logger,
                    trackingFunc,
                    () => 2,
                    () => 3,
                    () => 4,
                    () => 5,
                    () => 6,
                    null);
            }
        }
    }
}
