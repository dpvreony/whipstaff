// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using ReactiveUI.Builder;
using Whipstaff.ReactiveUI.Bootstrap.ContractBased;

namespace Whipstaff.ReactiveUI.Bootstrap
{
    /// <summary>
    /// Details the registration information to be used to set up ReactiveUI.
    /// </summary>
    public abstract class AbstractContractModelBasedRegistration : IReactiveUIBuilderRegistration
    {
        /// <summary>
        /// Gets a list of view model registrations to configure as constants.
        /// </summary>
        protected abstract IList<IConstantViewModelRegistration> ConstantViewModelRegistrations { get; }

        /// <summary>
        /// Gets a list of view registrations to configure as singleton.
        /// </summary>
        protected abstract IList<ISingletonViewRegistration> SingletonViewRegistrations { get; }

        /// <summary>
        /// Gets a list of view model registrations to configure.
        /// </summary>
        protected abstract IList<ISingletonViewModelRegistration> SingletonViewModelRegistrations { get; }

        /// <summary>
        /// Gets a list of view registrations to configure.
        /// </summary>
        protected abstract IList<IViewRegistration> ViewRegistrations { get; }

        /// <summary>
        /// Gets a list of view model registrations to configure.
        /// </summary>
        protected abstract IList<IViewModelRegistration> ViewModelRegistrations { get; }

        /// <inheritdoc/>
        public void ActOnBuilder(IReactiveUIBuilder reactiveUiBuilder)
        {
            foreach (var registration in ConstantViewModelRegistrations)
            {
                registration.Register(reactiveUiBuilder);
            }

            foreach (var registration in SingletonViewRegistrations)
            {
                registration.Register(reactiveUiBuilder);
            }

            foreach (var registration in SingletonViewModelRegistrations)
            {
                registration.Register(reactiveUiBuilder);
            }

            foreach (var registration in ViewRegistrations)
            {
                registration.Register(reactiveUiBuilder);
            }

            foreach (var registration in ViewModelRegistrations)
            {
                registration.Register(reactiveUiBuilder);
            }
        }
    }
}
