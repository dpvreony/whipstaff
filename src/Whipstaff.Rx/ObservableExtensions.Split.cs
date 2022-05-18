using System;
using System.Reactive.Linq;

namespace Whipstaff.Rx
{
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
        /// <returns></returns>
        public static (IObservable<TResult1>, IObservable<TResult2>) Split<TInput, TResult1, TResult2>(
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
        /// <returns></returns>
        public static (IObservable<TResult1>, IObservable<TResult2>, IObservable<TResult3>) Split<TInput, TResult1, TResult2, TResult3>(
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
        /// <typeparam name="TResult4">The output type for the third consuming observable.</typeparam>
        /// <param name="observable">The upstream observable.</param>
        /// <param name="selector1">The selector to apply to the upstream observable in order to create the first consuming observable.</param>
        /// <param name="selector2">The selector to apply to the upstream observable in order to create the second consuming observable.</param>
        /// <param name="selector3">The selector to apply to the upstream observable in order to create the third consuming observable.</param>
        /// <param name="selector4">The selector to apply to the upstream observable in order to create the fourth consuming observable.</param>
        /// <returns></returns>
        public static (IObservable<TResult1>, IObservable<TResult2>, IObservable<TResult3>, IObservable<TResult4>) Split<TInput, TResult1, TResult2, TResult3, TResult4>(
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
    }
}
