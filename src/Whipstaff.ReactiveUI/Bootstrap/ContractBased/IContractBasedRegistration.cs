// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveUI.Builder;

namespace Whipstaff.ReactiveUI.Bootstrap.ContractBased
{
    /// <summary>
    /// Represents a registration to be carried out against the ReactiveUI builder.
    /// </summary>
    public interface IContractBasedRegistration
    {
        /// <summary>
        /// Carries out the registration of the view model as a constant.
        /// </summary>
        /// <param name="reactiveUiBuilder">ReactiveUI Builder instance to register against.</param>
        void Register(IReactiveUIBuilder reactiveUiBuilder);
    }
}
