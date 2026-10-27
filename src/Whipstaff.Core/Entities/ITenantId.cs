// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Core.Entities
{
    /// <summary>
    /// Represents an object that has a tenant identifier.
    /// </summary>
    /// <typeparam name="T">The type for the TenantId.</typeparam>
    public interface ITenantId<out T>
        where T : struct
    {
        /// <summary>
        /// Gets the unique id for the tenant.
        /// </summary>
        T TenantId { get; }
    }
}
