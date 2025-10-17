// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

namespace Whipstaff.Aspire.Hosting.HealthChecksUI
{
    /// <summary>
    /// A container-based resource for the HealthChecksUI container.
    /// See https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks?tab=readme-ov-file#HealthCheckUI for more details.
    /// </summary>
    /// <param name="name">The resource name.</param>
    public class HealthChecksUIResource(string name) : ContainerResource(name), IResourceWithServiceDiscovery
    {
        /// <summary>
        /// Gets the projects to be monitored by the HealthChecksUI container.
        /// </summary>
        public IList<MonitoredProject> MonitoredProjects { get; } = [];

#pragma warning disable Inclusive // Use inclusive words
        /// <summary>
        /// Known environment variables for the HealthChecksUI container that can be used to configure the container.
        /// See https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks/blob/master/doc/ui-docker.md#environment-variables-table for more details.
        /// </summary>
#pragma warning disable CA1034 // Nested types should not be visible
        public static class KnownEnvVars
#pragma warning restore Inclusive // Use inclusive words
#pragma warning restore CA1034 // Nested types should not be visible
        {
            /// <summary>
            /// The path the HealthChecksUI container will be served from.
            /// </summary>
            public const string UiPath = "ui_path";

            /// <summary>
            /// The configuration section for the HealthChecksUI container.
            /// </summary>
            /// <remarks>
            /// See https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks?tab=readme-ov-file#sample-2-configuration-using-appsettingsjson for more details.
            /// </remarks>
            public const string HealthChecksConfigSection = "HealthChecksUI__HealthChecks";

            /// <summary>
            /// The name of the health check.
            /// </summary>
            public const string HealthCheckName = "Name";

            /// <summary>
            /// The URI of the health check.
            /// </summary>
            public const string HealthCheckUri = "Uri";

            internal static string GetHealthCheckNameKey(int index) => $"{HealthChecksConfigSection}__{index}__{HealthCheckName}";

            internal static string GetHealthCheckUriKey(int index) => $"{HealthChecksConfigSection}__{index}__{HealthCheckUri}";
        }
    }
}
