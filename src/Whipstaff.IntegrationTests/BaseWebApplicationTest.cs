// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Dhgms.AspNetCoreContrib.IntegrationTests
{
    /// <summary>
    /// Base class for unit tests for netcore web apps.
    /// </summary>
    /// <typeparam name="TStartup">The type of the startup class.</typeparam>
    [ExcludeFromCodeCoverage]
    public class BaseWebApplicationTest<TStartup>
        : Foundatio.Logging.Xunit.TestWithLoggingBase, IClassFixture<WebApplicationFactory<TStartup>>
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
            Factory = new WebApplicationFactory<TStartup>();
        }

        /// <summary>
        /// Gets the Web Application Factory.
        /// </summary>
        protected WebApplicationFactory<TStartup> Factory { get; }

        /// <summary>
        /// Helper method to log the http response.
        /// </summary>
        /// <param name="response">Http Response.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected async Task LogResponseAsync(HttpResponseMessage response)
        {
            if (response == null)
            {
                _logger.LogInformation("No HTTP Response Message.");
                return;
            }

            foreach (var (key, value) in response.Headers)
            {
                _logger.LogInformation($"{key}: {string.Join(",", value)}");
            }

            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            _logger.LogInformation(result);
        }
    }
}
