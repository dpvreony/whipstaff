// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Whipstaff.IntegrationTests.Features.ApplicationInsights.TelemetryInitializers
{
    /// <summary>
    /// Stub to track exceptions happening in telemetry initializers.
    /// </summary>
    public sealed class TelemetryExceptionTracker
    {
        /// <summary>
        /// Gets the number of invocations.
        /// </summary>
        public int InvokeCount { get; private set; }

        /// <summary>
        /// Gets the exceptions.
        /// </summary>
        public ICollection<Exception> Exceptions { get; } = new List<Exception>();

        /// <summary>
        /// Tracks exceptions in the telemetry initializers.
        /// </summary>
        /// <param name="exception">The exception that occured.</param>
        public void TrackException(Exception exception)
        {
            InvokeCount++;
            Exceptions.Add(exception);
        }
    }
}
