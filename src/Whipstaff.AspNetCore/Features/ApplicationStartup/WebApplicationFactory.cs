// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Whipstaff.AspNetCore.Features.ApplicationStartup
{
    /// <summary>
    /// Helper class for creating a <see cref="IHost"/>.
    /// </summary>
    public static class WebApplicationFactory
    {
        /// <summary>
        /// Gets a host builder based on the provided startup class.
        /// </summary>
        /// <typeparam name="TStartup">The type for the startup class. This is not an interface based on the ASP.NET core <see cref="IStartup"/>, this one provides hosting context and takes the app away from reflection whilst enforcing some design contract.</typeparam>
        /// <param name="args">Command line arguments.</param>
        /// <param name="builderOverrideAction">Action to override the builder setup. Typically used to inject a test host etc.</param>
        /// <returns>The host builder instance.</returns>
        public static WebApplication GetHostApplicationBuilder<TStartup>(
            string[] args,
            Action<WebApplicationBuilder>? builderOverrideAction)
            where TStartup : IWhipstaffWebAppStartup, new()
        {
            var builder = WebApplication.CreateBuilder(args);

            return SetUpHostApplicationBuilder<TStartup>(
                builder,
                builderOverrideAction);
        }

        /// <summary>
        /// Gets a host builder based on the provided startup class.
        /// </summary>
        /// <typeparam name="TStartup">The type for the startup class. This is not an interface based on the ASP.NET core <see cref="IStartup"/>, this one provides hosting context and takes the app away from reflection whilst enforcing some design contract.</typeparam>
        /// <param name="options">Options to set the web application up with.</param>
        /// <param name="builderOverrideAction">Action to override the builder setup. Typically used to inject a test host etc.</param>
        /// <returns>The host builder instance.</returns>
        public static WebApplication GetHostApplicationBuilder<TStartup>(
            WebApplicationOptions options,
            Action<WebApplicationBuilder>? builderOverrideAction)
            where TStartup : IWhipstaffWebAppStartup, new()
        {
            ArgumentNullException.ThrowIfNull(options);

            var builder = WebApplication.CreateBuilder(options);

            return SetUpHostApplicationBuilder<TStartup>(
                builder,
                builderOverrideAction);
        }

        private static WebApplication SetUpHostApplicationBuilder<TStartup>(
            WebApplicationBuilder builder,
            Action<WebApplicationBuilder>? builderOverrideAction)
            where TStartup : IWhipstaffWebAppStartup, new()
        {
            var startup = new TStartup();

            startup.ConfigureAspireServiceDefaults(builder);

            startup.ConfigureLogging(
                builder.Logging,
                builder.Configuration,
                builder.Environment);

            startup.ConfigureServices(
                builder.Services,
                builder.Configuration,
                builder.Environment);

            if (builderOverrideAction != null)
            {
                builderOverrideAction(builder);
            }

            var appBuilder = builder.Build();

            startup.ConfigureWebApplication(appBuilder);
            return appBuilder;
        }
    }
}
