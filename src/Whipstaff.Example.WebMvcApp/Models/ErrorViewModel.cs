// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Dhgms.AspNetCoreContrib.Example.WebMvcApp.Models
{
    /// <summary>
    /// View Model for rendering errors.
    /// </summary>
#pragma warning disable GR0005 // ViewModel classes should inherit from ReactiveUI.ReactiveObject.
#pragma warning disable GR0034 // ViewModel classes should inherit from a ViewModel interface.
    internal sealed class ErrorViewModel
#pragma warning restore GR0034 // ViewModel classes should inherit from a ViewModel interface.
#pragma warning restore GR0005 // ViewModel classes should inherit from ReactiveUI.ReactiveObject.
    {
        /// <summary>
        /// Gets or sets the request id.
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Gets a value indicating whether to show the request id.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
