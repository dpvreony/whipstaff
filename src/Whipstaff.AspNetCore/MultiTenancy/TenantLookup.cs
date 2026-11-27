// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Finbuckle.MultiTenant.Abstractions;

namespace Whipstaff.AspNetCore.MultiTenancy
{
    /// <summary>A unique lookup key associated with a tenant.</summary>
    public sealed class TenantLookup
    {
        /// <summary>Gets or sets the lookup source.</summary>
        public TenantLookupSource Source { get; set; }

        /// <summary>Gets or sets the normalized lookup value.</summary>
        public required string Value { get; set; }

        /// <summary>Gets or sets the tenant's stable ID.</summary>
        public required string TenantId { get; set; }

        /// <summary>Gets or sets the associated tenant.</summary>
        public TenantInfo Tenant { get; set; } = null!;

        /// <summary>Normalizes an input for insertion into or querying of the lookup table.</summary>
        /// <param name="source">The lookup source.</param>
        /// <param name="value">The input value.</param>
        /// <returns>The normalized value.</returns>
        public static string Normalize(TenantLookupSource source, string value)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            value = value.Trim();
            return source switch
            {
                TenantLookupSource.EntraTenantId => Guid.Parse(value).ToString("D"),
                TenantLookupSource.Email => value.ToUpperInvariant(),
                TenantLookupSource.HostPrefix => value.ToLowerInvariant(),
                _ => throw new ArgumentOutOfRangeException(nameof(source)),
            };
        }
    }
}
