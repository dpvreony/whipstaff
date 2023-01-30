// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
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
        where TStartup : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseWebApplicationTest{TStartup}"/> class.
        /// </summary>
        /// <param name="output">XUnit Logging output helper.</param>
        public BaseWebApplicationTest(
            ITestOutputHelper output)
            : base(output)
        {
        }

        /// <summary>
        /// Gets the Web Application Factory.
        /// </summary>
        /// <param name="webApplicationFunc">Function to pass the web application factory to.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected Task WithWebApplicationFactory(Func<WebApplicationFactory<TStartup>, Task> webApplicationFunc)
        {
            ArgumentNullException.ThrowIfNull(webApplicationFunc);

            using (var factory = new WebApplicationFactory<TStartup>())
            {
                var runner = factory.WithWebHostBuilder(configuration => configuration.UseStartup<TStartup>());
                using (runner)
                {
                    return webApplicationFunc(factory);
                }
            }
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
