// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Finbuckle.MultiTenant.AspNetCore.Extensions;
using Finbuckle.MultiTenant.EntityFrameworkCore.Extensions;
using Finbuckle.MultiTenant.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Whipstaff.AspNetCore.MultiTenancy
{
    /// <summary>Registers lookup-backed Finbuckle tenant resolution.</summary>
    public static class TenantResolutionExtensions
    {
        /// <summary>Registers the lookup database, a single resolution strategy and the Finbuckle EF Core store.</summary>
        /// <param name="services">The application services.</param>
        /// <param name="configureDatabase">Configures the independent lookup database provider.</param>
        /// <param name="source">The resolution source to use for this application.</param>
        /// <param name="hostDomain">The allowed parent domain, required for host-prefix resolution.</param>
        /// <returns>The Finbuckle builder.</returns>
        public static MultiTenantBuilder<TenantInfo> AddLookupTenantResolution(
            this IServiceCollection services,
            Action<DbContextOptionsBuilder> configureDatabase,
            TenantLookupSource source,
            string? hostDomain = null)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configureDatabase);
            if (!Enum.IsDefined(source) || source == TenantLookupSource.None)
            {
                throw new ArgumentOutOfRangeException(nameof(source));
            }

            if (source == TenantLookupSource.HostPrefix &&
                (string.IsNullOrWhiteSpace(hostDomain) || Uri.CheckHostName(hostDomain) != UriHostNameType.Dns))
            {
                throw new ArgumentException("A valid parent DNS domain is required for host-prefix resolution.", nameof(hostDomain));
            }

            _ = services.AddDbContext<TenantLookupDbContext>(configureDatabase);
            return services.AddMultiTenant<TenantInfo>()
                .WithHttpContextStrategy(context => ResolveIdentifierAsync(context, source, hostDomain))
                .WithEFCoreStore<TenantLookupDbContext, TenantInfo>();
        }

        private static async Task<string?> ResolveIdentifierAsync(HttpContext context, TenantLookupSource source, string? hostDomain)
        {
            string? value;
            if (source == TenantLookupSource.HostPrefix)
            {
                var host = context.Request.Host.Host;
                var suffix = "." + hostDomain;
                if (!host.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }

                value = host.Substring(0, host.Length - suffix.Length);
                if (value.Length == 0 || value.Contains('.', StringComparison.Ordinal) || Uri.CheckHostName(value) != UriHostNameType.Dns)
                {
                    return null;
                }
            }
            else
            {
                var claimTypes = source == TenantLookupSource.EntraTenantId
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
                if (values.Length != 1 || (source == TenantLookupSource.EntraTenantId && !Guid.TryParse(values[0], out _)))
                {
                    return null;
                }

                value = values[0];
            }

            var normalized = TenantLookup.Normalize(source, value);
            var database = context.RequestServices.GetRequiredService<TenantLookupDbContext>();
            return await database.Lookups.AsNoTracking()
                .Where(lookup => lookup.Source == source && lookup.Value == normalized)
                .Select(lookup => lookup.Tenant.Identifier)
                .SingleOrDefaultAsync(context.RequestAborted)
                .ConfigureAwait(false);
        }
    }
}
