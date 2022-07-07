// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Builder;

namespace Whipstaff.AspNetCore.Features.ApplicationStartup
{
    /// <summary>
    /// Represents a class that is used for configuring a feature during Application Startup.
    /// </summary>
    public interface IConfigureApplication
    {
        /// <summary>
        /// Configures the feature for the application.
        /// </summary>
        /// <param name="app">The application builder to add the configuration to.</param>
        void ConfigureApplication(IApplicationBuilder app);
    }
}
