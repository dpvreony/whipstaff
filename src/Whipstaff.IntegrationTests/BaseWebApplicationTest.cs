// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Whipstaff.AspNetCore.Features.ApplicationStartup;
using Whipstaff.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Whipstaff.IntegrationTests
{
    /// <summary>
    /// Base class for unit tests for netcore web apps.
    /// </summary>
    /// <typeparam name="TStartup">The type of the startup class.</typeparam>
    [ExcludeFromCodeCoverage]
    public class BaseWebApplicationTest<TStartup>
        : Foundatio.Xunit.TestWithLoggingBase
        where TStartup : class, IWhipstaffWebAppStartup, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseWebApplicationTest{TStartup}"/> class.
        /// </summary>
        /// <param name="output">XUnit Logging output helper.</param>
        public BaseWebApplicationTest(
            ITestOutputHelper output)
            : base(output)
        {
            Log.DefaultMinimumLevel = LogLevel.Debug;
        }

        /// <summary>
        /// Gets the Web Application Factory.
        /// </summary>
        /// <param name="webApplicationFunc">Function to pass the web application factory to.</param>
        /// <param name="args">Command line arguments.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected static async Task WithWebApplicationFactoryAsync(Func<TestServer, Task> webApplicationFunc, string[] args)
        {
            ArgumentNullException.ThrowIfNull(webApplicationFunc);

            var webApplication = WebApplicationFactory.GetHostApplicationBuilder<TStartup>(
                args,
                applicationBuilder => applicationBuilder.WebHost.UseTestServer());
            var server = webApplication.Services.GetRequiredService<IServer>();
            if (server is not TestServer testServer)
            {
                throw new InvalidOperationException("Server is not a TestServer.");
            }

            await webApplication.StartAsync();

            await webApplicationFunc(testServer).ConfigureAwait(false);
        }

        /// <summary>
        /// Helper method to log the http response.
        /// </summary>
        /// <param name="response">Http Response.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected async Task LogResponseAsync(HttpResponseMessage response)
        {
            if (response == null)
            {
#pragma warning disable CA1848 // Use the LoggerMessage delegates
                _logger.LogInformation("No HTTP Response Message.");
#pragma warning restore CA1848 // Use the LoggerMessage delegates
                return;
            }

            foreach (var (key, value) in response.Headers)
            {
#pragma warning disable CA1848 // Use the LoggerMessage delegates
                _logger.LogInformation("Header Item: {Key} -> {Values}", key, string.Join(",", value));
#pragma warning restore CA1848 // Use the LoggerMessage delegates
            }

            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
#pragma warning disable CA1848 // Use the LoggerMessage delegates
            _logger.LogInformation("Result: {Result}", result);
#pragma warning restore CA1848 // Use the LoggerMessage delegates
        }
    }
}
