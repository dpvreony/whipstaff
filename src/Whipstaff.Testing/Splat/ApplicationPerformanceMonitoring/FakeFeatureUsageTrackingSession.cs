// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Splat.ApplicationPerformanceMonitoring;

namespace Whipstaff.Testing.Splat.ApplicationPerformanceMonitoring
{
    /// <summary>
    /// Fake in memory implementation of <see cref="IFeatureUsageTrackingSession"/>.
    /// </summary>
    public sealed class FakeFeatureUsageTrackingSession : IFeatureUsageTrackingSession
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeFeatureUsageTrackingSession"/> class.
        /// </summary>
        /// <param name="featureName">Name of the feature.</param>
        public FakeFeatureUsageTrackingSession(string featureName)
        {
            FeatureName = featureName;
        }

        /// <inheritdoc/>
        public string FeatureName { get; }

        /// <inheritdoc/>
        public void Dispose()
        {
        }

        /// <inheritdoc/>
        public void OnException(Exception exception)
        {
        }

        /// <inheritdoc/>
        public IFeatureUsageTrackingSession SubFeature(string description)
        {
            return new FakeFeatureUsageTrackingSession(description);
        }
    }
}
