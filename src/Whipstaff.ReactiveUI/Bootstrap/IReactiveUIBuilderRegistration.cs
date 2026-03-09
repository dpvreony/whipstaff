// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveUI.Builder;

namespace Whipstaff.ReactiveUI.Bootstrap
{
    /// <summary>
    /// Represents the registration helper for working with <see cref="ReactiveUIBuilder"/>.
    /// </summary>
    public interface IReactiveUIBuilderRegistration
    {
        /// <summary>
        /// Carries out actions against the ReactiveUI builder.
        /// </summary>
        /// <param name="reactiveUiBuilder">ReactiveUI Builder instance to register against.</param>
        void ActOnBuilder(IReactiveUIBuilder reactiveUiBuilder);
    }
}
