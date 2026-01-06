// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Linq;
using System.Reflection;
using Whipstaff.Runtime.AppDomains;

#if ARGUMENT_NULL_EXCEPTION_SHIM
using ArgumentNullException = Whipstaff.Runtime.Exceptions.ArgumentNullException;
#endif

namespace Whipstaff.Rx.Runtime.AppDomains
{
    /// <summary>
    /// Extension methods for <see cref="IAssemblyResolveHelper"/>.
    /// </summary>
#pragma warning disable GR0049 // Do not use Tuples.
    public static class AssemblyResolverExtensions
    {
        /// <summary>
        /// Creates an observable that will handle the <see cref="AppDomain.AssemblyResolve"/> event.
        /// </summary>
        /// <param name="assemblyResolveHelper">Assembly resolver helper to bind.</param>
        /// <returns>Observable for <see cref="AppDomain.AssemblyResolve"/>.</returns>
        public static IObservable<(ResolveEventArgs Args, Assembly? Assembly)> ToObservable(this IAssemblyResolveHelper assemblyResolveHelper)
        {
            ArgumentNullException.ThrowIfNull(assemblyResolveHelper);

            return Observable.Create<(ResolveEventArgs Args, Assembly? Assembly)>(observer =>
            {
                // The handler will capture the message and produce a return value
                ResolveEventHandler handler = (sender, args) =>
                {
                    // Notify the observer with the message and the return value
                    var result = assemblyResolveHelper.OnAssemblyResolve(sender, args);
                    observer.OnNext((args, result));

                    return null; // return this value to the event invoker
                };

                AppDomain.CurrentDomain.AssemblyResolve += handler;

                // Return a disposable that unsubscribes from the event when disposed
                return () => AppDomain.CurrentDomain.AssemblyResolve -= handler;
            });
        }
    }
#pragma warning restore GR0049 // Do not use Tuples.
}
