// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Dhgms.AspNetCoreContrib.Abstractions.Features.ApplicationStartup;
using HealthChecks.UI.Client;
using HealthChecks.UI.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

namespace Dhgms.AspNetCoreContrib.App.Features.Apm.HealthChecks
{
    public sealed class HealthChecksApplicationStartHelper : IConfigureService, IConfigureApplication
    {
        public void ConfigureService(IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddApplicationInsightsPublisher();
        }

        public void ConfigureApplication(IApplicationBuilder app)
        {
            var healthCheckOptions = new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            };

            app.UseHealthChecks("/hc", healthCheckOptions);
            app.UseHealthChecksUI(SetupHealthChecksUi);
        }

        private static void SetupHealthChecksUi(Options setup)
        {
            setup.ApiPath = "/hc";
            setup.UIPath = "/healthcheckui";
        }
    }
}
