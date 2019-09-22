namespace Dhgms.AspNetCoreContrib.Example.WebSite.Features.Apm.ApplicationInsights
{
    using System;
    using System.Linq;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;

    public class SignalRTelemetryProcessor : ITelemetryProcessor
    {
        private readonly ITelemetryProcessor _nextTelemetryProcessor;

        private string[] _urlPaths = new[]
        {
            "/signalr/poll",
        };

        public SignalRTelemetryProcessor(ITelemetryProcessor next)
        {
            this._nextTelemetryProcessor = next;
        }

        public void Process(ITelemetry item)
        {
            try
            {
                if (item is RequestTelemetry request)
                {
                    var url = request.Url;

                    if (url != null && this._urlPaths.Any(urlPath => url.AbsolutePath.StartsWith(urlPath, StringComparison.OrdinalIgnoreCase)))
                    {
                        request.Duration = TimeSpan.Zero;
                    }
                }
            }
#pragma warning disable CC0004 // Catch block cannot be empty
            catch
            {
                // no op
            }
#pragma warning restore CC0004 // Catch block cannot be empty

            this._nextTelemetryProcessor.Process(item);
        }
    }
}
