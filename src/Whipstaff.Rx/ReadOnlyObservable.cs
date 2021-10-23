using System;
using System.Reactive.Subjects;

namespace Whipstaff.Rx
{
    /// <summary>
    /// Represents a <see cref="T:System.Reactive.Subjects.BehaviorSubject">BehaviorSubject</see> that has been wrapped to make it read only by hiding the next, error, completed methods.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadOnlyObservable<T> : IObservable<T>
    {
        /// <summary>
        ///
        /// </summary>
        T Value { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool TryGetValue(out T value);
    }

    /// <summary>
    /// Represents a <see cref="T:System.Reactive.Subjects.BehaviorSubject">BehaviorSubject</see> that has been wrapped to make it read only by hiding the next, error, completed methods.
    /// </summary>
    /// <typeparam name="T">The type being exposed by the subject.</typeparam>
    public class ReadOnlyBehaviorObservable<T> : IReadOnlyObservable<T>
    {
        private readonly BehaviorSubject<T> _behaviorSubject;

        /// <summary>
        ///
        /// </summary>
        /// <param name="behaviorSubject"></param>
        public ReadOnlyBehaviorObservable(BehaviorSubject<T> behaviorSubject)
        {
            _behaviorSubject = behaviorSubject ?? throw new ArgumentNullException(nameof(behaviorSubject));
        }

        /// <inheritdoc />
        public T Value => _behaviorSubject.Value;

        /// <inheritdoc />
        public IDisposable Subscribe(IObserver<T> observer) => _behaviorSubject.Subscribe(observer);

        /// <inheritdoc />>
        public bool TryGetValue(out T value)
        {
            return _behaviorSubject.TryGetValue(out value);
        }
    }

    /// <summary>
    /// Extension Methods for <see cref="T:System.Reactive.Subjects.BehaviorSubject">BehaviorSubject</see>
    /// </summary>
    public static class BehaviorSubjectExtensions
    {
        /// <summary>
        /// Wraps a Behaviour Subject as a Read Only observable.
        /// </summary>
        /// <typeparam name="T">The type being exposed by the subject.</typeparam>
        /// <param name="behaviorSubject">The behavior subject to wrap and observe.</param>
        /// <returns></returns>
        public static ReadOnlyBehaviorObservable<T> ToReadOnlyBehaviorObservable<T>(this BehaviorSubject<T> behaviorSubject)
        {
            if (behaviorSubject == null)
            {
                throw new ArgumentNullException(nameof(behaviorSubject));
            }

            return new ReadOnlyBehaviorObservable<T>(behaviorSubject);
        }
    }
}
