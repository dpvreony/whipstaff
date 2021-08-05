// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Whipstaff.AspNetCore.Features.Apm.ApplicationInsights;
using Whipstaff.AspNetCore.Features.Apm.HealthChecks;
using Whipstaff.Core.ApplicationStartup;

namespace Whipstaff.AspNetCore.Features.Apm
{
    /// <summary>
    /// Initialization logic for Application Performance Monitoring.
    /// </summary>
    public class ApmApplicationStartHelper : IConfigureService, IConfigureApplication
    {
        /// <inheritdoc/>
        public void ConfigureService(
            IServiceCollection services,
            IConfiguration configuration)
        {
            new ApplicationInsightsApplicationStartHelper()
                .ConfigureService(services, configuration);
        }

        /// <inheritdoc/>
        public void ConfigureApplication(IApplicationBuilder app)
        {
            // ExceptionlessApplicationStartHelper.Configure(configuration, app, version);
            new HealthChecksApplicationStartHelper().ConfigureApplication(app);
        }
    }
}
