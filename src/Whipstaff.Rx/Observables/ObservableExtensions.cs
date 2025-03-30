// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Linq;

namespace Whipstaff.Rx.Observables
{
    /// <summary>
    /// Extension methods for <see cref="IObservable{T}"/>.
    /// </summary>
    public static class ObservableExtensions
    {
        /// <summary>
        /// Returns an observable that will return true if both values in the tuple are false.
        /// </summary>
        /// <remarks>
        /// The main use case for this is checking boolean flags from <see cref="System.Reactive.Linq.ObservableEx.CombineLatest{TFirst, TSecond}"/>.</remarks>
        /// <param name="observable">The observable to check.</param>
        /// <returns>An observable sequence for whether all boolean flags in the tuple are false.</returns>
        public static IObservable<bool> AllFalse(this IObservable<(bool First, bool Second)> observable)
        {
            return observable.Select(static tuple => tuple is { First: false, Second: false });
        }

        /// <summary>
        /// Returns an observable that will return true if all values in the tuple are false.
        /// </summary>
        /// <remarks>
        /// The main use case for this is checking boolean flags from <see cref="System.Reactive.Linq.ObservableEx.CombineLatest{TFirst, TSecond, TThird}"/>.</remarks>
        /// <param name="observable">The observable to check.</param>
        /// <returns>An observable sequence for whether all boolean flags in the tuple are false.</returns>
        public static IObservable<bool> AllFalse(this IObservable<(bool First, bool Second, bool Third)> observable)
        {
            return observable.Select(static tuple => tuple is { First: false, Second: false, Third: false });
        }

        /// <summary>
        /// Returns an observable that will return true if all values in the tuple are false.
        /// </summary>
        /// <remarks>
        /// The main use case for this is checking boolean flags from <see cref="System.Reactive.Linq.ObservableEx.CombineLatest{TFirst, TSecond, TThird, TFourth}"/>.</remarks>
        /// <param name="observable">The observable to check.</param>
        /// <returns>An observable sequence for whether all boolean flags in the tuple are false.</returns>
        public static IObservable<bool> AllFalse(this IObservable<(bool First, bool Second, bool Third, bool Fourth)> observable)
        {
            return observable.Select(static tuple => tuple is { First: false, Second: false, Third: false, Fourth: false });
        }

        /// <summary>
        /// Returns an observable that will return true if all values in the tuple are false.
        /// </summary>
        /// <remarks>
        /// The main use case for this is checking boolean flags from <see cref="System.Reactive.Linq.ObservableEx.CombineLatest{TFirst, TSecond, TThird, TFourth, TFifth}"/>.</remarks>
        /// <param name="observable">The observable to check.</param>
        /// <returns>An observable sequence for whether all boolean flags in the tuple are false.</returns>
        public static IObservable<bool> AllFalse(this IObservable<(bool First, bool Second, bool Third, bool Fourth, bool Fifth)> observable)
        {
            return observable.Select(static tuple => tuple is { First: false, Second: false, Third: false, Fourth: false, Fifth: false });
        }

        /// <summary>
        /// Returns an observable that will return true if all values in the tuple are false.
        /// </summary>
        /// <remarks>
        /// The main use case for this is checking boolean flags from <see cref="System.Reactive.Linq.ObservableEx.CombineLatest{TFirst, TSecond, TThird, TFourth, TFifth, TSixth}"/>.</remarks>
        /// <param name="observable">The observable to check.</param>
        /// <returns>An observable sequence for whether all boolean flags in the tuple are false.</returns>
        public static IObservable<bool> AllFalse(this IObservable<(bool First, bool Second, bool Third, bool Fourth, bool Fifth, bool Sixth)> observable)
        {
            return observable.Select(static tuple => tuple is { First: false, Second: false, Third: false, Fourth: false, Fifth: false, Sixth: false });
        }

        /// <summary>
        /// Returns an observable that will return true if all values in the tuple are false.
        /// </summary>
        /// <remarks>
        /// The main use case for this is checking boolean flags from <see cref="System.Reactive.Linq.ObservableEx.CombineLatest{TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh}"/>.</remarks>
        /// <param name="observable">The observable to check.</param>
        /// <returns>An observable sequence for whether all boolean flags in the tuple are false.</returns>
        public static IObservable<bool> AllFalse(this IObservable<(bool First, bool Second, bool Third, bool Fourth, bool Fifth, bool Sixth, bool Seventh)> observable)
        {
            return observable.Select(static tuple => tuple is { First: false, Second: false, Third: false, Fourth: false, Fifth: false, Sixth: false, Seventh: false });
        }

        /// <summary>
        /// Returns an observable that will return true if all values in the tuple are false.
        /// </summary>
        /// <remarks>
        /// The main use case for this is checking boolean flags from <see cref="System.Reactive.Linq.ObservableEx.CombineLatest{TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth}"/>.</remarks>
        /// <param name="observable">The observable to check.</param>
        /// <returns>An observable sequence for whether all boolean flags in the tuple are false.</returns>
        public static IObservable<bool> AllFalse(this IObservable<(bool First, bool Second, bool Third, bool Fourth, bool Fifth, bool Sixth, bool Seventh, bool Eighth)> observable)
        {
            return observable.Select(static tuple => tuple is { First: false, Second: false, Third: false, Fourth: false, Fifth: false, Sixth: false, Seventh: false, Eighth: false });
        }

