// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using HealthChecks.UI.Client;
using HealthChecks.UI.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Whipstaff.AspNetCore.Features.ApplicationStartup;

namespace Whipstaff.AspNetCore.Features.Apm.HealthChecks
{
    /// <summary>
    /// Initialization logic for Health Checks.
    /// </summary>
    public sealed class HealthChecksApplicationStartHelper : IConfigureService, IConfigureApplication
    {
        /// <inheritdoc />
        public void ConfigureService(
            IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddApplicationInsightsPublisher();
        }

        /// <inheritdoc />
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
