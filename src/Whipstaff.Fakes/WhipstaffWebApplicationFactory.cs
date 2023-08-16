// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Whipstaff.Testing
{
    /// <summary>
    /// Web Application Factory Wrapper that configures hooking up logging into the XUnit test runner.
    /// </summary>
    /// <typeparam name="TEntryPoint">A type in the entry point assembly of the application.
    /// Typically the Startup or Program classes can be used.</typeparam>
    public class WhipstaffWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint>
        where TEntryPoint : class
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
            return base.CreateHostBuilder()?
                .ConfigureLogging(loggingBuilder => OnConfigureLogging(loggingBuilder));
        }

        /// <inheritdoc/>
        protected override IWebHostBuilder? CreateWebHostBuilder()
        {
            return base.CreateWebHostBuilder()?.ConfigureLogging(loggingBuilder => OnConfigureLogging(loggingBuilder));
        }

        private void OnConfigureLogging(ILoggingBuilder loggingBuilder)
        {
            _ = loggingBuilder.Services.AddSingleton(_logFactory);
        }
    }
}
