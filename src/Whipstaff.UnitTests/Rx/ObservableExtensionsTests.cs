// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Primitives;
using ReactiveUI.Primitives.Concurrency;
using Splat.ApplicationPerformanceMonitoring;
using Whipstaff.Rx.Observables;
using Whipstaff.Testing.Splat.ApplicationPerformanceMonitoring;
using Xunit;

namespace Whipstaff.UnitTests.Rx
{
    /// <summary>
    /// RxVoid Tests for the Observable Extensions.
    /// </summary>
    public static partial class ObservableExtensionsTests
    {
        /// <summary>
        /// RxVoid Test for the Subscribe With Feature Usage Tracking Method that takes a Next action.
        /// </summary>
        public sealed class SubscribeWithFeatureUsageTrackingMethodWithNext
        {
            /// <summary>
            /// Test to ensure the downstream subscription correctly fires off the next action.
            /// </summary>
            [Fact]
            public void SubjectFiresOffNextSubscription()
            {
                var nextCount = 0;
                var featureUsageTrackingManager = new FuncFeatureUsageTrackingManager(featureName => new FakeFeatureUsageTrackingSession(featureName));
                var subFeatureName = "FeatureTwo";

                using (var observable = new Subject<int>())
                using (var subscription = observable.SubscribeWithFeatureUsageTracking(
                    _ => nextCount++,
                    featureUsageTrackingManager,
                    subFeatureName))
                {
                    Assert.Equal(0, nextCount);

                    observable.OnNext(1);

                    Assert.Equal(1, nextCount);
                }
            }

            /// <summary>
            /// Test to ensure the downstream subscription correctly fires off the next action.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous RxVoid test.</returns>
            [Fact]
            public async Task ReactiveCommandFiresOffNextSubscriptionAsync()
            {
                var testScheduler = Sequencer.Immediate;
                var nextCount = 0;
                var featureUsageTrackingManager = new FuncFeatureUsageTrackingManager(featureName => new FakeFeatureUsageTrackingSession(featureName));
                var subFeatureName = "FeatureTwo";

                using (var observable = ReactiveCommand.CreateFromTask<RxVoid, RxVoid>(
                           static rxVoid => Task.FromResult(rxVoid),
                           outputScheduler: testScheduler))
                using (var subscription = observable.SubscribeWithFeatureUsageTracking(
                           _ => nextCount++,
                           featureUsageTrackingManager,
                           subFeatureName))
                {
                    Assert.Equal(0, nextCount);

                    _ = await observable.Execute(RxVoid.Default);

                    Assert.Equal(1, nextCount);
                }
            }
        }

        /// <summary>
        /// RxVoid Test for the Subscribe With Feature Usage Tracking Method that takes Next and Error actions.
        /// </summary>
        public sealed class SubscribeWithFeatureUsageTrackingMethodWithNextAndCompleted
        {
            /// <summary>
            /// Test to ensure the downstream subscription correctly fires off the next action.
            /// </summary>
            [Fact]
            public void SubjectFiresOffNextSubscription()
            {
                var nextCount = 0;
                var completedCount = 0;
                var featureUsageTrackingManager = new FuncFeatureUsageTrackingManager(featureName => new FakeFeatureUsageTrackingSession(featureName));
                var subFeatureName = "FeatureTwo";

                using (var observable = new Subject<int>())
                using (var subscription = observable.SubscribeWithFeatureUsageTracking(
                    _ => nextCount++,
                    () => completedCount++,
                    featureUsageTrackingManager,
                    subFeatureName))
                {
                    Assert.Equal(0, nextCount);
                    Assert.Equal(0, completedCount);

                    observable.OnNext(1);

                    Assert.Equal(1, nextCount);
                    Assert.Equal(0, completedCount);
                }
            }

