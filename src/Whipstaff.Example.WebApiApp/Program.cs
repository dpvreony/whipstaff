﻿// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Whipstaff.AspNetCore.Features.Apm.ApplicationInsights;

namespace Dhgms.AspNetCoreContrib.Example.WebApiApp
{
    /// <summary>
    /// Holds the entry point for the application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Entry point for the application.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Gets the web host builder for the application instance.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>Web host builder.</returns>
        public static IHostBuilder CreateWebHostBuilder(string[] args)
        {
            var applicationInsights = new ApplicationInsightsApplicationStartHelper();

            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => _ = webBuilder.UseStartup<Startup>())
                .ConfigureLogging((context, builder) =>
                {
                    // TODO: add default logging configuration
                    applicationInsights.ConfigureLogging(context, builder);
                });
        }
    }
}
