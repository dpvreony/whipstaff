using System;
using System.Reactive.Linq;

namespace Whipstaff.Rx
{
    /// <summary>
    /// Extension methods for LINQ observables.
    /// </summary>
    public static class ObservableExtensions
    {
        /// <summary>
        /// Produces an observable for monitoring the previous and current values.
        /// </summary>
        /// <remarks>
        /// Based upon http://www.zerobugbuild.com/?p=213
        /// </remarks>
        /// <typeparam name="TSource">The type for the input observable.</typeparam>
        /// <param name="source">The input observable source.</param>
        /// <returns>A tuple representing the previous and current values.</returns>
        public static IObservable<(TSource Previous, TSource Current)> ObserveCurrentAndPrevious<TSource>(this IObservable<TSource> source)
        {
            return source.Scan(
                (Previous: default(TSource), Current: default(TSource)),
                (acc, current) => (acc.Previous, current));
        }

        /// <summary>
        /// Produces an observable for monitoring the previous and current values, allowing manipulation of the result via a selector.
        /// </summary>
        /// <remarks>
        /// Based upon http://www.zerobugbuild.com/?p=213
        /// </remarks>
        /// <typeparam name="TSource">The type for the input observable.</typeparam>
        /// <typeparam name="TResult">The type for the result returned by the method.</typeparam>
        /// <param name="source">The input observable source.</param>
        /// <param name="selector">Selector to process the tuple of the previous and current values.</param>
        /// <returns>The output of the selector function.</returns>
        public static IObservable<TResult> ObserveCurrentAndPrevious<TSource, TResult>(
            this IObservable<TSource> source,
            Func<TSource, TSource, TResult> selector)
        {
            return source.Scan(
                Tuple.Create(default(TSource), default(TSource)),
                (acc, current) => Tuple.Create(
                    acc.Item2,
                    current))
                .Select(s => selector(
                    s.Item1,
                    s.Item2));
        }
    }
}