            /// <summary>
            /// Test to ensure the downstream subscription correctly fires off the next action.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous RxVoid test.</returns>
            [Fact]
            public async Task ReactiveCommandFiresOffNextSubscriptionAsync()
            {
                var testScheduler = Sequencer.Immediate;
                var nextCount = 0;
                var completedCount = 0;
                var featureUsageTrackingManager = new FuncFeatureUsageTrackingManager(featureName =>
                    new FakeFeatureUsageTrackingSession(featureName));
                var subFeatureName = "FeatureTwo";

                using (var observable = ReactiveCommand.CreateFromTask<RxVoid, RxVoid>(
                           rxVoid => Task.FromResult(rxVoid),
                           outputScheduler: testScheduler))
                using (var subscription = observable.SubscribeWithFeatureUsageTracking(
                           _ => nextCount++,
                           () => completedCount++,
                           featureUsageTrackingManager,
                           subFeatureName))
                {
                    Assert.Equal(0, nextCount);
                    Assert.Equal(0, completedCount);

                    _ = await observable.Execute(RxVoid.Default);

                    Assert.Equal(1, nextCount);
                    Assert.Equal(0, completedCount);
                }
            }
        }

        /// <summary>
        /// RxVoid Test for the Subscribe With Feature Usage Tracking Method that takes Next and Error actions.
        /// </summary>
        public sealed class SubscribeWithFeatureUsageTrackingMethodWithNextAndError
        {
            /// <summary>
            /// Test to ensure the downstream subscription correctly fires off the next action.
            /// </summary>
            [Fact]
            public void SubjectFiresOffNextSubscription()
            {
                var nextCount = 0;
                var errorCount = 0;
                using (var featureUsageTrackingSession = new FakeFeatureUsageTrackingSession("FeatureOne"))
                {
                    var subFeatureName = "FeatureTwo";

                    using (var observable = new Subject<int>())
                    using (var subscription = observable.SubscribeWithSubFeatureUsageTracking(
                               _ => nextCount++,
                               _ => errorCount++,
                               featureUsageTrackingSession,
                               subFeatureName))
                    {
                        Assert.Equal(0, nextCount);
                        Assert.Equal(0, errorCount);

                        observable.OnNext(1);

                        Assert.Equal(1, nextCount);
                        Assert.Equal(0, errorCount);
                    }
                }
            }

            /// <summary>
            /// Test to ensure the downstream subscription correctly fires off the next action.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous RxVoid test.</returns>
            [Fact]
            public async Task ReactiveCommandFiresOffNextSubscriptionAsync()
            {
                var testScheduler = Sequencer.Immediate;
                var nextCount = 0;
                var errorCount = 0;
                var featureUsageTrackingManager = new FuncFeatureUsageTrackingManager(featureName =>
                    new FakeFeatureUsageTrackingSession(featureName));
                var subFeatureName = "FeatureTwo";

                using (var observable = ReactiveCommand.CreateFromTask<RxVoid, RxVoid>(
                           rxVoid => Task.FromResult(rxVoid),
                           outputScheduler: testScheduler))
                using (var subscription = observable.SubscribeWithFeatureUsageTracking(
                           _ => nextCount++,
                           _ => errorCount++,
                           featureUsageTrackingManager,
                           subFeatureName))
                {
                    Assert.Equal(0, nextCount);
                    Assert.Equal(0, errorCount);

                    _ = await observable.Execute(RxVoid.Default);

                    Assert.Equal(1, nextCount);
                    Assert.Equal(0, errorCount);
                }
            }
        }

