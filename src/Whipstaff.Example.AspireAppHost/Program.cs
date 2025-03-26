// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Aspire.Hosting;
using Whipstaff.Aspire.Hosting.ZedAttackProxy;

namespace Whipstaff.Example.AspireAppHost
{
    /// <summary>
    /// Program entry point for the application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main entry point for the application.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task Main(string[] args)
        {
            var app = GetApplication(args);
            await app.RunAsync();
        }

        /// <summary>
        /// Gets the builder for the distributed application.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>Instance of application builder.</returns>
        public static IDistributedApplicationBuilder GetBuilder(string[] args)
        {
            var builder = DistributedApplication.CreateBuilder(args);

            var apiSite = builder.AddProject<Projects.Dhgms_AspNetCoreContrib_Example_WebApiApp>("api-site")
                .WithExternalHttpEndpoints();

            var mvcSite = builder.AddProject<Projects.Dhgms_AspNetCoreContrib_Example_WebMvcApp>("mvc-site")
                .WithExternalHttpEndpoints();

            if (builder.ExecutionContext.IsRunMode)
            {
                var zapApiKey = "ZAPROXY-API-SECRET";

                _ = builder.AddZedAttackProxyContainerAsDaemon(
                        60080,
                        zapApiKey)
                    .WithReference(apiSite)
                    .WithReference(mvcSite);
            }

            return builder;
        }

        /// <summary>
        /// Gets the distributed application.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>Instance of the application.</returns>
        public static DistributedApplication GetApplication(string[] args)
        {
            var builder = GetBuilder(args);
            var app = builder.Build();
            return app;
        }
    }
}
