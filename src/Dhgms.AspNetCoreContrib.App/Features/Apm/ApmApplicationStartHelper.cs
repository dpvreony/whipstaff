namespace Dhgms.AspNetCoreContrib.Example.WebSite.Features.Apm
{
    using System;
    using Dhgms.AspNetCoreContrib.Example.WebSite.Features.Apm.ApplicationInsights;
    using Dhgms.AspNetCoreContrib.Example.WebSite.Features.Apm.Exceptionless;
    using Dhgms.AspNetCoreContrib.Example.WebSite.Features.Apm.HealthChecks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

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