        /// <summary>
        /// RxVoid Test for the Subscribe With Feature Usage Tracking Method that takes Next, Error and Completed actions.
        /// </summary>
        public sealed class SubscribeWithFeatureUsageTrackingMethodWithNextErrorAndCompleted
        {
            /// <summary>
            /// Test to ensure the downstream subscription correctly fires off the next action.
            /// </summary>
            [Fact]
            public void SubjectFiresOffNextSubscription()
            {
                var nextCount = 0;
                var errorCount = 0;
                var completedCount = 0;
                var featureUsageTrackingManager = new FuncFeatureUsageTrackingManager(featureName => new FakeFeatureUsageTrackingSession(featureName));
                var subFeatureName = "FeatureTwo";

                using (var observable = new Subject<int>())
                using (var subscription = observable.SubscribeWithFeatureUsageTracking(
                    _ => nextCount++,
                    _ => errorCount++,
                    () => completedCount++,
                    featureUsageTrackingManager,
                    subFeatureName))
                {
                    Assert.Equal(0, nextCount);
                    Assert.Equal(0, errorCount);
                    Assert.Equal(0, completedCount);

                    observable.OnNext(1);

                    Assert.Equal(1, nextCount);
                    Assert.Equal(0, errorCount);
                    Assert.Equal(0, completedCount);
                }
            }

            /// <summary>
            /// Test to ensure the downstream subscription correctly fires off the next action.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous RxVoid test.</returns>
            [Fact]
            public async Task ReactiveCommandFiresOffNextSubscriptionAsync()
            {
                var testScheduler = Sequencer.Immediate;
                var nextCount = 0;
                var errorCount = 0;
                var completedCount = 0;
                using (var featureUsageTrackingSession = new FakeFeatureUsageTrackingSession("FeatureOne"))
                {
                    var subFeatureName = "FeatureTwo";

                    using (var observable = ReactiveCommand.CreateFromTask<RxVoid, RxVoid>(
                               rxVoid => Task.FromResult(rxVoid),
                               outputScheduler: testScheduler))
                    using (var subscription = observable.SubscribeWithSubFeatureUsageTracking(
                               _ => nextCount++,
                               _ => errorCount++,
                               () => completedCount++,
                               featureUsageTrackingSession,
                               subFeatureName))
                    {
                        Assert.Equal(0, nextCount);
                        Assert.Equal(0, errorCount);
                        Assert.Equal(0, completedCount);

                        _ = await observable.Execute(RxVoid.Default);

                        Assert.Equal(1, nextCount);
                        Assert.Equal(0, errorCount);
                        Assert.Equal(0, completedCount);
                    }
                }
            }

#if TBC
            /// <summary>
            /// Test to ensure the downstream subscription correctly fires off the next action.
            /// </summary>
            [Fact]
            public void ReactiveCommandFiresOffNextAndErrorSubscription()
            {
                var commandCount = 0;
                var nextCount = 0;
                var errorCount = 0;
                var thrownExceptionCount = 0;
                var completedCount = 0;

                var featureUsageTrackingSession = new DefaultFeatureUsageTrackingSession("FeatureOne");
                var subFeatureName = "FeatureTwo";

                var observable = ReactiveCommand.CreateFromObservable<RxVoid, RxVoid>(_ =>
                {
                    commandCount++;
                    return Observable.Throw<RxVoid>(new ArgumentException("Test"));
                });

                var observable2 = ReactiveCommand.CreateFromTask<RxVoid, RxVoid>(_ =>
                {
                    commandCount++;
                    return Task.FromException<RxVoid>(new ArgumentException("Test"));
                });

                var observable3 = ReactiveCommand.CreateFromTask<RxVoid, RxVoid>(_ =>
                {
                    commandCount++;
                    throw new ArgumentException("Test");
                });

                using (var subscription = observable2.SubscribeWithSubFeatureUsageTracking(
                    _ =>
                    {
                        nextCount++;
                    },
                    _ => errorCount++,
                    () => completedCount++,
                    featureUsageTrackingSession,
                    subFeatureName))
                using (var thrownExceptions = observable2.ThrownExceptions.Subscribe(_ => thrownExceptionCount++))
                {
                    Assert.Equal(0, commandCount);
                    Assert.Equal(0, nextCount);
                    Assert.Equal(0, errorCount);
                    Assert.Equal(0, thrownExceptionCount);
                    Assert.Equal(0, completedCount);

                    _ = observable.Execute(RxVoid.Default).Subscribe(_ => { });

                    Assert.Equal(1, commandCount);
                    Assert.Equal(1, nextCount);
                    Assert.Equal(0, errorCount);
                    Assert.Equal(1, thrownExceptionCount);
                    Assert.Equal(0, completedCount);
                }
            }
#endif
        }

