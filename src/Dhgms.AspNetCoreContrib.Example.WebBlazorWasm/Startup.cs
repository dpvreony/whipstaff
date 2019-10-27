// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Dhgms.AspNetCoreContrib.Example.WebBlazorWasm
{
    /// <summary>
    /// Startup logic for Web Blazor WASM.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configures the application.
        /// </summary>
        /// <param name="app">Application Builder to configure against.</param>
#pragma warning disable CA1822 // Mark members as static
#pragma warning disable CC0091 // Use static method
        public void Configure(IComponentsApplicationBuilder app)
#pragma warning restore CC0091 // Use static method
#pragma warning restore CA1822 // Mark members as static
        {
#pragma warning disable CC0021 // Use nameof
            app.AddComponent<App>("app");
#pragma warning restore CC0021 // Use nameof
        }

        /// <summary>
        /// Configure services for the application.
        /// </summary>
        /// <param name="services">Service collection to modify.</param>
#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable CC0057 // Unused parameters
#pragma warning disable CA1822 // Mark members as static
#pragma warning disable CC0091 // Use static method
#pragma warning disable RECS0154 // Parameter is never used
#pragma warning disable CA1801 // Unused variable
        public void ConfigureServices(IServiceCollection services)
#pragma warning restore CA1801 // Unused variable
#pragma warning restore RECS0154 // Parameter is never used
#pragma warning restore CC0091 // Use static method
#pragma warning restore CA1822 // Mark members as static
#pragma warning restore CC0057 // Unused parameters
#pragma warning restore IDE0060 // Remove unused parameter
        {
        }
    }
}
