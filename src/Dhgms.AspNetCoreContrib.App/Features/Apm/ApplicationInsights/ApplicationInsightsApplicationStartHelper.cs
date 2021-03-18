// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Dhgms.AspNetCoreContrib.Abstractions.Features.ApplicationStartup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dhgms.AspNetCoreContrib.App.Features.Apm.ApplicationInsights
{
    /// <summary>
    /// Initialization logic for Application Insights.
    /// </summary>
    public sealed class ApplicationInsightsApplicationStartHelper : Abstractions.Features.ApplicationStartup.IConfigureService
    {
        /// <inheritdoc />
        public void ConfigureService(
            IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddApplicationInsightsTelemetryProcessor<SignalRTelemetryProcessor>();
        }
    }
}
