using System;
using System.Reactive.Subjects;

namespace Whipstaff.Rx
{
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
}
