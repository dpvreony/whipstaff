using System;
using System.Reactive.Linq;
using Splat.ApplicationPerformanceMonitoring;

namespace Whipstaff.Rx
{
    /// <summary>
    /// Extension methods for LINQ observables.
    /// </summary>
    public static class ObservableExtensions
    {
        /// <summary>
        /// Produces an observable that scans for the number of items in the sequence that are true.
        /// </summary>
        /// <param name="observable">The observable boolean sequence to scan.</param>
        /// <returns></returns>
        public static IObservable<int> ScanNumberThatAreTrue(this IObservable<bool> observable)
        {
            return observable.Scan(
                0,
                (accumulatedTotal, isRunning) => accumulatedTotal + (isRunning ? 1 : 0));
        }

        /// <summary>
        /// Produces an observable for monitoring the previous and current values.
        /// </summary>
        /// <remarks>
        /// Based upon http://www.zerobugbuild.com/?p=213
        /// </remarks>
        /// <typeparam name="TSource">The type for the input observable.</typeparam>
        /// <param name="source">The input observable source.</param>
        /// <returns>A tuple representing the previous and current values.</returns>
        public static IObservable<(TSource? Previous, TSource? Current)> ObserveCurrentAndPrevious<TSource>(this IObservable<TSource> source)
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
            this IObservable<TSource?> source,
            Func<TSource?, TSource?, TResult> selector)
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

        /// <summary>
        /// Wraps an observable subscription with a Feature Usage Tracking Session.
        /// </summary>
        /// <typeparam name="T">The type for the input observable.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <param name="featureUsageTrackingManager"></param>
        /// <param name="featureName"></param>
        /// <returns></returns>
        public static IDisposable SubscribeWithFeatureUsageTracking<T>(
            this IObservable<T> source,
            Action<T> onNext,
            Splat.ApplicationPerformanceMonitoring.IFeatureUsageTrackingManager featureUsageTrackingManager,
            string featureName)
        {
            return source.Subscribe(
                next => featureUsageTrackingManager.WithFeatureUsageTrackingSession(
                    featureName,
                    _ => onNext(next)));
        }

        /// <summary>
        /// Wraps an observable subscription with a Feature Usage Tracking Session.
        /// </summary>
        /// <typeparam name="T">The type for the input observable.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <param name="onError">Action to invoke upon exceptional termination of the observable sequence.</param>
        /// <param name="featureUsageTrackingManager"></param>
        /// <param name="featureName"></param>
        /// <returns></returns>
        public static IDisposable SubscribeWithFeatureUsageTracking<T>(
            this IObservable<T> source,
            Action<T> onNext,
            Action<Exception> onError,
            Splat.ApplicationPerformanceMonitoring.IFeatureUsageTrackingManager featureUsageTrackingManager,
            string featureName)
        {
            return source.Subscribe(
                next => featureUsageTrackingManager.WithFeatureUsageTrackingSession(
                    featureName,
                    _ => onNext(next)),
                onError);
        }

        /// <summary>
        /// Wraps an observable subscription with a Feature Usage Tracking Session.
        /// </summary>
        /// <typeparam name="T">The type for the input observable.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <param name="onCompleted">Action to invoke upon graceful termination of the observable sequence.</param>
        /// <param name="featureUsageTrackingManager">The Feature Usage Tracking Manager for starting the session under.</param>
        /// <param name="featureName">Name of the feature to track.</param>
        /// <returns>System.IDisposable object used to unsubscribe from the observable sequence.</returns>
        public static IDisposable SubscribeWithFeatureUsageTracking<T>(
            this IObservable<T> source,
            Action<T> onNext,
            Action onCompleted,
            Splat.ApplicationPerformanceMonitoring.IFeatureUsageTrackingManager featureUsageTrackingManager,
            string featureName)
        {
            return source.Subscribe(
                next => featureUsageTrackingManager.WithFeatureUsageTrackingSession(
                    featureName,
                    _ => onNext(next)),
                onCompleted);
        }

        /// <summary>
        /// Wraps an observable subscription with a Feature Usage Tracking Session.
        /// </summary>
        /// <typeparam name="T">The type for the input observable.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <param name="onError">Action to invoke upon exceptional termination of the observable sequence.</param>
        /// <param name="onCompleted">Action to invoke upon graceful termination of the observable sequence.</param>
        /// <param name="featureUsageTrackingManager">The Feature Usage Tracking Manager for starting the session under.</param>
        /// <param name="featureName">Name of the feature to track.</param>
        /// <returns>System.IDisposable object used to unsubscribe from the observable sequence.</returns>
        public static IDisposable SubscribeWithFeatureUsageTracking<T>(
            this IObservable<T> source,
            Action<T> onNext,
            Action<Exception> onError,
            Action onCompleted,
            Splat.ApplicationPerformanceMonitoring.IFeatureUsageTrackingManager featureUsageTrackingManager,
            string featureName)
        {
            return source.Subscribe(
                next => featureUsageTrackingManager.WithFeatureUsageTrackingSession(
                    featureName,
                    _ => onNext(next)),
                onError,
                onCompleted);
        }

        /// <summary>
        /// Wraps an observable subscription with a Feature Usage Tracking Session.
        /// </summary>
        /// <typeparam name="T">The type for the input observable.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="observer">The observer that will receive the subscription notification.</param>
        /// <param name="featureUsageTrackingManager">The Feature Usage Tracking Manager for starting the session under.</param>
        /// <param name="featureName">Name of the feature to track.</param>
        /// <returns>System.IDisposable object used to unsubscribe from the observable sequence.</returns>
        public static IDisposable SubscribeWithFeatureUsageTracking<T>(
            this IObservable<T> source,
            IObserver<T> observer,
            Splat.ApplicationPerformanceMonitoring.IFeatureUsageTrackingManager featureUsageTrackingManager,
            string featureName)
        {
            return source.Subscribe(
                next => featureUsageTrackingManager.WithFeatureUsageTrackingSession(
                    featureName,
                    _ => observer.OnNext(next)));
        }