        /// <summary>
        /// RxVoid Test for the Subscribe With Feature Usage Tracking Method that takes a Next action.
        /// </summary>
        public sealed class SubscribeWithSubFeatureUsageTrackingMethodWithNext
        {
            /// <summary>
            /// Test to ensure the downstream subscription correctly fires off the next action.
            /// </summary>
            [Fact]
            public void SubjectFiresOffNextSubscription()
            {
                var nextCount = 0;
                using (var featureUsageTrackingSession = new FakeFeatureUsageTrackingSession("FeatureOne"))
                {
                    var subFeatureName = "FeatureTwo";

                    using (var observable = new Subject<int>())
                    using (var subscription = observable.SubscribeWithSubFeatureUsageTracking(
                               _ => nextCount++,
                               featureUsageTrackingSession,
                               subFeatureName))
                    {
                        Assert.Equal(0, nextCount);

                        observable.OnNext(1);

                        Assert.Equal(1, nextCount);
                    }
                }
            }

            /// <summary>
            /// Test to ensure the downstream subscription correctly fires off the next action.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous RxVoid test.</returns>
            [Fact]
            public async Task ReactiveCommandFiresOffNextSubscriptionAsync()
            {
                var testScheduler = Sequencer.Immediate;
                var nextCount = 0;
                using (var featureUsageTrackingSession = new FakeFeatureUsageTrackingSession("FeatureOne"))
                {
                    var subFeatureName = "FeatureTwo";

                    using (var observable = ReactiveCommand.CreateFromTask<RxVoid, RxVoid>(
                               rxVoid => Task.FromResult(rxVoid),
                               outputScheduler: testScheduler))
                    using (var subscription = observable.SubscribeWithSubFeatureUsageTracking(
                               _ => nextCount++,
                               featureUsageTrackingSession,
                               subFeatureName))
                    {
                        Assert.Equal(0, nextCount);

                        _ = await observable.Execute(RxVoid.Default);

                        Assert.Equal(1, nextCount);
                    }
                }
            }
        }

        /// <summary>
        /// RxVoid Test for the Subscribe With Feature Usage Tracking Method that takes Next and Error actions.
        /// </summary>
        public sealed class SubscribeWithSubFeatureUsageTrackingMethodWithNextAndCompleted
        {
            /// <summary>
            /// Test to ensure the downstream subscription correctly fires off the next action.
            /// </summary>
            [Fact]
            public void SubjectFiresOffNextSubscription()
            {
                var nextCount = 0;
                var completedCount = 0;
                using (var featureUsageTrackingSession = new FakeFeatureUsageTrackingSession("FeatureOne"))
                {
                    var subFeatureName = "FeatureTwo";

                    using (var observable = new Subject<int>())
                    using (var subscription = observable.SubscribeWithSubFeatureUsageTracking(
                               _ => nextCount++,
                               () => completedCount++,
                               featureUsageTrackingSession,
                               subFeatureName))
                    {
                        Assert.Equal(0, nextCount);
                        Assert.Equal(0, completedCount);

                        observable.OnNext(1);

                        Assert.Equal(1, nextCount);
                        Assert.Equal(0, completedCount);
                    }
                }
            }

