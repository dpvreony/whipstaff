// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using ReactiveUI;
using Whipstaff.Rx;
using Xunit;

namespace Whipstaff.UnitTests.Rx
{
    /// <summary>
    /// Extension methods for performing split on an observable.
    /// </summary>
    public static class SplitExtensionsTests
    {
        /// <summary>
        /// Mass Test Object for splitting.
        /// </summary>
        public sealed class SomeMassObject : ReactiveObject
        {
            private int _one;

            /// <summary>
            /// Initializes a new instance of the <see cref="SomeMassObject"/> class.
            /// </summary>
            public SomeMassObject()
            {
                One = 1;
            }

            /// <summary>
            /// Gets or sets the first property.
            /// </summary>
            public int One
            {
                get => _one;
                set => this.RaiseAndSetIfChanged(ref _one, value);
            }

            /// <summary>
            /// Gets or sets the second property.
            /// </summary>
            public int Two { get; set; } = 2;

            /// <summary>
            /// Gets or sets the third property.
            /// </summary>
            public int Three { get; set; } = 3;

            /// <summary>
            /// Gets or sets the fourth property.
            /// </summary>
            public int Four { get; set; } = 4;

            /// <summary>
            /// Gets or sets the fifth property.
            /// </summary>
            public int Five { get; set; } = 5;

            /// <summary>
            /// Gets or sets the sixth property.
            /// </summary>
            public int Six { get; set; } = 6;

            /// <summary>
            /// Gets or sets the seventh property.
            /// </summary>
            public int Seven { get; set; } = 7;

            /// <summary>
            /// Gets or sets the eight property.
            /// </summary>
            public int Eight { get; set; } = 8;

            /// <summary>
            /// Gets or sets the ninth property.
            /// </summary>
            public int Nine { get; set; } = 9;

            /// <summary>
            /// Gets or sets the tenth property.
            /// </summary>
            public int Ten { get; set; } = 10;

            /// <summary>
            /// Gets or sets the eleventh property.
            /// </summary>
            public int Eleven { get; set; } = 11;

            /// <summary>
            /// Gets or sets the twelfth property.
            /// </summary>
            public int Twelve { get; set; } = 12;

            /// <summary>
            /// Gets or sets the thirteenth property.
            /// </summary>
            public int Thirteen { get; set; } = 13;

            /// <summary>
            /// Gets or sets the fourteenth property.
            /// </summary>
            public int Fourteen { get; set; } = 14;

            /// <summary>
            /// Gets or sets the fifteenth property.
            /// </summary>
            public int Fifteen { get; set; } = 15;

            /// <summary>
            /// Gets or sets the sixteenth property.
            /// </summary>
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
                using (var observable = new Subject<SomeMassObject>())
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

                        var massObjectToTest = new SomeMassObject();
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
                using (var observable = new Subject<SomeMassObject>())
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

                        var massObjectToTest = new SomeMassObject();
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
