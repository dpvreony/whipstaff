// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Whipstaff.MultiTenancy.AspNetCore
{
    /// <summary>Exposes the tenant resolved for the current request scope.</summary>
    public sealed class CurrentTenant
    {
        /// <summary>Gets the resolved tenant ID, or null outside tenant middleware.</summary>
        public Guid? Id { get; internal set; }

        /// <summary>Gets the resolved tenant display name, or null outside tenant middleware.</summary>
        public string? Name { get; internal set; }
    }
}