        /// <summary>
        /// Returns an observable that will return true if both values in the tuple are true.
        /// </summary>
        /// <remarks>
        /// The main use case for this is checking boolean flags from <see cref="System.Reactive.Linq.ObservableEx.CombineLatest{TFirst, TSecond}"/>.</remarks>
        /// <param name="observable">The observable to check.</param>
        /// <returns>An observable sequence for whether all boolean flags in the tuple are true.</returns>
        public static IObservable<bool> AllTrue(this IObservable<(bool First, bool Second)> observable)
        {
            return observable.Select(static tuple => tuple is { First: true, Second: true });
        }

        /// <summary>
        /// Returns an observable that will return true if all values in the tuple are true.
        /// </summary>
        /// <remarks>
        /// The main use case for this is checking boolean flags from <see cref="System.Reactive.Linq.ObservableEx.CombineLatest{TFirst, TSecond, TThird}"/>.</remarks>
        /// <param name="observable">The observable to check.</param>
        /// <returns>An observable sequence for whether all boolean flags in the tuple are true.</returns>
        public static IObservable<bool> AllTrue(this IObservable<(bool First, bool Second, bool Third)> observable)
        {
            return observable.Select(static tuple => tuple is { First: true, Second: true, Third: true });
        }

        /// <summary>
        /// Returns an observable that will return true if all values in the tuple are true.
        /// </summary>
        /// <remarks>
        /// The main use case for this is checking boolean flags from <see cref="System.Reactive.Linq.ObservableEx.CombineLatest{TFirst, TSecond, TThird, TFourth}"/>.</remarks>
        /// <param name="observable">The observable to check.</param>
        /// <returns>An observable sequence for whether all boolean flags in the tuple are true.</returns>
        public static IObservable<bool> AllTrue(this IObservable<(bool First, bool Second, bool Third, bool Fourth)> observable)
        {
            return observable.Select(static tuple => tuple is { First: true, Second: true, Third: true, Fourth: true });
        }

        /// <summary>
        /// Returns an observable that will return true if all values in the tuple are true.
        /// </summary>
        /// <remarks>
        /// The main use case for this is checking boolean flags from <see cref="System.Reactive.Linq.ObservableEx.CombineLatest{TFirst, TSecond, TThird, TFourth, TFifth}"/>.</remarks>
        /// <param name="observable">The observable to check.</param>
        /// <returns>An observable sequence for whether all boolean flags in the tuple are true.</returns>
        public static IObservable<bool> AllTrue(this IObservable<(bool First, bool Second, bool Third, bool Fourth, bool Fifth)> observable)
        {
            return observable.Select(static tuple => tuple is { First: true, Second: true, Third: true, Fourth: true, Fifth: true });
        }

        /// <summary>
        /// Returns an observable that will return true if all values in the tuple are true.
        /// </summary>
        /// <remarks>
        /// The main use case for this is checking boolean flags from <see cref="System.Reactive.Linq.ObservableEx.CombineLatest{TFirst, TSecond, TThird, TFourth, TFifth, TSixth}"/>.</remarks>
        /// <param name="observable">The observable to check.</param>
        /// <returns>An observable sequence for whether all boolean flags in the tuple are true.</returns>
        public static IObservable<bool> AllTrue(this IObservable<(bool First, bool Second, bool Third, bool Fourth, bool Fifth, bool Sixth)> observable)
        {
            return observable.Select(static tuple => tuple is { First: true, Second: true, Third: true, Fourth: true, Fifth: true, Sixth: true });
        }

        /// <summary>
        /// Returns an observable that will return true if all values in the tuple are true.
        /// </summary>
        /// <remarks>
        /// The main use case for this is checking boolean flags from <see cref="System.Reactive.Linq.ObservableEx.CombineLatest{TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh}"/>.</remarks>
        /// <param name="observable">The observable to check.</param>
        /// <returns>An observable sequence for whether all boolean flags in the tuple are true.</returns>
        public static IObservable<bool> AllTrue(this IObservable<(bool First, bool Second, bool Third, bool Fourth, bool Fifth, bool Sixth, bool Seventh)> observable)
        {
            return observable.Select(static tuple => tuple is { First: true, Second: true, Third: true, Fourth: true, Fifth: true, Sixth: true, Seventh: true });
        }

        /// <summary>
        /// Returns an observable that will return true if all values in the tuple are true.
        /// </summary>
        /// <remarks>
        /// The main use case for this is checking boolean flags from <see cref="System.Reactive.Linq.ObservableEx.CombineLatest{TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth}"/>.</remarks>
        /// <param name="observable">The observable to check.</param>
        /// <returns>An observable sequence for whether all boolean flags in the tuple are true.</returns>
        public static IObservable<bool> AllTrue(this IObservable<(bool First, bool Second, bool Third, bool Fourth, bool Fifth, bool Sixth, bool Seventh, bool Eighth)> observable)
        {
            return observable.Select(static tuple => tuple is { First: true, Second: true, Third: true, Fourth: true, Fifth: true, Sixth: true, Seventh: true, Eighth: true });
        }
    }
}
