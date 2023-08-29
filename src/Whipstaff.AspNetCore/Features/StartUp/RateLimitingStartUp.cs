// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Whipstaff.AspNetCore.Features.ApplicationStartup;

namespace Whipstaff.AspNetCore.Features.StartUp
{
    /// <summary>
    /// Initialization logic for Rate Limiting.
    /// </summary>
    public sealed class RateLimitingStartUp : IConfigureService, IConfigureApplication
    {
        /// <inheritdoc />
        public void ConfigureService(
            IServiceCollection services,
            IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configuration);

            // needed to load configuration from appsettings.json
            _ = services.AddOptions();

            // needed to store rate limit counters and ip rules
            _ = services.AddMemoryCache();

            // load general configuration from appsettings.json
            _ = services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));

            // load ip rules from appsettings.json
            _ = services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));

            var useDistributedCache = false;

            // inject counter and rules stores
            if (useDistributedCache)
            {
                _ = services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();
                _ = services.AddSingleton<IRateLimitCounterStore, DistributedCacheRateLimitCounterStore>();
            }
            else
            {
                _ = services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
                _ = services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            }
        }

        /// <inheritdoc />
        public void ConfigureApplication(IApplicationBuilder app)
        {
            _ = app.UseIpRateLimiting();
        }
    }
}
