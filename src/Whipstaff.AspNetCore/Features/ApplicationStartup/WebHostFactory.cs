// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Hosting;

namespace Whipstaff.AspNetCore.Features.ApplicationStartup
{
    /// <summary>
    /// Factory helper class for creating a <see cref="IWebHost"/>.
    /// </summary>
    public static class WebHostFactory
    {
        /// <summary>
        /// Gets a host based on the provided startup class.
        /// </summary>
        /// <typeparam name="TStartup">The type for the startup class. This is not an interface based on the ASP.NET core <see cref="IStartup"/>, this one provides hosting context and takes the app away from reflection whilst enforcing some design contract.</typeparam>
        /// <param name="args">Command line arguments.</param>
        /// <returns>The host instance.</returns>
        public static IWebHost GetWebHost<TStartup>(string[] args)
            where TStartup : IWhipstaffWebAppStartup, new()
        {
            var hostBuilder = WebHostBuilderFactory.GetHostBuilder<TStartup>(args);
            return hostBuilder.Build();
        }
    }
}
