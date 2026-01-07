// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;

namespace Whipstaff.IntegrationTests.Features.ApplicationInsights.TelemetryInitializers
{
    /// <summary>
    /// Telemetry Initializer that checks for an exception when the HTTP Request body is unavailable due to it already being disposed.
    /// </summary>
    public sealed class HttpPostExceptionTelemetryInitializer : ITelemetryInitializer
    {
        private readonly TelemetryExceptionTracker _telemetryExceptionTracker;
        private readonly HttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpPostExceptionTelemetryInitializer"/> class.
        /// </summary>
        /// <param name="telemetryExceptionTracker">Telemetry Exception Tracker.</param>
        /// <param name="httpContextAccessor">Http Context accessor for the request.</param>
#pragma warning disable GR0027 // Constructor should have a logging framework instance as the final parameter.
        public HttpPostExceptionTelemetryInitializer(TelemetryExceptionTracker telemetryExceptionTracker, HttpContextAccessor httpContextAccessor)
        {
            _telemetryExceptionTracker = telemetryExceptionTracker;
            _httpContextAccessor = httpContextAccessor;
        }
#pragma warning restore GR0027 // Constructor should have a logging framework instance as the final parameter.

        /// <inheritdoc/>
        public void Initialize(ITelemetry telemetry)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
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
            }
        }
    }
}
