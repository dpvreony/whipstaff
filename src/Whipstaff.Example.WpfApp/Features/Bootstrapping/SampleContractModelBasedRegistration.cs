// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Whipstaff.ReactiveUI.Bootstrap;
using Whipstaff.ReactiveUI.Bootstrap.ContractBased;

namespace Whipstaff.Example.WpfApp.Features.Bootstrapping
{
    /// <summary>
    /// Sample contract model based registration for the example application.
    /// </summary>
    internal sealed class SampleContractModelBasedRegistration : AbstractContractModelBasedRegistration
    {
        /// <inheritdoc/>
        protected override IList<IConstantViewModelRegistration> ConstantViewModelRegistrations => [];

        /// <inheritdoc/>
        protected override IList<ISingletonViewRegistration> SingletonViewRegistrations => [];

        /// <inheritdoc/>
        protected override IList<ISingletonViewModelRegistration> SingletonViewModelRegistrations => [];

        /// <inheritdoc/>
        protected override IList<IViewRegistration> ViewRegistrations => [];

        /// <inheritdoc/>
        protected override IList<IViewModelRegistration> ViewModelRegistrations => [];
    }
}
