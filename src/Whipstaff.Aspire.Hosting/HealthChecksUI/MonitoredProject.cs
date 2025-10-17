// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Aspire.Hosting.ApplicationModel;

namespace Whipstaff.Aspire.Hosting.HealthChecksUI
{
    /// <summary>
    /// Represents a project to be monitored by a <see cref="HealthChecksUIResource"/>.
    /// </summary>
    public class MonitoredProject(IResourceBuilder<ProjectResource> project, string endpointName, string probePath)
    {
        private string? _name;

        /// <summary>
        /// Gets the project to be monitored.
        /// </summary>
        public IResourceBuilder<ProjectResource> Project { get; } = project ?? throw new ArgumentNullException(nameof(project));

        /// <summary>
        /// Gets the name of the endpoint the project serves health check details from. If it doesn't exist it will be added.
        /// </summary>
        public string EndpointName { get; } = endpointName ?? throw new ArgumentNullException(nameof(endpointName));

        /// <summary>
        /// Gets or sets the name of the project to be displayed in the HealthChecksUI dashboard. Defaults to the project resource's name.
        /// </summary>
        public string Name
        {
            get => _name ?? Project.Resource.Name;
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the request path the project serves health check details for the HealthChecksUI dashboard from.
        /// </summary>
        public string ProbePath { get; set; } = probePath ?? throw new ArgumentNullException(nameof(probePath));
    }
}
