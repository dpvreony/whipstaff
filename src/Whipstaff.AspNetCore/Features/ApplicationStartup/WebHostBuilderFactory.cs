// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Whipstaff.AspNetCore.Features.ApplicationStartup
{
    /// <summary>
    /// Helper class for creating a <see cref="IHost"/>.
    /// </summary>
    public static class WebHostBuilderFactory
    {
        /// <summary>
        /// Gets a host builder based on the provided startup class.
        /// </summary>
        /// <typeparam name="TStartup">The type for the startup class. This is not an interface based on the ASP.NET core <see cref="IStartup"/>, this one provides hosting context and takes the app away from reflection whilst enforcing some design contract.</typeparam>
        /// <param name="args">Command line arguments.</param>
        /// <returns>The host builder instance.</returns>
        public static IWebHostBuilder GetHostBuilder<TStartup>(string[] args)
            where TStartup : IWhipstaffWebAppStartup, new()
        {
            var startup = new TStartup();

            return WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((ctx, loggingBuilder) => startup.ConfigureLogging(ctx, loggingBuilder))
                .ConfigureServices((ctx, services) => startup.ConfigureServices(ctx, services))
                .Configure((context, builder) => startup.ConfigureWebApplication(context, builder));
        }
    }
}
