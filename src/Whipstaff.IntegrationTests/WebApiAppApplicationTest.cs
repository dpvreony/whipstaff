// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace Whipstaff.IntegrationTests
{
    /// <summary>
    /// Unit Tests for a Web API app startup.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class WebApiAppApplicationTest : BaseWebApplicationTest<Dhgms.AspNetCoreContrib.Example.WebApiApp.Startup>
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
        public static TheoryData<string, string> GetReturnsSuccessAndCorrectContentTypeTestSource =>
            GetGetReturnsSuccessAndCorrectContentTypeTestSource();

        /// <summary>
        /// Checks that GET requests work.
        /// </summary>
        /// <param name="requestPath">URL to test.</param>
        /// <param name="expectedContentType">Content Type expected back in the HTTP response.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Theory]
        [MemberData(nameof(GetReturnsSuccessAndCorrectContentTypeTestSource))]
        public async Task GetReturnsSuccessAndCorrectContentTypeAsync(string requestPath, string expectedContentType)
        {
            var args = new[]
            {
                "UseSwagger=true",
                "ApplicationInsights:ConnectionString=InstrumentationKey=00000000-0000-0000-0000-000000000000"
            };

            await WithWebApplicationFactoryAsync(
                async factory =>
                {
                    var client = factory.CreateClient();

    #pragma warning disable CA2234 // Pass system uri objects instead of strings
                    var response = await client.GetAsync(requestPath);
#pragma warning restore CA2234 // Pass system uri objects instead of strings

                    await LogResponseAsync(response, Logger);

                    _ = response.EnsureSuccessStatusCode();

                    Assert.Equal(
                        expectedContentType,
                        response.Content.Headers.ContentType!.ToString());
                },
                args);
        }

        private static TheoryData<string, string> GetGetReturnsSuccessAndCorrectContentTypeTestSource()
        {
            return new TheoryData<string, string>
            {
                {
                    "https://localhost/api/fakecrud/",
                    "application/json; charset=utf-8"
                },
                {
                    "https://localhost/api/fakecrud/1",
                    "application/json; charset=utf-8"
                },
                {
                    "https://localhost/swagger/index.html",
                    "text/html; charset=utf-8"
                },
                {
                    "https://localhost/swagger/v1/swagger.json",
                    "application/json; charset=utf-8"
                },
            };
        }
    }
}
