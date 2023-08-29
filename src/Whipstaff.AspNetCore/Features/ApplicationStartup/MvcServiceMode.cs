// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.AspNetCore.Features.ApplicationStartup
{
    /// <summary>
    /// Represents the mode of MVC services to add.
    /// </summary>
    public enum MvcServiceMode
    {
        /// <summary>
        /// No MVC services.
        /// </summary>
        None,

        /// <summary>
        /// Basic MVC services.
        /// </summary>
        Basic,

        /// <summary>
        /// Controllers with views.
        /// </summary>
        ControllersWithViews,
    }
}
