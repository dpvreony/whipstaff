// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using ReactiveUI;
using ReactiveUI.Builder;

#if ARGUMENT_NULL_EXCEPTION_SHIM
using ArgumentNullException = Whipstaff.Runtime.Exceptions.ArgumentNullException;
#endif

namespace Whipstaff.ReactiveUI.Bootstrap.ContractBased
{
    /// <summary>
    /// Registration details for a view that is registered as a singleton.
    /// </summary>
    /// <typeparam name="TView">The type of the view.</typeparam>
    /// <typeparam name="TViewModel">The type of the view model to register. Must be a reference type that implements IReactiveObject and has a
    /// parameterless constructor.</typeparam>
    public sealed class SingletonViewRegistration<TView, TViewModel> : ISingletonViewRegistration
        where TView : class, IViewFor<TViewModel>, new()
        where TViewModel : class, IReactiveObject
    {
        /// <inheritdoc/>
        public void Register(IReactiveUIBuilder reactiveUiBuilder)
        {
            ArgumentNullException.ThrowIfNull(reactiveUiBuilder);

            _ = reactiveUiBuilder.RegisterSingletonView<TView, TViewModel>();
        }
    }
}
