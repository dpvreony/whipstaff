// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Whipstaff.AspNetCore.Features.ApplicationStartup
{
    /// <summary>
    /// Represents a feature that needs to have services configured as part of application initialization.
    /// </summary>
    public interface IConfigureService
    {
        /// <summary>
        /// Configures the services for the feature into the application.
        /// </summary>
        /// <param name="services">The existing service collection to add registrations to.</param>
        /// <param name="configuration">Configuration object for the application.</param>
        void ConfigureService(
            IServiceCollection services,
            IConfiguration configuration);
    }
}