            /// <summary>
            /// Test to ensure the downstream subscription correctly fires off the next action.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous RxVoid test.</returns>
            [Fact]
            public async Task ReactiveCommandFiresOffNextSubscriptionAsync()
            {
                var testScheduler = Sequencer.Immediate;
                var nextCount = 0;
                var completedCount = 0;

                using (var featureUsageTrackingSession = new FakeFeatureUsageTrackingSession("FeatureOne"))
                {
                    var subFeatureName = "FeatureTwo";

                    using (var observable = ReactiveCommand.CreateFromTask<RxVoid, RxVoid>(
                               rxVoid => Task.FromResult(rxVoid),
                               outputScheduler: testScheduler))
                    using (var subscription = observable.SubscribeWithSubFeatureUsageTracking(
                               _ => nextCount++,
                               () => completedCount++,
                               featureUsageTrackingSession,
                               subFeatureName))
                    {
                        Assert.Equal(0, nextCount);
                        Assert.Equal(0, completedCount);

                        _ = await observable.Execute(RxVoid.Default);

                        Assert.Equal(1, nextCount);
                        Assert.Equal(0, completedCount);
                    }
                }
            }
        }

        /// <summary>
        /// RxVoid Test for the Subscribe With Feature Usage Tracking Method that takes Next and Error actions.
        /// </summary>
        public sealed class SubscribeWithSubFeatureUsageTrackingMethodWithNextAndError
        {
            /// <summary>
            /// Test to ensure the downstream subscription correctly fires off the next action.
            /// </summary>
            [Fact]
            public void SubjectFiresOffNextSubscription()
            {
                var nextCount = 0;
                var errorCount = 0;
                using (var featureUsageTrackingSession = new FakeFeatureUsageTrackingSession("FeatureOne"))
                {
                    var subFeatureName = "FeatureTwo";

                    using (var observable = new Subject<int>())
                    using (var subscription = observable.SubscribeWithSubFeatureUsageTracking(
                               _ => nextCount++,
                               _ => errorCount++,
                               featureUsageTrackingSession,
                               subFeatureName))
                    {
                        Assert.Equal(0, nextCount);
                        Assert.Equal(0, errorCount);

                        observable.OnNext(1);

                        Assert.Equal(1, nextCount);
                        Assert.Equal(0, errorCount);
                    }
                }
            }

            /// <summary>
            /// Test to ensure the downstream subscription correctly fires off the next action.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous RxVoid test.</returns>
            [Fact]
            public async Task ReactiveCommandFiresOffNextSubscriptionAsync()
            {
                var testScheduler = Sequencer.Immediate;
                var nextCount = 0;
                var errorCount = 0;
                using (var featureUsageTrackingSession = new FakeFeatureUsageTrackingSession("FeatureOne"))
                {
                    var subFeatureName = "FeatureTwo";

                    using (var observable = ReactiveCommand.CreateFromTask<RxVoid, RxVoid>(
                               static rxVoid => Task.FromResult(rxVoid),
                               outputScheduler: testScheduler))
                    using (var subscription = observable.SubscribeWithSubFeatureUsageTracking(
                               _ => nextCount++,
                               _ => errorCount++,
                               featureUsageTrackingSession,
                               subFeatureName))
                    {
                        Assert.Equal(0, nextCount);
                        Assert.Equal(0, errorCount);

                        _ = await observable.Execute(RxVoid.Default);

                        Assert.Equal(1, nextCount);
                        Assert.Equal(0, errorCount);
                    }
                }
            }
        }

        /// <summary>
        /// RxVoid Test for the Subscribe With Feature Usage Tracking Method that takes Next, Error and Completed actions.
        /// </summary>
        public sealed class SubscribeWithSubFeatureUsageTrackingMethodWithNextErrorAndCompleted
        {
            /// <summary>
            /// Test to ensure the downstream subscription correctly fires off the next action.
            /// </summary>
            [Fact]
            public void SubjectFiresOffNextSubscription()
            {
                var nextCount = 0;
                var errorCount = 0;
                var completedCount = 0;
                using (var featureUsageTrackingSession = new FakeFeatureUsageTrackingSession("FeatureOne"))
                {
                    var subFeatureName = "FeatureTwo";

                    using (var observable = new Subject<int>())
                    using (var subscription = observable.SubscribeWithSubFeatureUsageTracking(
                               _ => nextCount++,
                               _ => errorCount++,
                               () => completedCount++,
                               featureUsageTrackingSession,
                               subFeatureName))
                    {
                        Assert.Equal(0, nextCount);
                        Assert.Equal(0, errorCount);
                        Assert.Equal(0, completedCount);

                        observable.OnNext(1);

                        Assert.Equal(1, nextCount);
                        Assert.Equal(0, errorCount);
                        Assert.Equal(0, completedCount);
                    }
                }
            }

