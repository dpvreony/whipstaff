// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Globalization;
using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

namespace Whipstaff.Aspire.Hosting.ZedAttackProxy
{
    /// <summary>
    /// Zed Attack Proxy extensions for <see cref="IDistributedApplicationBuilder"/>.
    /// </summary>
    public static class DistributedApplicationBuilderExtensions
    {
        /// <summary>
        /// Registers a container for Zed Attack Proxy that is configured to run as a daemon.
        /// </summary>
        /// <param name="builder">Builder to update.</param>
        /// <returns>Resource builder for the container, to allow further configuration.</returns>
        public static IResourceBuilder<ContainerResource> AddZedAttackProxyContainer(this IDistributedApplicationBuilder builder)
        {
            return builder.AddContainer(
                "zaproxy",
                "zaproxy/zap-stable");
        }

        /// <summary>
        /// Registers a container for Zed Attack Proxy that is configured to run as a daemon.
        /// </summary>
        /// <param name="builder">Builder to update.</param>
        /// <param name="targetPort">Port to use to host the proxy.</param>
        /// <param name="zapApiKey">API key to apply to the proxy.</param>
        /// <returns>Resource builder for the container, to allow further configuration.</returns>
        public static IResourceBuilder<ContainerResource> AddZedAttackProxyContainerAsDaemon(
            this IDistributedApplicationBuilder builder,
            int targetPort,
            string zapApiKey)
        {
            var zapArgs = new[]
            {
                "zap-x.sh",
                "-daemon",
                "-host", "0.0.0.0",
                "-port",
                targetPort.ToString(NumberFormatInfo.InvariantInfo),
                "-config",
                "api.addrs.addr.name=.*",
                "-config",
                "api.addrs.addr.regex=true",
                "-config",
                $"api.key={zapApiKey}"
            };

            var tempBuilder = builder.AddZedAttackProxyContainer()
                .WithArgs(zapArgs)
                .WithEnvironment(
                    "zaproxy_api_key",
                    zapApiKey);

            return tempBuilder.WithHttpEndpoint(
                targetPort: targetPort,
                isProxied: false);
        }
    }
}
