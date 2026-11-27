// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Finbuckle.MultiTenant.AspNetCore.Extensions;
using Finbuckle.MultiTenant.EntityFrameworkCore.Extensions;
using Finbuckle.MultiTenant.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Whipstaff.MultiTenancy.Internal;

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
            var value = TenantKeyReader.Read(context, source == TenantLookupSource.HostPrefix, source == TenantLookupSource.EntraTenantId, hostDomain);
            if (value == null)
            {
                return null;
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
