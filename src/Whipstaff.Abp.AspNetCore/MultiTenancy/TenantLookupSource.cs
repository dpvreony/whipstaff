// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Abp.AspNetCore.MultiTenancy
{
    /// <summary>The source of a tenant lookup key.</summary>
    public enum TenantLookupSource
    {
        /// <summary>No source is configured.</summary>
        None = 0,

        /// <summary>The authenticated Microsoft Entra tenant ID.</summary>
        EntraTenantId = 1,

        /// <summary>The authenticated user's email claim.</summary>
        Email = 2,

        /// <summary>The first label of an allowed request hostname.</summary>
        HostPrefix = 3,
    }
}
