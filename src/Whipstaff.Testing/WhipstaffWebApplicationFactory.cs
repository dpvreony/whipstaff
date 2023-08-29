// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Whipstaff.AspNetCore.Features.ApplicationStartup;

namespace Whipstaff.Testing
{
    /// <summary>
    /// Web Application Factory Wrapper that configures hooking up logging into the XUnit test runner.
    /// </summary>
    /// <typeparam name="TStartup">The type for the startup class. This is not an interface based on the ASP.NET core <see cref="IStartup"/>, this one provides hosting context and takes the app away from reflection whilst enforcing some design contract.</typeparam>
    public class WhipstaffWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class, IWhipstaffWebAppStartup, new()
    {
        private readonly ILoggerFactory _logFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="WhipstaffWebApplicationFactory{TEntryPoint}"/> class.
        /// </summary>
        /// <param name="logFactory">Logger Factory instance to hook into.</param>
        public WhipstaffWebApplicationFactory(ILoggerFactory logFactory)
        {
            ArgumentNullException.ThrowIfNull(logFactory);
            _logFactory = logFactory;
        }

        /// <inheritdoc/>
        protected override IHostBuilder? CreateHostBuilder()
        {
            return null;
        }

        /// <inheritdoc/>
        protected override IWebHostBuilder? CreateWebHostBuilder()
        {
            return WebHostBuilderFactory.GetHostBuilder<TStartup>(Array.Empty<string>()).ConfigureLogging(loggingBuilder => OnConfigureLogging(loggingBuilder));
        }

        private void OnConfigureLogging(ILoggingBuilder loggingBuilder)
        {
            _ = loggingBuilder.Services.AddSingleton(_logFactory);
        }
    }
}
