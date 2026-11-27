// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Whipstaff.MultiTenancy.Internal;

namespace Whipstaff.Abp.AspNetCore.MultiTenancy
{
    /// <summary>A lookup key associated with an ABP tenant ID.</summary>
    public sealed class TenantLookup
    {
        /// <summary>Gets or sets the lookup source.</summary>
        public TenantLookupSource Source { get; set; }

        /// <summary>Gets or sets the normalized lookup value.</summary>
        public required string Value { get; set; }

        /// <summary>Gets or sets the tenant ID from the ABP tenant store.</summary>
        public Guid TenantId { get; set; }

        /// <summary>Normalizes an input for insertion into or querying of the lookup table.</summary>
        /// <param name="source">The lookup source.</param>
        /// <param name="value">The input value.</param>
        /// <returns>The normalized value.</returns>
        public static string Normalize(TenantLookupSource source, string value)
        {
            if (!Enum.IsDefined(source) || source == TenantLookupSource.None)
            {
                throw new ArgumentOutOfRangeException(nameof(source));
            }

            return TenantKeyReader.Normalize(value, source == TenantLookupSource.HostPrefix, source == TenantLookupSource.EntraTenantId);
        }
    }
}
