// Copyright (c) 2021 - 2024 DHGMS Solutions and Contributors. All rights reserved.
// DHGMS Solutions and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Aspire.Hosting.HealthChecksUI
{
    /// <summary>
    /// Default values used by <see cref="HealthChecksUIResource" />.
    /// </summary>
    public static class HealthChecksUIDefaults
    {
        /// <summary>
        /// The default container registry to pull the HealthChecksUI container image from.
        /// </summary>
        public const string ContainerRegistry = "docker.io";

        /// <summary>
        /// The default container image name to use for the HealthChecksUI container.
        /// </summary>
        public const string ContainerImageName = "xabarilcoding/healthchecksui";

        /// <summary>
        /// The default container image tag to use for the HealthChecksUI container.
        /// </summary>
        public const string ContainerImageTag = "5.0.0";

        /// <summary>
        /// The target port the HealthChecksUI container listens on.
        /// </summary>
        public const int ContainerPort = 80;

        /// <summary>
        /// The default request path projects serve health check details from.
        /// </summary>
        public const string ProbePath = "/healthz";

        /// <summary>
        /// The default name of the HTTP endpoint projects serve health check details from.
        /// </summary>
        public const string EndpointName = "healthchecks";
    }
}
