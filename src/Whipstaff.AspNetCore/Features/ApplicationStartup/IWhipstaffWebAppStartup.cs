// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Whipstaff.AspNetCore.Features.ApplicationStartup
{
    /// <summary>
    /// Represents a web application startup.
    /// </summary>
    public interface IWhipstaffWebAppStartup
    {
        /// <summary>
        /// Configures logging for the application.
        /// </summary>
        /// <param name="hostBuilderContext">The Host Builder Context.</param>
        /// <param name="loggingBuilder">Logging Builder to configure.</param>
        void ConfigureLogging(WebHostBuilderContext hostBuilderContext, ILoggingBuilder loggingBuilder);

        /// <summary>
        /// Configures services for the application.
        /// </summary>
        /// <param name="hostBuilderContext">The Host Builder Context.</param>
        /// <param name="services">The service collection to configure.</param>
        void ConfigureServices(WebHostBuilderContext hostBuilderContext, IServiceCollection services);

        /// <summary>
        /// Configures the web application.
        /// </summary>
        /// <param name="webHostBuilderContext">The Web Host Builder Context.</param>
        /// <param name="applicationBuilder">The application builder to configure.</param>
        void ConfigureWebApplication(WebHostBuilderContext webHostBuilderContext, IApplicationBuilder applicationBuilder);
    }
}