        /// <summary>
        /// Wraps an observable subscription with a SubFeature Usage Tracking Session.
        /// </summary>
        /// <typeparam name="T">The type for the input observable.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <param name="featureUsageTrackingSession">The Feature Usage Tracking Session to track the Sub Feature under.</param>
        /// <param name="featureName">Name of the feature to track.</param>
        /// <returns>System.IDisposable object used to unsubscribe from the observable sequence.</returns>
        public static IDisposable SubscribeWithSubFeatureUsageTracking<T>(
            this IObservable<T> source,
            Action<T> onNext,
            IFeatureUsageTrackingSession  featureUsageTrackingSession,
            string featureName)
        {
            return source.Subscribe(
                next => featureUsageTrackingSession.WithSubFeatureUsageTrackingSession(
                    featureName,
                    _ => onNext(next)));
        }

        /// <summary>
        /// Wraps an observable subscription with a SubFeature Usage Tracking Session.
        /// </summary>
        /// <typeparam name="T">The type for the input observable.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <param name="onError">Action to invoke upon exceptional termination of the observable sequence.</param>
        /// <param name="featureUsageTrackingSession">The Feature Usage Tracking Session to track the Sub Feature under.</param>
        /// <param name="featureName">Name of the feature to track.</param>
        /// <returns>System.IDisposable object used to unsubscribe from the observable sequence.</returns>
        public static IDisposable SubscribeWithSubFeatureUsageTracking<T>(
            this IObservable<T> source,
            Action<T> onNext,
            Action<Exception> onError,
            IFeatureUsageTrackingSession  featureUsageTrackingSession,
            string featureName)
        {
            return source.Subscribe(
                next => featureUsageTrackingSession.WithSubFeatureUsageTrackingSession(
                    featureName,
                    _ => onNext(next)),
                onError);
        }

        /// <summary>
        /// Wraps an observable subscription with a SubFeature Usage Tracking Session.
        /// </summary>
        /// <typeparam name="T">The type for the input observable.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <param name="onCompleted">Action to invoke upon graceful termination of the observable sequence.</param>
        /// <param name="featureUsageTrackingSession">The Feature Usage Tracking Session to track the Sub Feature under.</param>
        /// <param name="featureName">Name of the feature to track.</param>
        /// <returns>System.IDisposable object used to unsubscribe from the observable sequence.</returns>
        public static IDisposable SubscribeWithSubFeatureUsageTracking<T>(
            this IObservable<T> source,
            Action<T> onNext,
            Action onCompleted,
            IFeatureUsageTrackingSession  featureUsageTrackingSession,
            string featureName)
        {
            return source.Subscribe(
                next => featureUsageTrackingSession.WithSubFeatureUsageTrackingSession(
                    featureName,
                    _ => onNext(next)),
                onCompleted);
        }

        /// <summary>
        /// Wraps an observable subscription with a SubFeature Usage Tracking Session.
        /// </summary>
        /// <typeparam name="T">The type for the input observable.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <param name="onError">Action to invoke upon exceptional termination of the observable sequence.</param>
        /// <param name="onCompleted">Action to invoke upon graceful termination of the observable sequence.</param>
        /// <param name="featureUsageTrackingSession">The Feature Usage Tracking Session to track the Sub Feature under.</param>
        /// <param name="featureName">Name of the feature to track.</param>
        /// <returns>System.IDisposable object used to unsubscribe from the observable sequence.</returns>
        public static IDisposable SubscribeWithSubFeatureUsageTracking<T>(
            this IObservable<T> source,
            Action<T> onNext,
            Action<Exception> onError,
            Action onCompleted,
            IFeatureUsageTrackingSession  featureUsageTrackingSession,
            string featureName)
        {
            return source.Subscribe(
                next => featureUsageTrackingSession.WithSubFeatureUsageTrackingSession(
                    featureName,
                    _ => onNext(next)),
                onError,
                onCompleted);
        }

        /// <summary>
        /// Wraps an observable subscription with a SubFeature Usage Tracking Session.
        /// </summary>
        /// <typeparam name="T">The type for the input observable.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="observer">The observer that will receive the subscription notification.</param>
        /// <param name="featureUsageTrackingSession">The Feature Usage Tracking Session to track the Sub Feature under.</param>
        /// <param name="featureName">Name of the feature to track.</param>
        /// <returns>System.IDisposable object used to unsubscribe from the observable sequence.</returns>
        public static IDisposable SubscribeWithSubFeatureUsageTracking<T>(
            this IObservable<T> source,
            IObserver<T> observer,
            IFeatureUsageTrackingSession  featureUsageTrackingSession,
            string featureName)
        {
            return source.Subscribe(
                next => featureUsageTrackingSession.WithSubFeatureUsageTrackingSession(
                    featureName,
                    _ => observer.OnNext(next)));
        }

        private static void WithFeatureUsageTrackingSession(
            this Splat.ApplicationPerformanceMonitoring.IFeatureUsageTrackingManager featureUsageTrackingManager,
            string featureName,
            Action<IFeatureUsageTrackingSession> action)
        {
            using (var session = featureUsageTrackingManager.GetFeatureUsageTrackingSession(featureName))
            {
                try
                {
                    action(session);
                }
                catch (Exception exception)
                {
                    session.OnException(exception);
                    throw;
                }
            }
        }
    }
}
