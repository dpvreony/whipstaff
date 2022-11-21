// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Example.WebApiApp;
using Whipstaff.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Whipstaff.IntegrationTests
{
    /// <summary>
    /// Unit Tests for a Web API app startup.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class WebApiAppApplicationTest : BaseWebApplicationTest<FakeWebApiStartup>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiAppApplicationTest"/> class.
        /// </summary>
        /// <param name="output">XUnit Logging output helper.</param>
        public WebApiAppApplicationTest(ITestOutputHelper output)
            : base(output)
        {
        }

        /// <summary>
        /// Gets the XUnit test source for checking GET requests succeed.
        /// </summary>
        public static IEnumerable<object[]> GetReturnsSuccessAndCorrectContentTypeTestSource =>
            GetGetReturnsSuccessAndCorrectContentTypeTestSource();

        /// <summary>
        /// Checks that GET requests work.
        /// </summary>
        /// <param name="requestPath">URL to test.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Theory]
        [MemberData(nameof(GetReturnsSuccessAndCorrectContentTypeTestSource))]
        public async Task GetReturnsSuccessAndCorrectContentTypeAsync(string requestPath)
        {
            var client = Factory.CreateClient();

#pragma warning disable CA2234 // Pass system uri objects instead of strings
            var response = await client.GetAsync(requestPath).ConfigureAwait(false);
#pragma warning restore CA2234 // Pass system uri objects instead of strings

            _ = response.EnsureSuccessStatusCode();

            Assert.Equal(
                "application/json; charset=utf-8",
                response.Content.Headers.ContentType!.ToString());

            await LogResponseAsync(response).ConfigureAwait(false);
        }

        private static IEnumerable<object[]> GetGetReturnsSuccessAndCorrectContentTypeTestSource()
        {
            return new[]
            {
                new object[]
                {
                    "https://localhost/api/fakecrud/",
                },
                new object[]
                {
                    "https://localhost/api/fakecrud/1",
                },
            };
        }
    }
}
