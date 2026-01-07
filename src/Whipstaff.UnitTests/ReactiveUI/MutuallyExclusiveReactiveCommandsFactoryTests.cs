// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using NetTestRegimentation.XUnit.Theories.ArgumentNullException;
using ReactiveUI;
using ReactiveUI.Testing;
using Whipstaff.ReactiveUI.ReactiveCommands;
using Xunit;

namespace Whipstaff.UnitTests.ReactiveUI
{
    /// <summary>
    /// Unit Tests for <see cref="MutuallyExclusiveReactiveCommandsFactory"/>.
    /// </summary>
    public static class MutuallyExclusiveReactiveCommandsFactoryTests
    {
        private static (Func<bool> IsExecuting, Func<bool> CanExecute, ReactiveCommand<Unit, Unit> Command, CancellationTokenSource CancellationTokenSource) GetTestRow((ReactiveCommand<Unit, Unit> Command, CancellationTokenSource CancellationTokenSource) current)
        {
            var currentCommand = current.Command;
            bool canExecute = false;
            _ = currentCommand.CanExecute.Subscribe(currentValue =>
            {
                canExecute = currentValue;
            });

            bool isExecuting = false;
            _ = currentCommand.IsExecuting.Subscribe(currentValue =>
            {
                isExecuting = currentValue;
            });

            return (
                () => isExecuting,
                () => canExecute,
                currentCommand,
                current.CancellationTokenSource);
        }

        private static async Task TestEachCommandIsMutuallyExclusiveAsync(
            (ReactiveCommand<Unit, Unit> Command, CancellationTokenSource CancellationTokenSource)[] commandsToTest,
            TestScheduler testScheduler)
        {
            var sut = commandsToTest.Select(currentCommand => GetTestRow(currentCommand)).ToArray();

            await TestEachCommandIsMutuallyExclusiveAsync(
                sut,
                testScheduler);
        }

        private static async Task TestEachCommandIsMutuallyExclusiveAsync(
            (Func<bool> IsExecuting, Func<bool> CanExecute, ReactiveCommand<Unit, Unit> Command, CancellationTokenSource CancellationTokenSource)[] commandsToTest,
            TestScheduler testScheduler)
        {
            for (int i = 0; i < commandsToTest.Length; i++)
            {
                var currentItem = commandsToTest[i];
                testScheduler.Start();

                Assert.All(commandsToTest.Select(x => x.CanExecute()), b => Assert.True(b));
                Assert.All(commandsToTest.Select(x => x.IsExecuting()), b => Assert.False(b));

                var currentCommand = currentItem.Command;
                var currentCancellationToken = currentItem.CancellationTokenSource;

                var currentExecution = currentCommand.Execute(Unit.Default).ToTask(CancellationToken.None);
                testScheduler.Start();

                Assert.All(commandsToTest.Select(x => x.CanExecute()), b => Assert.False(b));
                Assert.True(currentItem.IsExecuting());

                var allOtherItems = commandsToTest[..i].Concat(commandsToTest[(i + 1)..]).ToArray();
                Assert.All(allOtherItems.Select(x => x.IsExecuting()), b => Assert.False(b));

                await currentCancellationToken.CancelAsync();
                testScheduler.Start();

                _ = await currentExecution;
                testScheduler.Start();

                Assert.All(commandsToTest.Select(x => x.CanExecute()), b => Assert.True(b));
                Assert.All(commandsToTest.Select(x => x.IsExecuting()), b => Assert.False(b));
            }
        }

        private static async Task<Unit> RunUntilToldToStopAsync(Unit param, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
#pragma warning disable CA1031 // Do not catch general exception types
                try
                {
                    await Task.Delay(100, token);
                }
                catch
                {
                    // no op
                }
#pragma warning restore CA1031 // Do not catch general exception types
            }

            return param;
        }

