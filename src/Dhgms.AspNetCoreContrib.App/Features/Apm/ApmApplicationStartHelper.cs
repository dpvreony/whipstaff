// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Dhgms.AspNetCoreContrib.Abstractions.Features.ApplicationStartup;
using Dhgms.AspNetCoreContrib.App.Features.Apm.ApplicationInsights;
using Dhgms.AspNetCoreContrib.App.Features.Apm.Exceptionless;
using Dhgms.AspNetCoreContrib.App.Features.Apm.HealthChecks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dhgms.AspNetCoreContrib.App.Features.Apm
{
    public class ApmApplicationStartHelper : IConfigureService
    {
        public static void Configure(
            IConfiguration configuration,
            IApplicationBuilder app,
            Version version)
        {
            ExceptionlessApplicationStartHelper.Configure(configuration, app, version);
            new HealthChecksApplicationStartHelper().ConfigureApplication(app);
        }

        /// <inheritdoc/>
        public void ConfigureService(
            IServiceCollection services,
            IConfiguration configuration)
        {
            new ApplicationInsightsApplicationStartHelper().ConfigureService(services);
        }
    }
}
