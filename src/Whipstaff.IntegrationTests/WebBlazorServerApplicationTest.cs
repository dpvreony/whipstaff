// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if TBC
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Example.WebBlazorServer;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Dhgms.AspNetCoreContrib.IntegrationTests
{
    /// <summary>
    /// Unit Tests for a Web Blazor app startup.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class WebBlazorServerApplicationTest : BaseWebApplicationTest<Startup>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebMvcAppApplicationTest"/> class.
        /// </summary>
        /// <param name="output">XUnit Logging output helper.</param>
        public WebBlazorServerApplicationTest(ITestOutputHelper output)
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
        /// <param name="requestUri">URL to test.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Theory]
        [MemberData(nameof(GetReturnsSuccessAndCorrectContentTypeTestSource))]
        public async Task GetReturnsSuccessAndCorrectContentTypeAsync(string requestUri)
        {
            var client = Factory.CreateClient();
            var response = await client.GetAsync(requestUri).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            Assert.Equal(
                "text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            await LogResponseAsync(response).ConfigureAwait(false);
        }

        private static IEnumerable<object[]> GetGetReturnsSuccessAndCorrectContentTypeTestSource()
        {
            return new[]
            {
                new object[]
                {
                    "/",
                },
            };
        }
    }
}
#endif
