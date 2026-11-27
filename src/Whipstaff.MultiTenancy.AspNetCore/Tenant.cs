// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Whipstaff.MultiTenancy.AspNetCore
{
    /// <summary>A tenant stored in the host lookup database.</summary>
    public sealed class Tenant
    {
        /// <summary>Gets or sets the stable tenant ID.</summary>
        public Guid Id { get; set; }

        /// <summary>Gets or sets the tenant's display name.</summary>
        public required string Name { get; set; }

        /// <summary>Gets or sets a value indicating whether the tenant can be resolved.</summary>
        public bool IsActive { get; set; } = true;
    }
}
