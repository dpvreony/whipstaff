// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Trace;
using Whipstaff.AspNetCore.Features.ApplicationStartup;

namespace Whipstaff.AspNetCore.Features.Apm.ApplicationInsights
{
    /// <summary>
    /// Initialization logic for Application Insights.
    /// </summary>
    public sealed class ApplicationInsightsApplicationStartHelper : IConfigureService
    {
        /// <inheritdoc />
        public void ConfigureService(
            IServiceCollection services,
            IConfiguration configuration)
        {
            _ = services.AddOpenTelemetry()
                .WithTracing(builder => builder
                    .AddAspNetCoreInstrumentation()
                    .AddProcessor<SignalRTelemetryProcessor>());

            _ = services.AddApplicationInsightsTelemetry();

            _ = services.AddServiceProfiler();
        }
    }
}
