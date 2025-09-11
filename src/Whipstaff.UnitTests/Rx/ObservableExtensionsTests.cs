// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using ReactiveUI;
using Splat.ApplicationPerformanceMonitoring;
using Whipstaff.Rx;
using Whipstaff.Testing.Splat.ApplicationPerformanceMonitoring;
using Xunit;

namespace Whipstaff.UnitTests.Rx
{
    /// <summary>
    /// Unit Tests for the Observable Extensions.
    /// </summary>
    public static partial class ObservableExtensionsTests
    {
        /// <summary>
        /// Unit Test for the Subscribe With Feature Usage Tracking Method that takes a Next action.
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
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReactiveCommandFiresOffNextSubscriptionAsync()
            {
                var nextCount = 0;
                var featureUsageTrackingManager = new FuncFeatureUsageTrackingManager(featureName => new FakeFeatureUsageTrackingSession(featureName));
                var subFeatureName = "FeatureTwo";

                using (var observable = ReactiveCommand.CreateFromTask<Unit, Unit>(unit => Task.FromResult(unit)))
                using (var subscription = observable.SubscribeWithFeatureUsageTracking(
                    _ => nextCount++,
                    featureUsageTrackingManager,
                    subFeatureName))
                {
                    Assert.Equal(0, nextCount);

                    _ = await observable.Execute(Unit.Default);

                    Assert.Equal(1, nextCount);
                }
            }
        }

