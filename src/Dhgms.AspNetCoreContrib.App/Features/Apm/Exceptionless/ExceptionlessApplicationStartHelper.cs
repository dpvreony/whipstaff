using Microsoft.Extensions.Hosting;

namespace Dhgms.AspNetCoreContrib.Example.WebSite.Features.Apm.Exceptionless
{
    using System;
    using global::Exceptionless;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    public static class ExceptionlessApplicationStartHelper
    {
        public static void Configure(
            IConfiguration configuration,
            IApplicationBuilder app,
            IWebHostEnvironment env,
            Version version)
        {
            var exceptionlessApiKey = configuration.GetValue<string>("Exceptionless:ApiKey", null);

            if (string.IsNullOrWhiteSpace(exceptionlessApiKey))
            {
                return;
            }

            var exceptionlessClient = ExceptionlessClient.Default;
            DoExceptionLessConfiguration(
                env,
                exceptionlessClient,
                version);
            app.UseExceptionless(exceptionlessClient);
        }

        private static void DoExceptionLessConfiguration(
            IWebHostEnvironment env,
            ExceptionlessClient exceptionlessClient,
            Version version)
        {
            var exceptionlessConfiguration = exceptionlessClient.Configuration;
            exceptionlessConfiguration.SetVersion(version);
            exceptionlessConfiguration.Enabled = !env.IsDevelopment();
        }
    }
}
