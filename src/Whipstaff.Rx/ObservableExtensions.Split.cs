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
    }
}
