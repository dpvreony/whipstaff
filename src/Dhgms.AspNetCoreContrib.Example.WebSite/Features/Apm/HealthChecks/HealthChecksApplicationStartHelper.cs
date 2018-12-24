namespace Dhgms.AspNetCoreContrib.Example.WebSite.Features.Apm.HealthChecks
{
    using Dhgms.AspNetCoreContrib.Example.WebSite.Features.ApplicationStartUp;
    using global::HealthChecks.UI.Client;
    using global::HealthChecks.UI.Configuration;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.Extensions.DependencyInjection;

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
