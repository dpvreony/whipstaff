// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Whipstaff.MultiTenancy.AspNetCore
{
    /// <summary>Registers standalone tenant resolution.</summary>
    public static class TenantResolutionExtensions
    {
        /// <summary>Registers the lookup database and request-scoped tenant accessor.</summary>
        /// <param name="services">The application services.</param>
        /// <param name="configureDatabase">Configures the independent host lookup database.</param>
        /// <param name="source">The resolution source.</param>
        /// <param name="hostDomain">The allowed parent domain for host-prefix resolution.</param>
        /// <returns>The original service collection.</returns>
        public static IServiceCollection AddStandaloneTenantResolution(
            this IServiceCollection services,
            Action<DbContextOptionsBuilder> configureDatabase,
            TenantLookupSource source,
            string? hostDomain = null)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configureDatabase);
            TenantResolutionOptions.Validate(source, hostDomain);

            _ = services.AddDbContext<TenantLookupDbContext>(configureDatabase);
            _ = services.AddScoped<CurrentTenant>();
            _ = services.Configure<TenantResolutionOptions>(options =>
            {
                options.Source = source;
                options.HostDomain = hostDomain;
            });
            return services;
        }

        /// <summary>Adds tenant resolution to the request pipeline.</summary>
        /// <param name="app">The application builder.</param>
        /// <returns>The original application builder.</returns>
        public static IApplicationBuilder UseStandaloneTenantResolution(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app);
            return app.UseMiddleware<TenantResolutionMiddleware>();
        }
    }
}