            /// <summary>
            /// Test to ensure the downstream subscription correctly fires off the next action.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous RxVoid test.</returns>
            [Fact]
            public async Task ReactiveCommandFiresOffNextSubscriptionAsync()
            {
                var testScheduler = Sequencer.Immediate;
                var nextCount = 0;
                var errorCount = 0;
                var completedCount = 0;
                using (var featureUsageTrackingSession = new FakeFeatureUsageTrackingSession("FeatureOne"))
                {
                    var subFeatureName = "FeatureTwo";

                    using (var observable = ReactiveCommand.CreateFromTask<RxVoid, RxVoid>(
                               static rxVoid => Task.FromResult(rxVoid),
                               outputScheduler: testScheduler))
                    using (var subscription = observable.SubscribeWithSubFeatureUsageTracking(
                               _ => nextCount++,
                               _ => errorCount++,
                               () => completedCount++,
                               featureUsageTrackingSession,
                               subFeatureName))
                    {
                        Assert.Equal(0, nextCount);
                        Assert.Equal(0, errorCount);
                        Assert.Equal(0, completedCount);

                        _ = await observable.Execute(RxVoid.Default);

                        Assert.Equal(1, nextCount);
                        Assert.Equal(0, errorCount);
                        Assert.Equal(0, completedCount);
                    }
                }
            }

#if TBC
            /// <summary>
            /// Test to ensure the downstream subscription correctly fires off the next action.
            /// </summary>
            [Fact]
            public void ReactiveCommandFiresOffNextAndErrorSubscription()
            {
                var commandCount = 0;
                var nextCount = 0;
                var errorCount = 0;
                var thrownExceptionCount = 0;
                var completedCount = 0;

                var featureUsageTrackingSession = new DefaultFeatureUsageTrackingSession("FeatureOne");
                var subFeatureName = "FeatureTwo";

                var observable = ReactiveCommand.CreateFromObservable<RxVoid, RxVoid>(_ =>
                {
                    commandCount++;
                    return Observable.Throw<RxVoid>(new ArgumentException("Test"));
                });

                var observable2 = ReactiveCommand.CreateFromTask<RxVoid, RxVoid>(_ =>
                {
                    commandCount++;
                    return Task.FromException<RxVoid>(new ArgumentException("Test"));
                });

                var observable3 = ReactiveCommand.CreateFromTask<RxVoid, RxVoid>(_ =>
                {
                    commandCount++;
                    throw new ArgumentException("Test");
                });

                using (var subscription = observable2.SubscribeWithSubFeatureUsageTracking(
                    _ =>
                    {
                        nextCount++;
                    },
                    _ => errorCount++,
                    () => completedCount++,
                    featureUsageTrackingSession,
                    subFeatureName))
                using (var thrownExceptions = observable2.ThrownExceptions.Subscribe(_ => thrownExceptionCount++))
                {
                    Assert.Equal(0, commandCount);
                    Assert.Equal(0, nextCount);
                    Assert.Equal(0, errorCount);
                    Assert.Equal(0, thrownExceptionCount);
                    Assert.Equal(0, completedCount);

                    _ = observable.Execute(RxVoid.Default).Subscribe(_ => { });

                    Assert.Equal(1, commandCount);
                    Assert.Equal(1, nextCount);
                    Assert.Equal(0, errorCount);
                    Assert.Equal(1, thrownExceptionCount);
                    Assert.Equal(0, completedCount);
                }
            }
#endif
        }
    }
}
