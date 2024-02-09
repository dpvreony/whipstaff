// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if TBC
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Whipstaff.AspNetCore.Features.ApplicationStartup;

namespace Whipstaff.AspNetCore.Features.StartUp
{
    /// <summary>
    /// Start Up Helper for Mini Profiler.
    /// </summary>
    /// <remarks>
    /// Based upon: https://miniprofiler.com/dotnet/AspDotNetCore documentation.
    /// </remarks>
    public class MiniProfilerApplicationStartHelper : IConfigureService, IConfigureApplication
    {
        /// <inheritdoc/>
        public void ConfigureService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMiniProfiler(options =>
            {
                // All of this is optional. You can simply call .AddMiniProfiler() for all defaults

                // (Optional) Path to use for profiler URLs, default is /mini-profiler-resources
                // options.RouteBasePath = "/profiler";

                // (Optional) Control storage
                // (default is 30 minutes in MemoryCacheStorage)
                // (options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(60);

                // (Optional) Control which SQL formatter to use, InlineFormatter is the default
                // options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();

                // (Optional) To control authorization, you can use the Func<HttpRequest, bool> options:
                // (default is everyone can access profilers)
                // options.ResultsAuthorize = request => MyGetUserFunction(request).CanSeeMiniProfiler;
                // options.ResultsListAuthorize = request => MyGetUserFunction(request).CanSeeMiniProfiler;

                // (Optional)  To control which requests are profiled, use the Func<HttpRequest, bool> option:
                // (default is everything should be profiled)
                // options.ShouldProfile = request => MyShouldThisBeProfiledFunction(request);

                // (Optional) Profiles are stored under a user ID, function to get it:
                // (default is null, since above methods don't use it by default)
                // options.UserIdProvider =  request => MyGetUserIdFunction(request);

                // (Optional) Swap out the entire profiler provider, if you want
                // (default handles async and works fine for almost all appliations)
                // options.ProfilerProvider = new MyProfilerProvider();

                // (Optional) You can disable "Connection Open()", "Connection Close()" (and async variant) tracking.
                // (defaults to true, and connection opening/closing is tracked)
                // options.TrackConnectionOpenClose = true;
            });
        }

        /// <inheritdoc/>
        public void ConfigureApplication(IApplicationBuilder app)
        {
            app.UseMiniProfiler();
        }
    }
}
#endif
