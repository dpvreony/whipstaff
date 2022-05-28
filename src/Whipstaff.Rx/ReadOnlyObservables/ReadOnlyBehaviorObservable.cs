// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Subjects;

namespace Whipstaff.Rx.ReadOnlyObservables
{
    /// <summary>
    /// Represents a <see cref="System.Reactive.Subjects.BehaviorSubject{T}">BehaviorSubject</see> that has been wrapped to make it read only by hiding the next, error, completed methods.
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
            value = default!;

            try
            {
                return _behaviorSubject.TryGetValue(out value!);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return false;
            }
        }
    }
}
