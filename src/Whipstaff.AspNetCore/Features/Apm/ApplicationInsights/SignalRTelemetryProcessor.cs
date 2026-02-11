// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using OpenTelemetry;

namespace Whipstaff.AspNetCore.Features.Apm.ApplicationInsights
{
    /// <summary>
    /// Telemetry processor for reducing the response times on SignalR requests. Done because otherwise it makes observability tools
    /// suggest that you have slow http requests taking place. Skews reporting and leaves a false positive on the screen that
    /// doesn't need investigating.
    /// </summary>
    public class SignalRTelemetryProcessor : BaseProcessor<Activity>
    {
        private readonly string[] _urlPaths =
        {
            "/signalr/poll",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="SignalRTelemetryProcessor"/> class.
        /// </summary>
#pragma warning disable GR0027 // Constructor should have a logging framework instance as the final parameter.
        public SignalRTelemetryProcessor()
        {
        }
#pragma warning restore GR0027 // Constructor should have a logging framework instance as the final parameter.

        /// <inheritdoc />
        public override void OnEnd(Activity activity)
        {
            if (activity == null)
            {
                return;
            }

            try
            {
                // Only process HTTP request activities
                if (activity.Kind == ActivityKind.Server)
                {
                    var url = activity.GetTagItem("url.path") as string
                              ?? activity.GetTagItem("http.target") as string;

                    if (url != null && Array.Exists(_urlPaths, urlPath => url.StartsWith(urlPath, StringComparison.OrdinalIgnoreCase)))
                    {
                        // Set duration to near-zero by adjusting the end time
                        _ = activity.SetEndTime(activity.StartTimeUtc);

                        // Optionally add a tag to indicate this was adjusted
                        _ = activity.SetTag("telemetry.adjusted", "signalr_duration_zeroed");
                    }
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch
#pragma warning restore CA1031 // Do not catch general exception types
            {
                // no op - don't let processor failures break telemetry pipeline
            }

            base.OnEnd(activity);
        }
    }
}
