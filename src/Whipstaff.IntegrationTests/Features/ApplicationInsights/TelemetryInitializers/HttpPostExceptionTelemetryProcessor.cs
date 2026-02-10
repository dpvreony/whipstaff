// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using OpenTelemetry;
using OpenTelemetry.Trace;

namespace Whipstaff.IntegrationTests.Features.ApplicationInsights.TelemetryInitializers
{
    /// <summary>
    /// Telemetry Processor that checks for an exception when the HTTP Request body is unavailable due to it already being disposed.
    /// </summary>
    public sealed class HttpPostExceptionTelemetryProcessor : BaseProcessor<Activity>
    {
        private readonly TelemetryExceptionTracker _telemetryExceptionTracker;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpPostExceptionTelemetryProcessor"/> class.
        /// </summary>
        /// <param name="telemetryExceptionTracker">Telemetry Exception Tracker.</param>
        /// <param name="httpContextAccessor">Http Context accessor for the request.</param>
#pragma warning disable GR0027 // Constructor should have a logging framework instance as the final parameter.
        public HttpPostExceptionTelemetryProcessor(
            TelemetryExceptionTracker telemetryExceptionTracker,
            IHttpContextAccessor httpContextAccessor)
        {
            _telemetryExceptionTracker = telemetryExceptionTracker;
            _httpContextAccessor = httpContextAccessor;
        }
#pragma warning restore GR0027 // Constructor should have a logging framework instance as the final parameter.

        /// <inheritdoc/>
        public override void OnEnd(Activity activity)
        {
            if (activity == null)
            {
                return;
            }

            // Only process HTTP server activities (incoming requests)
            if (activity.Kind != ActivityKind.Server)
            {
                base.OnEnd(activity);
                return;
            }

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                base.OnEnd(activity);
                return;
            }

            var request = httpContext.Request;
            var body = request.Body;

            try
            {
                _ = body.ReadByte();
            }
#pragma warning disable CA1031
            catch (Exception exception)
#pragma warning restore CA1031
            {
                _telemetryExceptionTracker.TrackException(exception);

                // Add diagnostic information to the activity
                _ = activity.SetStatus(ActivityStatusCode.Error, "Request body was disposed");
                _ = activity.AddException(exception);
                _ = activity.SetTag("http.request.body.disposed", true);
            }

            base.OnEnd(activity);
        }
    }
}
