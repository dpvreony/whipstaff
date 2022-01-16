using System;

namespace Whipstaff.Rx.ReadOnlyObservables
{
    /// <summary>
    /// Represents a <see cref="System.Reactive.Subjects.BehaviorSubject{T}">BehaviorSubject</see> that has been wrapped to make it read only by hiding the next, error, completed methods.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadOnlyObservable<T> : IObservable<T>
    {
        /// <summary>
        /// Gets the value from the subject.
        /// </summary>
        T Value { get; }

        /// <summary>
        /// Tries to get the value from the subject.
        /// </summary>
        /// <param name="value">The output value.</param>
        /// <returns>A flag indicating whether the Get attempt succeeded.</returns>
        bool TryGetValue(out T value);
    }
}
