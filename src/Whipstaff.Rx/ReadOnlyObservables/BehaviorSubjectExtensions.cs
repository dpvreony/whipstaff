using System;
using System.Reactive.Subjects;

namespace Whipstaff.Rx
{
    /// <summary>
    /// Extension Methods for <see cref="System.Reactive.Subjects.BehaviorSubject{T}">BehaviorSubject</see>
    /// </summary>
    public static class BehaviorSubjectExtensions
    {
        /// <summary>
        /// Wraps a Behaviour Subject as a Read Only observable.
        /// </summary>
        /// <typeparam name="T">The type being exposed by the subject.</typeparam>
        /// <param name="behaviorSubject">The behavior subject to wrap and observe.</param>
        /// <returns>Readonly observable wrapper.</returns>
        public static IReadOnlyObservable<T> ToReadOnlyObservable<T>(this BehaviorSubject<T> behaviorSubject)
        {
            if (behaviorSubject == null)
            {
                throw new ArgumentNullException(nameof(behaviorSubject));
            }

            return new ReadOnlyBehaviorObservable<T>(behaviorSubject);
        }
    }
}
