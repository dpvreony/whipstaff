// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Dhgms.AspNetCoreContrib.App.Features.Apm.ApplicationInsights;
using Dhgms.AspNetCoreContrib.App.Features.Apm.Exceptionless;
using Dhgms.AspNetCoreContrib.App.Features.Apm.HealthChecks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Dhgms.AspNetCoreContrib.App.Features.Apm
{
    public static class ApmApplicationStartHelper
    {
        public static void Configure(
            IConfiguration configuration,
            IApplicationBuilder app,
            IWebHostEnvironment env,
            Version version)
        {
            ExceptionlessApplicationStartHelper.Configure(configuration, app, env, version);
            new ApplicationInsightsApplicationStartHelper().ConfigureApplication(app);
            new HealthChecksApplicationStartHelper().ConfigureApplication(app);
        }
    }
}