        /// <summary>
        /// Unit Test for the Subscribe With Feature Usage Tracking Method that takes Next and Error actions.
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
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReactiveCommandFiresOffNextSubscriptionAsync()
            {
                var nextCount = 0;
                var completedCount = 0;
                var featureUsageTrackingManager = new FuncFeatureUsageTrackingManager(featureName => new FakeFeatureUsageTrackingSession(featureName));
                var subFeatureName = "FeatureTwo";

                using (var observable = ReactiveCommand.CreateFromTask<Unit, Unit>(unit => Task.FromResult(unit)))
                using (var subscription = observable.SubscribeWithFeatureUsageTracking(
                    _ => nextCount++,
                    () => completedCount++,
                    featureUsageTrackingManager,
                    subFeatureName))
                {
                    Assert.Equal(0, nextCount);
                    Assert.Equal(0, completedCount);

                    _ = await observable.Execute(Unit.Default);

                    Assert.Equal(1, nextCount);
                    Assert.Equal(0, completedCount);
                }
            }
        }

        /// <summary>
        /// Unit Test for the Subscribe With Feature Usage Tracking Method that takes Next and Error actions.
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
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReactiveCommandFiresOffNextSubscriptionAsync()
            {
                var nextCount = 0;
                var errorCount = 0;
                var featureUsageTrackingManager = new FuncFeatureUsageTrackingManager(featureName => new FakeFeatureUsageTrackingSession(featureName));
                var subFeatureName = "FeatureTwo";

                using (var observable = ReactiveCommand.CreateFromTask<Unit, Unit>(unit => Task.FromResult(unit)))
                using (var subscription = observable.SubscribeWithFeatureUsageTracking(
                    _ => nextCount++,
                    _ => errorCount++,
                    featureUsageTrackingManager,
                    subFeatureName))
                {
                    Assert.Equal(0, nextCount);
                    Assert.Equal(0, errorCount);

                    _ = await observable.Execute(Unit.Default);

                    Assert.Equal(1, nextCount);
                    Assert.Equal(0, errorCount);
                }
            }
        }

        /// <summary>
        /// Unit Test for the Subscribe With Feature Usage Tracking Method that takes Next, Error and Completed actions.
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
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReactiveCommandFiresOffNextSubscriptionAsync()
            {
                var nextCount = 0;
                var errorCount = 0;
                var completedCount = 0;
                using (var featureUsageTrackingSession = new FakeFeatureUsageTrackingSession("FeatureOne"))
                {
                    var subFeatureName = "FeatureTwo";

                    using (var observable = ReactiveCommand.CreateFromTask<Unit, Unit>(unit => Task.FromResult(unit)))
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

                        _ = await observable.Execute(Unit.Default);

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

                var observable = ReactiveCommand.CreateFromObservable<Unit, Unit>(_ =>
                {
                    commandCount++;
                    return Observable.Throw<Unit>(new ArgumentException("Test"));
                });

                var observable2 = ReactiveCommand.CreateFromTask<Unit, Unit>(_ =>
                {
                    commandCount++;
                    return Task.FromException<Unit>(new ArgumentException("Test"));
                });

                var observable3 = ReactiveCommand.CreateFromTask<Unit, Unit>(_ =>
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

                    _ = observable.Execute(Unit.Default).Subscribe(_ => { });

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
        /// Unit Test for the Subscribe With Feature Usage Tracking Method that takes a Next action.
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
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReactiveCommandFiresOffNextSubscriptionAsync()
            {
                var nextCount = 0;
                using (var featureUsageTrackingSession = new FakeFeatureUsageTrackingSession("FeatureOne"))
                {
                    var subFeatureName = "FeatureTwo";

                    using (var observable = ReactiveCommand.CreateFromTask<Unit, Unit>(unit => Task.FromResult(unit)))
                    using (var subscription = observable.SubscribeWithSubFeatureUsageTracking(
                               _ => nextCount++,
                               featureUsageTrackingSession,
                               subFeatureName))
                    {
                        Assert.Equal(0, nextCount);

                        _ = await observable.Execute(Unit.Default);

                        Assert.Equal(1, nextCount);
                    }
                }
            }
        }

        /// <summary>
        /// Unit Test for the Subscribe With Feature Usage Tracking Method that takes Next and Error actions.
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
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReactiveCommandFiresOffNextSubscriptionAsync()
            {
                var nextCount = 0;
                var completedCount = 0;

                using (var featureUsageTrackingSession = new FakeFeatureUsageTrackingSession("FeatureOne"))
                {
                    var subFeatureName = "FeatureTwo";

                    using (var observable = ReactiveCommand.CreateFromTask<Unit, Unit>(unit => Task.FromResult(unit)))
                    using (var subscription = observable.SubscribeWithSubFeatureUsageTracking(
                               _ => nextCount++,
                               () => completedCount++,
                               featureUsageTrackingSession,
                               subFeatureName))
                    {
                        Assert.Equal(0, nextCount);
                        Assert.Equal(0, completedCount);

                        _ = await observable.Execute(Unit.Default);

                        Assert.Equal(1, nextCount);
                        Assert.Equal(0, completedCount);
                    }
                }
            }
        }

        /// <summary>
        /// Unit Test for the Subscribe With Feature Usage Tracking Method that takes Next and Error actions.
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
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReactiveCommandFiresOffNextSubscriptionAsync()
            {
                var nextCount = 0;
                var errorCount = 0;
                using (var featureUsageTrackingSession = new FakeFeatureUsageTrackingSession("FeatureOne"))
                {
                    var subFeatureName = "FeatureTwo";

                    using (var observable = ReactiveCommand.CreateFromTask<Unit, Unit>(unit => Task.FromResult(unit)))
                    using (var subscription = observable.SubscribeWithSubFeatureUsageTracking(
                               _ => nextCount++,
                               _ => errorCount++,
                               featureUsageTrackingSession,
                               subFeatureName))
                    {
                        Assert.Equal(0, nextCount);
                        Assert.Equal(0, errorCount);

                        _ = await observable.Execute(Unit.Default);

                        Assert.Equal(1, nextCount);
                        Assert.Equal(0, errorCount);
                    }
                }
            }
        }

        /// <summary>
        /// Unit Test for the Subscribe With Feature Usage Tracking Method that takes Next, Error and Completed actions.
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
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReactiveCommandFiresOffNextSubscriptionAsync()
            {
                var nextCount = 0;
                var errorCount = 0;
                var completedCount = 0;
                using (var featureUsageTrackingSession = new FakeFeatureUsageTrackingSession("FeatureOne"))
                {
                    var subFeatureName = "FeatureTwo";

                    using (var observable = ReactiveCommand.CreateFromTask<Unit, Unit>(unit => Task.FromResult(unit)))
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

                        _ = await observable.Execute(Unit.Default);

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

                var observable = ReactiveCommand.CreateFromObservable<Unit, Unit>(_ =>
                {
                    commandCount++;
                    return Observable.Throw<Unit>(new ArgumentException("Test"));
                });

                var observable2 = ReactiveCommand.CreateFromTask<Unit, Unit>(_ =>
                {
                    commandCount++;
                    return Task.FromException<Unit>(new ArgumentException("Test"));
                });

                var observable3 = ReactiveCommand.CreateFromTask<Unit, Unit>(_ =>
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

                    _ = observable.Execute(Unit.Default).Subscribe(_ => { });

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
