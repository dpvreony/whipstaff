// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Dhgms.AspNetCoreContrib.Example.WebBlazorWasm
{
    /// <summary>
    /// Startup logic for Web Blazor WASM.
    /// </summary>
    public class Startup : IStartup
    {
        /// <inheritdoc/>
        public void Configure(IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var componentsApplicationBuilder = app.ApplicationServices.GetService<IComponentsApplicationBuilder>();
#pragma warning disable CC0021 // Use nameof
            componentsApplicationBuilder.AddComponent<App>("app");
#pragma warning restore CC0021 // Use nameof
        }

        /// <inheritdoc/>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
            return services.BuildServiceProvider();
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
        }
    }
}
