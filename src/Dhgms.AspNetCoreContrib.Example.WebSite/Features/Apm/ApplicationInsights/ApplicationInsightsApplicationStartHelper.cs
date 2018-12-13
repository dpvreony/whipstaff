using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Extensibility;

namespace Dhgms.AspNetCoreContrib.Example.WebSite.Features.Apm.ApplicationInsights
{
    public static class ApplicationInsightsApplicationStartHelper
    {
        public static void Configure()
        {
            var builder = TelemetryConfiguration.Active.TelemetryProcessorChainBuilder;
            builder.Use((next) => new SignalRTelemetryProcessor(next));
            builder.Build();
        }
    }
}
