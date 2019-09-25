using Dhgms.AspNetCoreContrib.Abstractions.Features.ApplicationStartup;

namespace Dhgms.AspNetCoreContrib.Example.WebSite.Features.Apm.ApplicationInsights
{
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.AspNetCore.Builder;

    public sealed class ApplicationInsightsApplicationStartHelper : IConfigureApplication
    {
        public void ConfigureApplication(IApplicationBuilder app)
        {
            var builder = TelemetryConfiguration.Active.TelemetryProcessorChainBuilder;
            builder.Use(next => new SignalRTelemetryProcessor(next));
            builder.Build();
        }
    }
}
