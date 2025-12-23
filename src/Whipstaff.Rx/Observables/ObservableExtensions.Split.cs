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
#pragma warning disable GR0049 // Do not use Tuples.
    public static partial class ObservableExtensions
    {
        /// <summary>
        /// Splits an observable into 2 observables based on items selected from the upstream observable.
        /// </summary>
        /// <typeparam name="TInput">The input type from the upstream observable.</typeparam>
        /// <typeparam name="TResult1">The output type for the first consuming observable.</typeparam>
        /// <typeparam name="TResult2">The output type for the second consuming observable.</typeparam>
        /// <param name="observable">The upstream observable.</param>
        /// <param name="selector1">The selector to apply to the upstream observable in order to create the first consuming observable.</param>
        /// <param name="selector2">The selector to apply to the upstream observable in order to create the second consuming observable.</param>
        /// <returns>Tuple of 2 split observables.</returns>
        public static (IObservable<TResult1> Observable1, IObservable<TResult2> Observable2) Split<TInput, TResult1, TResult2>(
            this IObservable<TInput> observable,
            Func<TInput, TResult1> selector1,
            Func<TInput, TResult2> selector2)
        {
            return (
                observable.Select(selector1),
                observable.Select(selector2));
        }

        /// <summary>
        /// Splits an observable into 3 observables based on items selected from the upstream observable.
        /// </summary>
        /// <typeparam name="TInput">The input type from the upstream observable.</typeparam>
        /// <typeparam name="TResult1">The output type for the first consuming observable.</typeparam>
        /// <typeparam name="TResult2">The output type for the second consuming observable.</typeparam>
        /// <typeparam name="TResult3">The output type for the third consuming observable.</typeparam>
        /// <param name="observable">The upstream observable.</param>
        /// <param name="selector1">The selector to apply to the upstream observable in order to create the first consuming observable.</param>
        /// <param name="selector2">The selector to apply to the upstream observable in order to create the second consuming observable.</param>
        /// <param name="selector3">The selector to apply to the upstream observable in order to create the third consuming observable.</param>
        /// <returns>Tuple of 3 split observables.</returns>
        public static (IObservable<TResult1> Observable1, IObservable<TResult2> Observable2, IObservable<TResult3> Observable3) Split<TInput, TResult1, TResult2, TResult3>(
            this IObservable<TInput> observable,
            Func<TInput, TResult1> selector1,
            Func<TInput, TResult2> selector2,
            Func<TInput, TResult3> selector3)
        {
            return (
                observable.Select(selector1),
                observable.Select(selector2),
                observable.Select(selector3));
        }

        /// <summary>
        /// Splits an observable into 4 observables based on items selected from the upstream observable.
        /// </summary>
        /// <typeparam name="TInput">The input type from the upstream observable.</typeparam>
        /// <typeparam name="TResult1">The output type for the first consuming observable.</typeparam>
        /// <typeparam name="TResult2">The output type for the second consuming observable.</typeparam>
        /// <typeparam name="TResult3">The output type for the third consuming observable.</typeparam>
        /// <typeparam name="TResult4">The output type for the fourth consuming observable.</typeparam>
        /// <param name="observable">The upstream observable.</param>
        /// <param name="selector1">The selector to apply to the upstream observable in order to create the first consuming observable.</param>
        /// <param name="selector2">The selector to apply to the upstream observable in order to create the second consuming observable.</param>
        /// <param name="selector3">The selector to apply to the upstream observable in order to create the third consuming observable.</param>
        /// <param name="selector4">The selector to apply to the upstream observable in order to create the fourth consuming observable.</param>
        /// <returns>Tuple of 4 split observables.</returns>
        public static (IObservable<TResult1> Observable1, IObservable<TResult2> Observable2, IObservable<TResult3> Observable3, IObservable<TResult4> Observable4) Split<TInput, TResult1, TResult2, TResult3, TResult4>(
            this IObservable<TInput> observable,
            Func<TInput, TResult1> selector1,
            Func<TInput, TResult2> selector2,
            Func<TInput, TResult3> selector3,
            Func<TInput, TResult4> selector4)
        {
            return (
                observable.Select(selector1),
                observable.Select(selector2),
                observable.Select(selector3),
                observable.Select(selector4));
        }

        /// <summary>
        /// Splits an observable into 5 observables based on items selected from the upstream observable.
        /// </summary>
        /// <typeparam name="TInput">The input type from the upstream observable.</typeparam>
        /// <typeparam name="TResult1">The output type for the first consuming observable.</typeparam>
        /// <typeparam name="TResult2">The output type for the second consuming observable.</typeparam>
        /// <typeparam name="TResult3">The output type for the third consuming observable.</typeparam>
        /// <typeparam name="TResult4">The output type for the fourth consuming observable.</typeparam>
        /// <typeparam name="TResult5">The output type for the fifth consuming observable.</typeparam>
        /// <param name="observable">The upstream observable.</param>
        /// <param name="selector1">The selector to apply to the upstream observable in order to create the first consuming observable.</param>
        /// <param name="selector2">The selector to apply to the upstream observable in order to create the second consuming observable.</param>
        /// <param name="selector3">The selector to apply to the upstream observable in order to create the third consuming observable.</param>
        /// <param name="selector4">The selector to apply to the upstream observable in order to create the fourth consuming observable.</param>
        /// <param name="selector5">The selector to apply to the upstream observable in order to create the fifth consuming observable.</param>
        /// <returns>Tuple of 5 split observables.</returns>
        public static (IObservable<TResult1> Observable1, IObservable<TResult2> Observable2, IObservable<TResult3> Observable3, IObservable<TResult4> Observable4, IObservable<TResult5> Observable5) Split<TInput, TResult1, TResult2, TResult3, TResult4, TResult5>(
            this IObservable<TInput> observable,
            Func<TInput, TResult1> selector1,
            Func<TInput, TResult2> selector2,
            Func<TInput, TResult3> selector3,
            Func<TInput, TResult4> selector4,
            Func<TInput, TResult5> selector5)
        {
            return (
                observable.Select(selector1),
                observable.Select(selector2),
                observable.Select(selector3),
                observable.Select(selector4),
                observable.Select(selector5));
        }

        /// <summary>
        /// Splits an observable into 6 observables based on items selected from the upstream observable.
        /// </summary>
        /// <typeparam name="TInput">The input type from the upstream observable.</typeparam>
        /// <typeparam name="TResult1">The output type for the first consuming observable.</typeparam>
        /// <typeparam name="TResult2">The output type for the second consuming observable.</typeparam>
        /// <typeparam name="TResult3">The output type for the third consuming observable.</typeparam>
        /// <typeparam name="TResult4">The output type for the fourth consuming observable.</typeparam>
        /// <typeparam name="TResult5">The output type for the fifth consuming observable.</typeparam>
        /// <typeparam name="TResult6">The output type for the sixth consuming observable.</typeparam>
        /// <param name="observable">The upstream observable.</param>
        /// <param name="selector1">The selector to apply to the upstream observable in order to create the first consuming observable.</param>
        /// <param name="selector2">The selector to apply to the upstream observable in order to create the second consuming observable.</param>
        /// <param name="selector3">The selector to apply to the upstream observable in order to create the third consuming observable.</param>
        /// <param name="selector4">The selector to apply to the upstream observable in order to create the fourth consuming observable.</param>
        /// <param name="selector5">The selector to apply to the upstream observable in order to create the fifth consuming observable.</param>
        /// <param name="selector6">The selector to apply to the upstream observable in order to create the sixth consuming observable.</param>
        /// <returns>Tuple of 6 split observables.</returns>
        public static (IObservable<TResult1> Observable1, IObservable<TResult2> Observable2, IObservable<TResult3> Observable3, IObservable<TResult4> Observable4, IObservable<TResult5> Observable5, IObservable<TResult6> Observable6) Split<TInput, TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>(
            this IObservable<TInput> observable,
            Func<TInput, TResult1> selector1,
            Func<TInput, TResult2> selector2,
            Func<TInput, TResult3> selector3,
            Func<TInput, TResult4> selector4,
            Func<TInput, TResult5> selector5,
            Func<TInput, TResult6> selector6)
        {
            return (
                observable.Select(selector1),
                observable.Select(selector2),
                observable.Select(selector3),
                observable.Select(selector4),
                observable.Select(selector5),
                observable.Select(selector6));
        }

        /// <summary>
        /// Splits an observable into 7 observables based on items selected from the upstream observable.
        /// </summary>
        /// <typeparam name="TInput">The input type from the upstream observable.</typeparam>
        /// <typeparam name="TResult1">The output type for the first consuming observable.</typeparam>
        /// <typeparam name="TResult2">The output type for the second consuming observable.</typeparam>
        /// <typeparam name="TResult3">The output type for the third consuming observable.</typeparam>
        /// <typeparam name="TResult4">The output type for the fourth consuming observable.</typeparam>
        /// <typeparam name="TResult5">The output type for the fifth consuming observable.</typeparam>
        /// <typeparam name="TResult6">The output type for the sixth consuming observable.</typeparam>
        /// <typeparam name="TResult7">The output type for the seventh consuming observable.</typeparam>
        /// <param name="observable">The upstream observable.</param>
        /// <param name="selector1">The selector to apply to the upstream observable in order to create the first consuming observable.</param>
        /// <param name="selector2">The selector to apply to the upstream observable in order to create the second consuming observable.</param>
        /// <param name="selector3">The selector to apply to the upstream observable in order to create the third consuming observable.</param>
        /// <param name="selector4">The selector to apply to the upstream observable in order to create the fourth consuming observable.</param>
        /// <param name="selector5">The selector to apply to the upstream observable in order to create the fifth consuming observable.</param>
        /// <param name="selector6">The selector to apply to the upstream observable in order to create the sixth consuming observable.</param>
        /// <param name="selector7">The selector to apply to the upstream observable in order to create the seventh consuming observable.</param>
        /// <returns>Tuple of 7 split observables.</returns>
        public static (IObservable<TResult1> Observable1, IObservable<TResult2> Observable2, IObservable<TResult3> Observable3, IObservable<TResult4> Observable4, IObservable<TResult5> Observable5, IObservable<TResult6> Observable6, IObservable<TResult7> Observable7) Split<TInput, TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>(
            this IObservable<TInput> observable,
            Func<TInput, TResult1> selector1,
            Func<TInput, TResult2> selector2,
            Func<TInput, TResult3> selector3,
            Func<TInput, TResult4> selector4,
            Func<TInput, TResult5> selector5,
            Func<TInput, TResult6> selector6,
            Func<TInput, TResult7> selector7)
        {
            return (
                observable.Select(selector1),
                observable.Select(selector2),
                observable.Select(selector3),
                observable.Select(selector4),
                observable.Select(selector5),
                observable.Select(selector6),
                observable.Select(selector7));
        }

        /// <summary>
        /// Splits an observable into 8 observables based on items selected from the upstream observable.
        /// </summary>
        /// <typeparam name="TInput">The input type from the upstream observable.</typeparam>
        /// <typeparam name="TResult1">The output type for the first consuming observable.</typeparam>
        /// <typeparam name="TResult2">The output type for the second consuming observable.</typeparam>
        /// <typeparam name="TResult3">The output type for the third consuming observable.</typeparam>
        /// <typeparam name="TResult4">The output type for the fourth consuming observable.</typeparam>
        /// <typeparam name="TResult5">The output type for the fifth consuming observable.</typeparam>
        /// <typeparam name="TResult6">The output type for the sixth consuming observable.</typeparam>
        /// <typeparam name="TResult7">The output type for the seventh consuming observable.</typeparam>
        /// <typeparam name="TResult8">The output type for the eighth consuming observable.</typeparam>
        /// <param name="observable">The upstream observable.</param>
        /// <param name="selector1">The selector to apply to the upstream observable in order to create the first consuming observable.</param>
        /// <param name="selector2">The selector to apply to the upstream observable in order to create the second consuming observable.</param>
        /// <param name="selector3">The selector to apply to the upstream observable in order to create the third consuming observable.</param>
        /// <param name="selector4">The selector to apply to the upstream observable in order to create the fourth consuming observable.</param>
        /// <param name="selector5">The selector to apply to the upstream observable in order to create the fifth consuming observable.</param>
        /// <param name="selector6">The selector to apply to the upstream observable in order to create the sixth consuming observable.</param>
        /// <param name="selector7">The selector to apply to the upstream observable in order to create the seventh consuming observable.</param>
        /// <param name="selector8">The selector to apply to the upstream observable in order to create the eighth consuming observable.</param>
        /// <returns>Tuple of 8 split observables.</returns>
        public static (
            IObservable<TResult1> Observable1,
            IObservable<TResult2> Observable2,
            IObservable<TResult3> Observable3,
            IObservable<TResult4> Observable4,
            IObservable<TResult5> Observable5,
            IObservable<TResult6> Observable6,
            IObservable<TResult7> Observable7,
            IObservable<TResult8> Observable8) Split<
                TInput,
                TResult1,
                TResult2,
                TResult3,
                TResult4,
                TResult5,
                TResult6,
                TResult7,
                TResult8>(
            this IObservable<TInput> observable,
            Func<TInput, TResult1> selector1,
            Func<TInput, TResult2> selector2,
            Func<TInput, TResult3> selector3,
            Func<TInput, TResult4> selector4,
            Func<TInput, TResult5> selector5,
            Func<TInput, TResult6> selector6,
            Func<TInput, TResult7> selector7,
            Func<TInput, TResult8> selector8)
        {
            return (
                observable.Select(selector1),
                observable.Select(selector2),
                observable.Select(selector3),
                observable.Select(selector4),
                observable.Select(selector5),
                observable.Select(selector6),
                observable.Select(selector7),
                observable.Select(selector8));
        }
    }
#pragma warning restore GR0049 // Do not use Tuples
}