        /// <summary>
        /// Unit tests for <see cref="MutuallyExclusiveReactiveCommandsFactory.Create{T1Param, T1CommandResult, T1ExecuteResult, T2Param, T2CommandResult, T2ExecuteResult}(ReactiveCommandFactoryArgument{T1Param, T1CommandResult, T1ExecuteResult}, ReactiveCommandFactoryArgument{T2Param, T2CommandResult, T2ExecuteResult})"/>.
        /// </summary>
        public sealed class CreateT6Method : NetTestRegimentation.ITestMethodWithNullableParameters<
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(CreateT6MethodThrowsArgumentNullTestSource))]
            public void ThrowsArgumentNullException(
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg1,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg2,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => MutuallyExclusiveReactiveCommandsFactory.Create(
                        arg1!,
                        arg2!));
            }

            /// <summary>
            /// Test that the method creates mutually exclusive commands.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
            [Fact]
            public async Task ProducesMutuallyExclusiveCommands()
            {
                var testScheduler = new TestScheduler();
                using (SchedulerExtensions.WithScheduler(testScheduler))
                using (var firstCancellationToken = new CancellationTokenSource())
                using (var secondCancellationToken = new CancellationTokenSource())
                {
                    RxSchedulers.MainThreadScheduler = testScheduler;
                    RxSchedulers.TaskpoolScheduler = testScheduler;

                    var first = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, firstCancellationToken.Token));
                    var second = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, secondCancellationToken.Token));

                    (BehaviorSubject<bool> nobodyIsExecuting,
                        IDisposable exclusiveLock,
                        ReactiveCommand<Unit, Unit> firstCommand,
                        ReactiveCommand<Unit, Unit> secondCommand) = MutuallyExclusiveReactiveCommandsFactory.Create(
                        first,
                        second);

                    Assert.NotNull(nobodyIsExecuting);
                    Assert.NotNull(exclusiveLock);
                    Assert.NotNull(firstCommand);
                    Assert.NotNull(secondCommand);

                    Assert.True(nobodyIsExecuting.Value);

                    await TestEachCommandIsMutuallyExclusiveAsync(
                        [
                            (
                                firstCommand,
                                firstCancellationToken),
                            (
                                secondCommand,
                                secondCancellationToken)
                        ],
                        testScheduler);
                }
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullException"/> that throws an <see cref="ArgumentNullException"/> when a parameter is null.
            /// </summary>
            public sealed class CreateT6MethodThrowsArgumentNullTestSource : ArgumentNullExceptionTheoryData<
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="CreateT6MethodThrowsArgumentNullTestSource"/> class.
                /// </summary>
                public CreateT6MethodThrowsArgumentNullTestSource()
                    : base(
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "first",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)),
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "second",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)))
                {
                }
            }
        }

        /// <summary>
        /// Unit tests for <see cref="MutuallyExclusiveReactiveCommandsFactory.Create{T1Param, T1CommandResult, T1ExecuteResult, T2Param, T2CommandResult, T2ExecuteResult, T3Param, T3CommandResult, T3ExecuteResult}(ReactiveCommandFactoryArgument{T1Param, T1CommandResult, T1ExecuteResult}, ReactiveCommandFactoryArgument{T2Param, T2CommandResult, T2ExecuteResult}, ReactiveCommandFactoryArgument{T3Param, T3CommandResult, T3ExecuteResult})"/>.
        /// </summary>
        public sealed class CreateT9Method : NetTestRegimentation.ITestMethodWithNullableParameters<
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(CreateT9MethodThrowsArgumentNullTestSource))]
            public void ThrowsArgumentNullException(
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg1,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg2,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg3,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => MutuallyExclusiveReactiveCommandsFactory.Create(
                        arg1!,
                        arg2!,
                        arg3!));
            }

            /// <summary>
            /// Test that the method creates mutually exclusive commands.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
            [Fact]
            public async Task ProducesMutuallyExclusiveCommands()
            {
                var testScheduler = new TestScheduler();
                using (SchedulerExtensions.WithScheduler(testScheduler))
                using (var firstCancellationToken = new CancellationTokenSource())
                using (var secondCancellationToken = new CancellationTokenSource())
                using (var thirdCancellationToken = new CancellationTokenSource())
                {
                    RxSchedulers.MainThreadScheduler = testScheduler;
                    RxSchedulers.TaskpoolScheduler = testScheduler;

                    var first = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, firstCancellationToken.Token));
                    var second = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, secondCancellationToken.Token));
                    var third = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, thirdCancellationToken.Token));

                    (BehaviorSubject<bool> nobodyIsExecuting,
                        IDisposable exclusiveLock,
                        ReactiveCommand<Unit, Unit> firstCommand,
                        ReactiveCommand<Unit, Unit> secondCommand,
                        ReactiveCommand<Unit, Unit> thirdCommand) = MutuallyExclusiveReactiveCommandsFactory.Create(
                        first,
                        second,
                        third);

                    Assert.NotNull(nobodyIsExecuting);
                    Assert.NotNull(exclusiveLock);
                    Assert.NotNull(firstCommand);
                    Assert.NotNull(secondCommand);
                    Assert.NotNull(thirdCommand);

                    Assert.True(nobodyIsExecuting.Value);

                    await TestEachCommandIsMutuallyExclusiveAsync(
                    [
                        (
                            firstCommand,
                            firstCancellationToken),
                        (
                            secondCommand,
                            secondCancellationToken),
                        (
                            thirdCommand,
                            thirdCancellationToken)
                    ],
                    testScheduler);
                }
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullException"/> that throws an <see cref="ArgumentNullException"/> when a parameter is null.
            /// </summary>
            public sealed class CreateT9MethodThrowsArgumentNullTestSource : ArgumentNullExceptionTheoryData<
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="CreateT9MethodThrowsArgumentNullTestSource"/> class.
                /// </summary>
                public CreateT9MethodThrowsArgumentNullTestSource()
                    : base(
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "first",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)),
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "second",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)),
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "third",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)))
                {
                }
            }
        }

        /// <summary>
        /// Unit tests for <see cref="MutuallyExclusiveReactiveCommandsFactory.Create{T1Param, T1CommandResult, T1ExecuteResult, T2Param, T2CommandResult, T2ExecuteResult, T3Param, T3CommandResult, T3ExecuteResult, T4Param, T4CommandResult, T4ExecuteResult}(ReactiveCommandFactoryArgument{T1Param, T1CommandResult, T1ExecuteResult}, ReactiveCommandFactoryArgument{T2Param, T2CommandResult, T2ExecuteResult}, ReactiveCommandFactoryArgument{T3Param, T3CommandResult, T3ExecuteResult}, ReactiveCommandFactoryArgument{T4Param, T4CommandResult, T4ExecuteResult})"/>.
        /// </summary>
        public sealed class CreateT12Method : NetTestRegimentation.ITestMethodWithNullableParameters<
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(CreateT12MethodThrowsArgumentNullTestSource))]
            public void ThrowsArgumentNullException(
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg1,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg2,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg3,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg4,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => MutuallyExclusiveReactiveCommandsFactory.Create(
                        arg1!,
                        arg2!,
                        arg3!,
                        arg4!));
            }

            /// <summary>
            /// Test that the method creates mutually exclusive commands.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
            [Fact]
            public async Task ProducesMutuallyExclusiveCommands()
            {
                var testScheduler = new TestScheduler();
                using (SchedulerExtensions.WithScheduler(testScheduler))
                using (var firstCancellationToken = new CancellationTokenSource())
                using (var secondCancellationToken = new CancellationTokenSource())
                using (var thirdCancellationToken = new CancellationTokenSource())
                using (var fourthCancellationToken = new CancellationTokenSource())
                {
                    RxSchedulers.MainThreadScheduler = testScheduler;
                    RxSchedulers.TaskpoolScheduler = testScheduler;

                    var first = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, firstCancellationToken.Token));
                    var second = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, secondCancellationToken.Token));
                    var third = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, thirdCancellationToken.Token));
                    var fourth = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, fourthCancellationToken.Token));

                    (BehaviorSubject<bool> nobodyIsExecuting,
                        IDisposable exclusiveLock,
                        ReactiveCommand<Unit, Unit> firstCommand,
                        ReactiveCommand<Unit, Unit> secondCommand,
                        ReactiveCommand<Unit, Unit> thirdCommand,
                        ReactiveCommand<Unit, Unit> fourthCommand) = MutuallyExclusiveReactiveCommandsFactory.Create(
                        first,
                        second,
                        third,
                        fourth);

                    Assert.NotNull(nobodyIsExecuting);
                    Assert.NotNull(exclusiveLock);
                    Assert.NotNull(firstCommand);
                    Assert.NotNull(secondCommand);
                    Assert.NotNull(thirdCommand);
                    Assert.NotNull(fourthCommand);

                    Assert.True(nobodyIsExecuting.Value);

                    await TestEachCommandIsMutuallyExclusiveAsync(
                    [
                        (
                            firstCommand,
                            firstCancellationToken),
                        (
                            secondCommand,
                            secondCancellationToken),
                        (
                            thirdCommand,
                            thirdCancellationToken),
                        (
                            fourthCommand,
                            fourthCancellationToken)
                    ],
                    testScheduler);
                }
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullException"/> that throws an <see cref="ArgumentNullException"/> when a parameter is null.
            /// </summary>
            public sealed class CreateT12MethodThrowsArgumentNullTestSource : ArgumentNullExceptionTheoryData<
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="CreateT12MethodThrowsArgumentNullTestSource"/> class.
                /// </summary>
                public CreateT12MethodThrowsArgumentNullTestSource()
                    : base(
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "first",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)),
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "second",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)),
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "third",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)),
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "fourth",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)))
                {
                }
            }
        }

        /// <summary>
        /// Unit tests for <see cref="MutuallyExclusiveReactiveCommandsFactory.Create{T1Param, T1CommandResult, T1ExecuteResult, T2Param, T2CommandResult, T2ExecuteResult, T3Param, T3CommandResult, T3ExecuteResult, T4Param, T4CommandResult, T4ExecuteResult, T5Param, T5CommandResult, T5ExecuteResult}(ReactiveCommandFactoryArgument{T1Param, T1CommandResult, T1ExecuteResult}, ReactiveCommandFactoryArgument{T2Param, T2CommandResult, T2ExecuteResult}, ReactiveCommandFactoryArgument{T3Param, T3CommandResult, T3ExecuteResult}, ReactiveCommandFactoryArgument{T4Param, T4CommandResult, T4ExecuteResult}, ReactiveCommandFactoryArgument{T5Param, T5CommandResult, T5ExecuteResult})"/>.
        /// </summary>
        public sealed class CreateT15Method : NetTestRegimentation.ITestMethodWithNullableParameters<
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(CreateT15MethodThrowsArgumentNullTestSource))]
            public void ThrowsArgumentNullException(
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg1,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg2,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg3,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg4,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg5,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => MutuallyExclusiveReactiveCommandsFactory.Create(
                        arg1!,
                        arg2!,
                        arg3!,
                        arg4!,
                        arg5!));
            }

            /// <summary>
            /// Test that the method creates mutually exclusive commands.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
            [Fact]
            public async Task ProducesMutuallyExclusiveCommands()
            {
                var testScheduler = new TestScheduler();
                using (SchedulerExtensions.WithScheduler(testScheduler))
                using (var firstCancellationToken = new CancellationTokenSource())
                using (var secondCancellationToken = new CancellationTokenSource())
                using (var thirdCancellationToken = new CancellationTokenSource())
                using (var fourthCancellationToken = new CancellationTokenSource())
                using (var fifthCancellationToken = new CancellationTokenSource())
                {
                    RxSchedulers.MainThreadScheduler = testScheduler;
                    RxSchedulers.TaskpoolScheduler = testScheduler;

                    var first = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, firstCancellationToken.Token));
                    var second = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, secondCancellationToken.Token));
                    var third = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, thirdCancellationToken.Token));
                    var fourth = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, fourthCancellationToken.Token));
                    var fifth = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, fifthCancellationToken.Token));

                    (BehaviorSubject<bool> nobodyIsExecuting,
                        IDisposable exclusiveLock,
                        ReactiveCommand<Unit, Unit> firstCommand,
                        ReactiveCommand<Unit, Unit> secondCommand,
                        ReactiveCommand<Unit, Unit> thirdCommand,
                        ReactiveCommand<Unit, Unit> fourthCommand,
                        ReactiveCommand<Unit, Unit> fifthCommand) = MutuallyExclusiveReactiveCommandsFactory.Create(
                        first,
                        second,
                        third,
                        fourth,
                        fifth);

                    Assert.NotNull(nobodyIsExecuting);
                    Assert.NotNull(exclusiveLock);
                    Assert.NotNull(firstCommand);
                    Assert.NotNull(secondCommand);
                    Assert.NotNull(thirdCommand);
                    Assert.NotNull(fourthCommand);
                    Assert.NotNull(fifthCommand);

                    Assert.True(nobodyIsExecuting.Value);

                    await TestEachCommandIsMutuallyExclusiveAsync(
                    [
                        (
                            firstCommand,
                            firstCancellationToken),
                        (
                            secondCommand,
                            secondCancellationToken),
                        (
                            thirdCommand,
                            thirdCancellationToken),
                        (
                            fourthCommand,
                            fourthCancellationToken),
                        (
                            fifthCommand,
                            fifthCancellationToken)
                    ],
                    testScheduler);
                }
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullException"/> that throws an <see cref="ArgumentNullException"/> when a parameter is null.
            /// </summary>
            public sealed class CreateT15MethodThrowsArgumentNullTestSource : ArgumentNullExceptionTheoryData<
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="CreateT15MethodThrowsArgumentNullTestSource"/> class.
                /// </summary>
                public CreateT15MethodThrowsArgumentNullTestSource()
                    : base(
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "first",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)),
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "second",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)),
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "third",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)),
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "fourth",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)),
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "fifth",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)))
                {
                }
            }
        }

        /// <summary>
        /// Unit tests for <see cref="MutuallyExclusiveReactiveCommandsFactory.Create{T1Param, T1CommandResult, T1ExecuteResult, T2Param, T2CommandResult, T2ExecuteResult, T3Param, T3CommandResult, T3ExecuteResult, T4Param, T4CommandResult, T4ExecuteResult, T5Param, T5CommandResult, T5ExecuteResult, T6Param, T6CommandResult, T6ExecuteResult}(ReactiveCommandFactoryArgument{T1Param, T1CommandResult, T1ExecuteResult}, ReactiveCommandFactoryArgument{T2Param, T2CommandResult, T2ExecuteResult}, ReactiveCommandFactoryArgument{T3Param, T3CommandResult, T3ExecuteResult}, ReactiveCommandFactoryArgument{T4Param, T4CommandResult, T4ExecuteResult}, ReactiveCommandFactoryArgument{T5Param, T5CommandResult, T5ExecuteResult}, ReactiveCommandFactoryArgument{T6Param, T6CommandResult, T6ExecuteResult})"/>.
        /// </summary>
        public sealed class CreateT18Method : NetTestRegimentation.ITestMethodWithNullableParameters<
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(CreateT18MethodThrowsArgumentNullTestSource))]
            public void ThrowsArgumentNullException(
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg1,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg2,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg3,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg4,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg5,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg6,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => MutuallyExclusiveReactiveCommandsFactory.Create(
                        arg1!,
                        arg2!,
                        arg3!,
                        arg4!,
                        arg5!,
                        arg6!));
            }

            /// <summary>
            /// Test that the method creates mutually exclusive commands.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
            [Fact]
            public async Task ProducesMutuallyExclusiveCommands()
            {
                var testScheduler = new TestScheduler();
                using (SchedulerExtensions.WithScheduler(testScheduler))
                using (var firstCancellationToken = new CancellationTokenSource())
                using (var secondCancellationToken = new CancellationTokenSource())
                using (var thirdCancellationToken = new CancellationTokenSource())
                using (var fourthCancellationToken = new CancellationTokenSource())
                using (var fifthCancellationToken = new CancellationTokenSource())
                using (var sixthCancellationToken = new CancellationTokenSource())
                {
                    RxSchedulers.MainThreadScheduler = testScheduler;
                    RxSchedulers.TaskpoolScheduler = testScheduler;

                    var first = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, firstCancellationToken.Token));
                    var second = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, secondCancellationToken.Token));
                    var third = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, thirdCancellationToken.Token));
                    var fourth = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, fourthCancellationToken.Token));
                    var fifth = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, fifthCancellationToken.Token));
                    var sixth = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, sixthCancellationToken.Token));

                    (BehaviorSubject<bool> nobodyIsExecuting,
                        IDisposable exclusiveLock,
                        ReactiveCommand<Unit, Unit> firstCommand,
                        ReactiveCommand<Unit, Unit> secondCommand,
                        ReactiveCommand<Unit, Unit> thirdCommand,
                        ReactiveCommand<Unit, Unit> fourthCommand,
                        ReactiveCommand<Unit, Unit> fifthCommand,
                        ReactiveCommand<Unit, Unit> sixthCommand) = MutuallyExclusiveReactiveCommandsFactory.Create(
                        first,
                        second,
                        third,
                        fourth,
                        fifth,
                        sixth);

                    Assert.NotNull(nobodyIsExecuting);
                    Assert.NotNull(exclusiveLock);
                    Assert.NotNull(firstCommand);
                    Assert.NotNull(secondCommand);
                    Assert.NotNull(thirdCommand);
                    Assert.NotNull(fourthCommand);
                    Assert.NotNull(fifthCommand);
                    Assert.NotNull(sixthCommand);

                    Assert.True(nobodyIsExecuting.Value);

                    await TestEachCommandIsMutuallyExclusiveAsync(
                    [
                        (
                            firstCommand,
                            firstCancellationToken),
                        (
                            secondCommand,
                            secondCancellationToken),
                        (
                            thirdCommand,
                            thirdCancellationToken),
                        (
                            fourthCommand,
                            fourthCancellationToken),
                        (
                            fifthCommand,
                            fifthCancellationToken),
                        (
                            sixthCommand,
                            sixthCancellationToken)
                    ],
                    testScheduler);
                }
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullException"/> that throws an <see cref="ArgumentNullException"/> when a parameter is null.
            /// </summary>
            public sealed class CreateT18MethodThrowsArgumentNullTestSource : ArgumentNullExceptionTheoryData<
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="CreateT18MethodThrowsArgumentNullTestSource"/> class.
                /// </summary>
                public CreateT18MethodThrowsArgumentNullTestSource()
                    : base(
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "first",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)),
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "second",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)),
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "third",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)),
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "fourth",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)),
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "fifth",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)),
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "sixth",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)))
                {
                }
            }
        }

        /// <summary>
        /// Unit tests for <see cref="MutuallyExclusiveReactiveCommandsFactory.Create{T1Param, T1CommandResult, T1ExecuteResult, T2Param, T2CommandResult, T2ExecuteResult, T3Param, T3CommandResult, T3ExecuteResult, T4Param, T4CommandResult, T4ExecuteResult, T5Param, T5CommandResult, T5ExecuteResult, T6Param, T6CommandResult, T6ExecuteResult, T7Param, T7CommandResult, T7ExecuteResult}(ReactiveCommandFactoryArgument{T1Param, T1CommandResult, T1ExecuteResult}, ReactiveCommandFactoryArgument{T2Param, T2CommandResult, T2ExecuteResult}, ReactiveCommandFactoryArgument{T3Param, T3CommandResult, T3ExecuteResult}, ReactiveCommandFactoryArgument{T4Param, T4CommandResult, T4ExecuteResult}, ReactiveCommandFactoryArgument{T5Param, T5CommandResult, T5ExecuteResult}, ReactiveCommandFactoryArgument{T6Param, T6CommandResult, T6ExecuteResult}, ReactiveCommandFactoryArgument{T7Param, T7CommandResult, T7ExecuteResult})"/>.
        /// </summary>
        public sealed class CreateT21Method : NetTestRegimentation.ITestMethodWithNullableParameters<
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(CreateT21MethodThrowsArgumentNullTestSource))]
            public void ThrowsArgumentNullException(
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg1,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg2,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg3,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg4,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg5,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg6,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg7,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => MutuallyExclusiveReactiveCommandsFactory.Create(
                        arg1!,
                        arg2!,
                        arg3!,
                        arg4!,
                        arg5!,
                        arg6!,
                        arg7!));
            }

            /// <summary>
            /// Test that the method creates mutually exclusive commands.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
            [Fact]
            public async Task ProducesMutuallyExclusiveCommands()
            {
                var testScheduler = new TestScheduler();
                using (SchedulerExtensions.WithScheduler(testScheduler))
                using (var firstCancellationToken = new CancellationTokenSource())
                using (var secondCancellationToken = new CancellationTokenSource())
                using (var thirdCancellationToken = new CancellationTokenSource())
                using (var fourthCancellationToken = new CancellationTokenSource())
                using (var fifthCancellationToken = new CancellationTokenSource())
                using (var sixthCancellationToken = new CancellationTokenSource())
                using (var seventhCancellationToken = new CancellationTokenSource())
                {
                    RxSchedulers.MainThreadScheduler = testScheduler;
                    RxSchedulers.TaskpoolScheduler = testScheduler;

                    var first = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, firstCancellationToken.Token));
                    var second = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, secondCancellationToken.Token));
                    var third = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, thirdCancellationToken.Token));
                    var fourth = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, fourthCancellationToken.Token));
                    var fifth = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, fifthCancellationToken.Token));
                    var sixth = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, sixthCancellationToken.Token));
                    var seventh = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, seventhCancellationToken.Token));

                    (BehaviorSubject<bool> nobodyIsExecuting,
                        IDisposable exclusiveLock,
                        ReactiveCommand<Unit, Unit> firstCommand,
                        ReactiveCommand<Unit, Unit> secondCommand,
                        ReactiveCommand<Unit, Unit> thirdCommand,
                        ReactiveCommand<Unit, Unit> fourthCommand,
                        ReactiveCommand<Unit, Unit> fifthCommand,
                        ReactiveCommand<Unit, Unit> sixthCommand,
                        ReactiveCommand<Unit, Unit> seventhCommand) = MutuallyExclusiveReactiveCommandsFactory.Create(
                        first,
                        second,
                        third,
                        fourth,
                        fifth,
                        sixth,
                        seventh);

                    Assert.NotNull(nobodyIsExecuting);
                    Assert.NotNull(exclusiveLock);
                    Assert.NotNull(firstCommand);
                    Assert.NotNull(secondCommand);
                    Assert.NotNull(thirdCommand);
                    Assert.NotNull(fourthCommand);
                    Assert.NotNull(fifthCommand);
                    Assert.NotNull(sixthCommand);
                    Assert.NotNull(seventhCommand);

                    Assert.True(nobodyIsExecuting.Value);

                    await TestEachCommandIsMutuallyExclusiveAsync(
                    [
                        (
                            firstCommand,
                            firstCancellationToken),
                        (
                            secondCommand,
                            secondCancellationToken),
                        (
                            thirdCommand,
                            thirdCancellationToken),
                        (
                            fourthCommand,
                            fourthCancellationToken),
                        (
                            fifthCommand,
                            fifthCancellationToken),
                        (
                            sixthCommand,
                            sixthCancellationToken),
                        (
                            seventhCommand,
                            seventhCancellationToken)
                    ],
                    testScheduler);
                }
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullException"/> that throws an <see cref="ArgumentNullException"/> when a parameter is null.
            /// </summary>
            public sealed class CreateT21MethodThrowsArgumentNullTestSource : ArgumentNullExceptionTheoryData<
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="CreateT21MethodThrowsArgumentNullTestSource"/> class.
                /// </summary>
                public CreateT21MethodThrowsArgumentNullTestSource()
                    : base(
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "first",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)),
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "second",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)),
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "third",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)),
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "fourth",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)),
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "fifth",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)),
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "sixth",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)),
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "seventh",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)))
                {
                }
            }
        }

        /// <summary>
        /// Unit tests for <see cref="MutuallyExclusiveReactiveCommandsFactory.Create{T1Param, T1CommandResult, T1ExecuteResult, T2Param, T2CommandResult, T2ExecuteResult, T3Param, T3CommandResult, T3ExecuteResult, T4Param, T4CommandResult, T4ExecuteResult, T5Param, T5CommandResult, T5ExecuteResult, T6Param, T6CommandResult, T6ExecuteResult, T7Param, T7CommandResult, T7ExecuteResult, T8Param, T8CommandResult, T8ExecuteResult}(ReactiveCommandFactoryArgument{T1Param, T1CommandResult, T1ExecuteResult}, ReactiveCommandFactoryArgument{T2Param, T2CommandResult, T2ExecuteResult}, ReactiveCommandFactoryArgument{T3Param, T3CommandResult, T3ExecuteResult}, ReactiveCommandFactoryArgument{T4Param, T4CommandResult, T4ExecuteResult}, ReactiveCommandFactoryArgument{T5Param, T5CommandResult, T5ExecuteResult}, ReactiveCommandFactoryArgument{T6Param, T6CommandResult, T6ExecuteResult}, ReactiveCommandFactoryArgument{T7Param, T7CommandResult, T7ExecuteResult}, ReactiveCommandFactoryArgument{T8Param, T8CommandResult, T8ExecuteResult})"/>.
        /// </summary>
        public sealed class CreateT24Method : NetTestRegimentation.ITestMethodWithNullableParameters<
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(CreateT24MethodThrowsArgumentNullTestSource))]
            public void ThrowsArgumentNullException(
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg1,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg2,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg3,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg4,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg5,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg6,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg7,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>? arg8,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => MutuallyExclusiveReactiveCommandsFactory.Create(
                        arg1!,
                        arg2!,
                        arg3!,
                        arg4!,
                        arg5!,
                        arg6!,
                        arg7!,
                        arg8!));
            }

            /// <summary>
            /// Test that the method creates mutually exclusive commands.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
            [Fact]
            public async Task ProducesMutuallyExclusiveCommands()
            {
                var testScheduler = new TestScheduler();
                using (SchedulerExtensions.WithScheduler(testScheduler))
                using (var firstCancellationToken = new CancellationTokenSource())
                using (var secondCancellationToken = new CancellationTokenSource())
                using (var thirdCancellationToken = new CancellationTokenSource())
                using (var fourthCancellationToken = new CancellationTokenSource())
                using (var fifthCancellationToken = new CancellationTokenSource())
                using (var sixthCancellationToken = new CancellationTokenSource())
                using (var seventhCancellationToken = new CancellationTokenSource())
                using (var eighthCancellationToken = new CancellationTokenSource())
                {
                    RxSchedulers.MainThreadScheduler = testScheduler;
                    RxSchedulers.TaskpoolScheduler = testScheduler;

                    var first = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, firstCancellationToken.Token));
                    var second = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, secondCancellationToken.Token));
                    var third = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, thirdCancellationToken.Token));
                    var fourth = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, fourthCancellationToken.Token));
                    var fifth = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, fifthCancellationToken.Token));
                    var sixth = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, sixthCancellationToken.Token));
                    var seventh = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, seventhCancellationToken.Token));
                    var eighth = ReactiveCommandFactoryArgument<Unit, Unit, Unit>.CreateFromTask(param => RunUntilToldToStopAsync(param, eighthCancellationToken.Token));

                    (BehaviorSubject<bool> nobodyIsExecuting,
                        IDisposable exclusiveLock,
                        ReactiveCommand<Unit, Unit> firstCommand,
                        ReactiveCommand<Unit, Unit> secondCommand,
                        ReactiveCommand<Unit, Unit> thirdCommand,
                        ReactiveCommand<Unit, Unit> fourthCommand,
                        ReactiveCommand<Unit, Unit> fifthCommand,
                        ReactiveCommand<Unit, Unit> sixthCommand,
                        ReactiveCommand<Unit, Unit> seventhCommand,
                        ReactiveCommand<Unit, Unit> eighthCommand) = MutuallyExclusiveReactiveCommandsFactory.Create(
                        first,
                        second,
                        third,
                        fourth,
                        fifth,
                        sixth,
                        seventh,
                        eighth);

                    Assert.NotNull(nobodyIsExecuting);
                    Assert.NotNull(exclusiveLock);
                    Assert.NotNull(firstCommand);
                    Assert.NotNull(secondCommand);
                    Assert.NotNull(thirdCommand);
                    Assert.NotNull(fourthCommand);
                    Assert.NotNull(fifthCommand);
                    Assert.NotNull(sixthCommand);
                    Assert.NotNull(seventhCommand);
                    Assert.NotNull(eighthCommand);

                    Assert.True(nobodyIsExecuting.Value);

                    await TestEachCommandIsMutuallyExclusiveAsync(
                    [
                        (
                            firstCommand,
                            firstCancellationToken),
                        (
                            secondCommand,
                            secondCancellationToken),
                        (
                            thirdCommand,
                            thirdCancellationToken),
                        (
                            fourthCommand,
                            fourthCancellationToken),
                        (
                            fifthCommand,
                            fifthCancellationToken),
                        (
                            sixthCommand,
                            sixthCancellationToken),
                        (
                            seventhCommand,
                            seventhCancellationToken),
                        (
                            eighthCommand,
                            eighthCancellationToken)
                    ],
                    testScheduler);
                }
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullException"/> that throws an <see cref="ArgumentNullException"/> when a parameter is null.
            /// </summary>
            public sealed class CreateT24MethodThrowsArgumentNullTestSource : ArgumentNullExceptionTheoryData<
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit>>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="CreateT24MethodThrowsArgumentNullTestSource"/> class.
                /// </summary>
                public CreateT24MethodThrowsArgumentNullTestSource()
                    : base(
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "first",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)),
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "second",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)),
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "third",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)),
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "fourth",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)),
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "fifth",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)),
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "sixth",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)),
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "seventh",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)),
                        new NamedParameterInput<ReactiveCommandFactoryArgument<Unit, Unit, Unit>>(
                            "eighth",
                            () => ReactiveCommandFactoryArgument<Unit, Unit, Unit>.Create(param => param)))
                {
                }
            }
        }
    }
}
