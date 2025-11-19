// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Eventing;
using Aspire.Hosting.Lifecycle;

namespace Whipstaff.Aspire.Hosting.HealthChecksUI
{
    /// <summary>
    /// Event subscriber for configuring Health Checks UI resources.
    /// </summary>
    public sealed class HealthChecksUILifecycleHook : IDistributedApplicationEventingSubscriber
    {
        private const string HEALTHCHECKSUIURLS = "HEALTHCHECKSUI_URLS";

        /// <inheritdoc/>
        public Task SubscribeAsync(IDistributedApplicationEventing eventing, DistributedApplicationExecutionContext executionContext, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(eventing);
            ArgumentNullException.ThrowIfNull(executionContext);

            _ = eventing.Subscribe<BeforeStartEvent>((@event, ct) => BeforeStartAsync(@event.Model, executionContext));
#pragma warning disable CS0618 // Type or member is obsolete - Will be refactored in future to use ResourceEndpointsAllocatedEvent
            _ = eventing.Subscribe<AfterEndpointsAllocatedEvent>((@event, ct) => AfterEndpointsAllocatedAsync(@event.Model));
#pragma warning restore CS0618 // Type or member is obsolete

            return Task.CompletedTask;
        }

        private static void ConfigureHealthChecksUIContainers(IResourceCollection resources, bool isPublishing)
        {
            var healhChecksUIResources = resources.OfType<HealthChecksUIResource>();

            foreach (var healthChecksUIResource in healhChecksUIResources)
            {
                var monitoredProjects = healthChecksUIResource.MonitoredProjects;

                // Add environment variables to configure the HealthChecksUI container with the health checks endpoints of each referenced project
                // See example configuration at https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks?tab=readme-ov-file#sample-2-configuration-using-appsettingsjson
                for (var i = 0; i < monitoredProjects.Count; i++)
                {
                    var monitoredProject = monitoredProjects[i];
                    var healthChecksEndpoint = monitoredProject.Project.GetEndpoint(monitoredProject.EndpointName);

                    // Set health check name
                    var nameEnvVarName = HealthChecksUIResource.KnownEnvVars.GetHealthCheckNameKey(i);
                    healthChecksUIResource.Annotations.Add(
                        new EnvironmentCallbackAnnotation(
                            nameEnvVarName,
                            () => monitoredProject.Name));

                    // Set health check URL
                    var probePath = monitoredProject.ProbePath.TrimStart('/');
                    var urlEnvVarName = HealthChecksUIResource.KnownEnvVars.GetHealthCheckUriKey(i);

                    healthChecksUIResource.Annotations.Add(
                        new EnvironmentCallbackAnnotation(
                            context => context[urlEnvVarName] = isPublishing
                                ? ReferenceExpression.Create($"{healthChecksEndpoint}/{probePath}")
                                : new HostUrl($"{healthChecksEndpoint.Url}/{probePath}")));
                }
            }
        }

        private Task BeforeStartAsync(DistributedApplicationModel appModel, DistributedApplicationExecutionContext executionContext)
        {
            ArgumentNullException.ThrowIfNull(appModel);

            // Configure each project referenced by a Health Checks UI resource
            var healthChecksUIResources = appModel.Resources.OfType<HealthChecksUIResource>();

            foreach (var healthChecksUIResource in healthChecksUIResources)
            {
                foreach (var monitoredProject in healthChecksUIResource.MonitoredProjects)
                {
                    var project = monitoredProject.Project;

                    // Add the health check endpoint if it doesn't exist
                    var healthChecksEndpoint = project.GetEndpoint(monitoredProject.EndpointName);
                    if (!healthChecksEndpoint.Exists)
                    {
                        _ = project.WithHttpEndpoint(name: monitoredProject.EndpointName);
                        Debug.Assert(healthChecksEndpoint.Exists, "The health check endpoint should exist after adding it.");
                    }

                    // Set environment variable to configure the URLs the health check endpoint is accessible from
                    _ = project.WithEnvironment(context =>
                    {
                        var probePath = monitoredProject.ProbePath.TrimStart('/');
                        var healthChecksEndpointsExpression = ReferenceExpression.Create($"{healthChecksEndpoint}/{probePath}");

                        if (context.ExecutionContext.IsRunMode)
                        {
                            // Running during dev inner-loop
                            var containerHost = healthChecksUIResource.GetEndpoint("http").Resource.Name;
                            var fromContainerUriBuilder = new UriBuilder(healthChecksEndpoint.Url)
                            {
                                Host = containerHost,
                                Path = monitoredProject.ProbePath
                            };

                            healthChecksEndpointsExpression = ReferenceExpression.Create($"{healthChecksEndpointsExpression};{fromContainerUriBuilder.ToString()}");
                        }

                        context.EnvironmentVariables.Add(HEALTHCHECKSUIURLS, healthChecksEndpointsExpression);
                    });
                }
            }

            if (executionContext.IsPublishMode)
            {
                ConfigureHealthChecksUIContainers(appModel.Resources, isPublishing: true);
            }

            return Task.CompletedTask;
        }

        private Task AfterEndpointsAllocatedAsync(DistributedApplicationModel appModel)
        {
            ArgumentNullException.ThrowIfNull(appModel);

            ConfigureHealthChecksUIContainers(appModel.Resources, isPublishing: false);

            return Task.CompletedTask;
        }
    }
}
