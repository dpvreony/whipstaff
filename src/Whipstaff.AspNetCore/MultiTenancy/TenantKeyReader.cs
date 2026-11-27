// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Whipstaff.MultiTenancy.Internal
{
    internal static class TenantKeyReader
    {
        internal static string? Read(HttpContext context, bool hostSource, bool entraSource, string? hostDomain)
        {
            if (hostSource)
            {
                var host = context.Request.Host.Host;
                var suffix = "." + hostDomain;
                if (!host.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }

                var prefix = host.Substring(0, host.Length - suffix.Length);
                return prefix.Length == 0 || prefix.Contains('.', StringComparison.Ordinal) || Uri.CheckHostName(prefix) != UriHostNameType.Dns
                    ? null
                    : prefix;
            }

            var claimTypes = entraSource
                ? new[] { "tid", "http://schemas.microsoft.com/identity/claims/tenantid" }
                : new[] { "email", ClaimTypes.Email };
            var values = context.User.Identities
                .Where(identity => identity.IsAuthenticated)
                .SelectMany(identity => identity.Claims)
                .Where(claim => claimTypes.Contains(claim.Type, StringComparer.Ordinal) && !string.IsNullOrWhiteSpace(claim.Value))
                .Select(claim => claim.Value.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Take(2)
                .ToArray();
            return values.Length != 1 || (entraSource && !Guid.TryParse(values[0], out _)) ? null : values[0];
        }

        internal static string Normalize(string value, bool hostSource, bool entraSource)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            value = value.Trim();
            if (entraSource)
            {
                return Guid.Parse(value).ToString("D");
            }

            return hostSource ? value.ToLowerInvariant() : value.ToUpperInvariant();
        }
    }
}
