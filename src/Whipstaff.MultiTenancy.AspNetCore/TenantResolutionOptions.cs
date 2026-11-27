// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Whipstaff.MultiTenancy.AspNetCore
{
    /// <summary>Configuration for a single tenant lookup source.</summary>
    public sealed class TenantResolutionOptions
    {
        /// <summary>Gets or sets the lookup source.</summary>
        public TenantLookupSource Source { get; set; }

        /// <summary>Gets or sets the allowed parent DNS domain for host-prefix lookup.</summary>
        public string? HostDomain { get; set; }

        internal static void Validate(TenantLookupSource source, string? hostDomain)
        {
            if (!Enum.IsDefined(source) || source == TenantLookupSource.None)
            {
                throw new ArgumentOutOfRangeException(nameof(source));
            }

            if (source == TenantLookupSource.HostPrefix &&
                (string.IsNullOrWhiteSpace(hostDomain) || Uri.CheckHostName(hostDomain) != UriHostNameType.Dns))
            {
                throw new ArgumentException("A valid parent DNS domain is required for host-prefix resolution.", nameof(hostDomain));
            }
        }
    }
}
