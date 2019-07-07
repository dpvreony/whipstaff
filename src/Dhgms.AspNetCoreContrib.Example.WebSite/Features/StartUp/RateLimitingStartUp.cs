using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dhgms.AspNetCoreContrib.Example.WebSite.Features.StartUp
{
    public sealed class RateLimitingStartUp
    {
        public void OnConfigureServices(
            IServiceCollection services,
            IConfiguration configuration)
        {
            // needed to load configuration from appsettings.json
            services.AddOptions();

            // needed to store rate limit counters and ip rules
            services.AddMemoryCache();

            //load general configuration from appsettings.json
            services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));

            //load ip rules from appsettings.json
            services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));

            var useDistributedCache = false;
            // inject counter and rules stores
            if (useDistributedCache)
            {
                services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();
                services.AddSingleton<IRateLimitCounterStore, DistributedCacheRateLimitCounterStore>();
            }
            else
            {
                services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
                services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            }
        }

        public void OnConfigure(IApplicationBuilder app)
        {
            app.UseIpRateLimiting();
        }
    }
}
