// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Lifecycle;

namespace Whipstaff.Aspire.Hosting.HealthChecksUI
{
    /// <summary>
    /// Extensions for adding HealthChecksUI resources to the aspire application model.
    /// </summary>
    public static class HealthChecksUiExtensions
    {
        /// <summary>
        /// Adds a HealthChecksUI container to the application model.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The resource name.</param>
        /// <param name="port">The host port to expose the container on.</param>
        /// <returns>The resource builder.</returns>
        public static IResourceBuilder<HealthChecksUIResource> AddHealthChecksUi(
            this IDistributedApplicationBuilder builder,
            string name,
            int? port = null)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.Services.TryAddEventingSubscriber<HealthChecksUILifecycleHook>();

            var resource = new HealthChecksUIResource(name);

            return builder
                .AddResource(resource)
                .WithImage(HealthChecksUIDefaults.ContainerImageName, HealthChecksUIDefaults.ContainerImageTag)
                .WithImageRegistry(HealthChecksUIDefaults.ContainerRegistry)
                .WithEnvironment(HealthChecksUIResource.KnownEnvVars.UiPath, "/")
                .WithHttpEndpoint(port: port, targetPort: HealthChecksUIDefaults.ContainerPort);
        }

        /// <summary>
        /// Adds a reference to a project that will be monitored by the HealthChecksUI container.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="project">The project.</param>
        /// <param name="endpointName">
        /// The name of the HTTP endpoint the <see cref="ProjectResource"/> serves health check details from.
        /// The endpoint will be added if it is not already defined on the <see cref="ProjectResource"/>.
        /// </param>
        /// <param name="probePath">The request path the project serves health check details from.</param>
        /// <returns>The resource builder.</returns>
        public static IResourceBuilder<HealthChecksUIResource> WithReference(
            this IResourceBuilder<HealthChecksUIResource> builder,
            IResourceBuilder<ProjectResource> project,
            string endpointName = HealthChecksUIDefaults.EndpointName,
            string probePath = HealthChecksUIDefaults.ProbePath)
        {
            ArgumentNullException.ThrowIfNull(builder);

            var monitoredProject = new MonitoredProject(project, endpointName: endpointName, probePath: probePath);
            builder.Resource.MonitoredProjects.Add(monitoredProject);

            return builder;
        }

        // IDEA: Support referencing supported database containers and/or connection strings and configuring the HealthChecksUI container to use them
        //       https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks/blob/master/doc/ui-docker.md#Storage-Providers-Configuration
    }
}
