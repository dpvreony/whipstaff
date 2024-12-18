// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Whipstaff.AspNetCore.Features.ApplicationStartup
{
    /// <summary>
    /// Represents a web application startup.
    /// </summary>
    public interface IWhipstaffWebAppStartup
    {
        /// <summary>
        /// Allows the application to configure the Aspire service defaults.
        /// </summary>
        /// <param name="builder">Host Application builder to modify.</param>
        void ConfigureAspireServiceDefaults(IHostApplicationBuilder builder);

        /// <summary>
        /// Configures logging for the application.
        /// </summary>
        /// <param name="loggingBuilder">Logging Builder to configure.</param>
        /// <param name="configuration">Application configuration.</param>
        /// <param name="environment">Web Host environment.</param>
        void ConfigureLogging(
            ILoggingBuilder loggingBuilder,
            ConfigurationManager configuration,
            IWebHostEnvironment environment);

        /// <summary>
        /// Configures services for the application.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        /// <param name="configuration">Application configuration.</param>
        /// <param name="environment">Web Host environment.</param>
        void ConfigureServices(
            IServiceCollection services,
            ConfigurationManager configuration,
            IWebHostEnvironment environment);

        /// <summary>
        /// Configures the web application.
        /// </summary>
        /// <param name="applicationBuilder">The application builder to configure.</param>
        void ConfigureWebApplication(WebApplication applicationBuilder);
    }
}
