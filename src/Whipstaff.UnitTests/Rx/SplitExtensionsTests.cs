// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using ReactiveUI;
using Whipstaff.Rx.Observables;
using Xunit;

namespace Whipstaff.UnitTests.Rx
{
    /// <summary>
    /// Extension methods for performing split on an observable.
    /// </summary>
    public static class SplitExtensionsTests
    {
        /// <summary>
        /// interface for Mass Test Object for splitting.
        /// </summary>
        public interface ISomeMassObjectViewModel : IReactiveObject
        {
            /// <summary>
            /// Gets or sets the first property.
            /// </summary>
            int One
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the second property.
            /// </summary>
            int Two { get; set; }

            /// <summary>
            /// Gets or sets the third property.
            /// </summary>
            int Three { get; set; }

            /// <summary>
            /// Gets or sets the fourth property.
            /// </summary>
            int Four { get; set; }

            /// <summary>
            /// Gets or sets the fifth property.
            /// </summary>
            int Five { get; set; }

            /// <summary>
            /// Gets or sets the sixth property.
            /// </summary>
            int Six { get; set; }

            /// <summary>
            /// Gets or sets the seventh property.
            /// </summary>
            int Seven { get; set; }

            /// <summary>
            /// Gets or sets the eight property.
            /// </summary>
            int Eight { get; set; }

            /// <summary>
            /// Gets or sets the ninth property.
            /// </summary>
            int Nine { get; set; }

            /// <summary>
            /// Gets or sets the tenth property.
            /// </summary>
            int Ten { get; set; }

            /// <summary>
            /// Gets or sets the eleventh property.
            /// </summary>
            int Eleven { get; set; }

            /// <summary>
            /// Gets or sets the twelfth property.
            /// </summary>
            int Twelve { get; set; }

            /// <summary>
            /// Gets or sets the thirteenth property.
            /// </summary>
            int Thirteen { get; set; }

            /// <summary>
            /// Gets or sets the fourteenth property.
            /// </summary>
            int Fourteen { get; set; }

            /// <summary>
            /// Gets or sets the fifteenth property.
            /// </summary>
            int Fifteen { get; set; }

            /// <summary>
            /// Gets or sets the sixteenth property.
            /// </summary>
            int Sixteen { get; set; }
        }

        /// <summary>
        /// Mass Test Object for splitting.
        /// </summary>
        public sealed class SomeMassObjectViewModel : ReactiveObject, ISomeMassObjectViewModel
        {
            private int _one;

            /// <summary>
            /// Initializes a new instance of the <see cref="SomeMassObjectViewModel"/> class.
            /// </summary>
#pragma warning disable GR0043 // ViewModel Constructor should have accept Scheduler as a parameter.
            public SomeMassObjectViewModel()
            {
                One = 1;
            }
#pragma warning restore GR0043 // ViewModel Constructor should have accept Scheduler as a parameter.

            /// <inheritdoc />
            public int One
            {
                get => _one;
                set => this.RaiseAndSetIfChanged(ref _one, value);
            }

            /// <inheritdoc />
            public int Two { get; set; } = 2;

            /// <inheritdoc />
            public int Three { get; set; } = 3;

            /// <inheritdoc />
            public int Four { get; set; } = 4;

            /// <inheritdoc />
            public int Five { get; set; } = 5;

            /// <inheritdoc />
            public int Six { get; set; } = 6;

            /// <inheritdoc />
            public int Seven { get; set; } = 7;

            /// <inheritdoc />
            public int Eight { get; set; } = 8;

            /// <inheritdoc />
            public int Nine { get; set; } = 9;

            /// <inheritdoc />
            public int Ten { get; set; } = 10;

            /// <inheritdoc />
            public int Eleven { get; set; } = 11;

            /// <inheritdoc />
            public int Twelve { get; set; } = 12;

            /// <inheritdoc />
            public int Thirteen { get; set; } = 13;

            /// <inheritdoc />
            public int Fourteen { get; set; } = 14;

            /// <inheritdoc />
            public int Fifteen { get; set; } = 15;

            /// <inheritdoc />
            public int Sixteen { get; set; } = 16;
        }

        /// <summary>
        /// Unit Tests for the Split Method.
        /// </summary>
        public sealed class SplitMethod
        {
            /// <summary>
            /// Check to ensure two observables are returned that track correctly.
            /// </summary>
            [Fact]
            public void ReturnsTwoObservables()
            {
                using (var observable = new Subject<SomeMassObjectViewModel>())
                {
                    var (observable1, observable2) = observable.Split(
                        x => x.One,
                        x => x.Two);

                    Assert.NotNull(observable1);
                    Assert.NotNull(observable2);

                    int item1 = 0;
                    int item2 = 0;

                    using (var cd = new CompositeDisposable())
                    {
                        cd.Add(observable1.Subscribe(i => item1 = i));
                        cd.Add(observable2.Subscribe(i => item2 = i));

                        var massObjectToTest = new SomeMassObjectViewModel();
                        observable.OnNext(massObjectToTest);

                        Assert.Equal(massObjectToTest.One, item1);
                        Assert.Equal(massObjectToTest.Two, item2);

                        massObjectToTest.One = 2;
                        observable.OnNext(massObjectToTest);

                        Assert.Equal(massObjectToTest.One, item1);
                        Assert.Equal(massObjectToTest.Two, item2);
                    }
                }
            }

            /// <summary>
            /// Check to ensure three observables are returned that track correctly.
            /// </summary>
            [Fact]
            public void ReturnsThreeObservables()
            {
                using (var observable = new Subject<SomeMassObjectViewModel>())
                {
                    var (
                        observable1,
                        observable2,
                        observable3) = observable.Split(
                        x => x.One,
                        x => x.Two,
                        x => x.Three);

                    Assert.NotNull(observable1);
                    Assert.NotNull(observable2);
                    Assert.NotNull(observable3);

                    int item1 = 0;
                    int item2 = 0;
                    int item3 = 0;

                    using (var cd = new CompositeDisposable())
                    {
                        cd.Add(observable1.Subscribe(i => item1 = i));
                        cd.Add(observable2.Subscribe(i => item2 = i));
                        cd.Add(observable3.Subscribe(i => item3 = i));

                        var massObjectToTest = new SomeMassObjectViewModel();
                        observable.OnNext(massObjectToTest);

                        Assert.Equal(massObjectToTest.One, item1);
                        Assert.Equal(massObjectToTest.Two, item2);
                        Assert.Equal(massObjectToTest.Three, item3);

                        massObjectToTest.One = 2;
                        observable.OnNext(massObjectToTest);

                        Assert.Equal(massObjectToTest.One, item1);
                        Assert.Equal(massObjectToTest.Two, item2);
                        Assert.Equal(massObjectToTest.Three, item3);
                    }
                }
            }
        }
    }
}
