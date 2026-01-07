// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Whipstaff.AspNetCore.Features.Apm.ApplicationInsights
{
    /// <summary>
    /// Telemetry processor for reducing the response times on SignalR requests. Done because otherwise it makes application insights
    /// suggest that you have slow http requests taking place. Skews reporting and leaves a false positive on the screen that
    /// doesn't need investigating.
    /// </summary>
    public class SignalRTelemetryProcessor : ITelemetryProcessor
    {
        private readonly ITelemetryProcessor _nextTelemetryProcessor;

        private readonly string[] _urlPaths =
        {
            "/signalr/poll",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="SignalRTelemetryProcessor"/> class.
        /// </summary>
        /// <param name="next">The next telemetry processor in the chain.</param>
#pragma warning disable GR0027 // Constructor should have a logging framework instance as the final parameter.
        public SignalRTelemetryProcessor(ITelemetryProcessor next)
        {
            _nextTelemetryProcessor = next;
        }
#pragma warning restore GR0027 // Constructor should have a logging framework instance as the final parameter.

        /// <inheritdoc />
        public void Process(ITelemetry item)
        {
            try
            {
                if (item is RequestTelemetry request)
                {
                    var url = request.Url;

                    if (url != null && Array.Exists(_urlPaths, urlPath => url.AbsolutePath.StartsWith(urlPath, StringComparison.OrdinalIgnoreCase)))
                    {
                        request.Duration = TimeSpan.Zero;
                    }
                }
            }
#pragma warning disable CC0004 // Catch block cannot be empty
#pragma warning disable CA1031 // Do not catch general exception types
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore CA1031 // Do not catch general exception types
            {
                // no op
            }
#pragma warning restore CC0004 // Catch block cannot be empty

            _nextTelemetryProcessor.Process(item);
        }
    }
}
