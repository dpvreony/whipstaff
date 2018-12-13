using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Example.WebSite.Features.ApplicationStartUp;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;

namespace Dhgms.AspNetCoreContrib.Example.WebSite.Features.Apm.ApplicationInsights
{
    public sealed class ApplicationInsightsApplicationStartHelper : IConfigureApplication
    {
        public void ConfigureApplication(IApplicationBuilder app)
        {
            var builder = TelemetryConfiguration.Active.TelemetryProcessorChainBuilder;
            builder.Use((next) => new SignalRTelemetryProcessor(next));
            builder.Build();
        }
    }
}
