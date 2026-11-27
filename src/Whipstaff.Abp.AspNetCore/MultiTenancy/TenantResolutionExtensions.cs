// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Whipstaff.Abp.AspNetCore.MultiTenancy
{
    /// <summary>Registers lookup-backed ABP tenant resolution.</summary>
    public static class TenantResolutionExtensions
    {
        /// <summary>Registers a host lookup database and replaces ABP's tenant contributors with the configured lookup.</summary>
        /// <param name="services">The application services.</param>
        /// <param name="configureDatabase">Configures the independent host lookup database provider.</param>
        /// <param name="source">The resolution source.</param>
        /// <param name="hostDomain">The allowed parent domain for host-prefix resolution.</param>
        /// <returns>The original service collection.</returns>
        public static IServiceCollection AddAbpLookupTenantResolution(
            this IServiceCollection services,
            Action<DbContextOptionsBuilder> configureDatabase,
            TenantLookupSource source,
            string? hostDomain = null)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configureDatabase);
            var contributor = new LookupTenantResolveContributor(source, hostDomain);
            _ = services.AddHttpContextAccessor();
            _ = services.AddDbContext<TenantLookupDbContext>(configureDatabase);
            _ = services.PostConfigure<AbpTenantResolveOptions>(options =>
            {
                options.TenantResolvers.Clear();
                options.TenantResolvers.Add(contributor);
                options.FallbackTenant = null;
            });
            return services;
        }
    }
}
