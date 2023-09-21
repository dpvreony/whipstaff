// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Whipstaff.AspNetCore.Features.ApplicationStartup;
using Xunit;
using Xunit.Abstractions;

namespace Whipstaff.UnitTests.Example
{
    /// <summary>
    /// Tests for the Startup class.
    /// </summary>
    /// <typeparam name="TStartUp">Whipstaff based startup class for the application.</typeparam>
    public abstract class AbstractStartupTests<TStartUp> : Foundatio.Xunit.TestWithLoggingBase
        where TStartUp : IWhipstaffWebAppStartup, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractStartupTests{TStartUp}"/> class.
        /// </summary>
        /// <param name="output">XUnit test output helper instance.</param>
        protected AbstractStartupTests(ITestOutputHelper output)
            : base(output)
        {
        }

        /// <summary>
        /// Tests that the Startup class can be created without error.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ReturnsAppAsync()
        {
            var args = new[]
            {
                "--environment",
                "Development"
            };

            var builder = AspNetCore.Features.ApplicationStartup.WebHostBuilderFactory.GetHostBuilder<TStartUp>(args).UseTestServer().UseDefaultServiceProvider(
                (_, options) =>
                {
                    options.ValidateScopes = true;
                    options.ValidateOnBuild = true;
                });

            var app = builder.Build();
            Assert.NotNull(app);

            await app.StartAsync().ConfigureAwait(false);
            await app.StopAsync().ConfigureAwait(false);
        }
    }
}
