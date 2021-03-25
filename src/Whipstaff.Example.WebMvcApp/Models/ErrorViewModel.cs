// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Dhgms.AspNetCoreContrib.Examples.WebMvcApp.Models
{
    /// <summary>
    /// View Model for rendering errors.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets or sets the request id.
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Gets a value indicating whether to show the request id.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
