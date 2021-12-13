// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.IntegrationTests;
using Xunit;
using Xunit.Abstractions;

namespace Whipstaff.IntegrationTests
{
    /// <summary>
    /// Unit Tests for a secured website.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class SecuredWebsiteTests : BaseWebApplicationTest<Dhgms.AspNetCoreContrib.Examples.WebMvcApp.Startup>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecuredWebsiteTests"/> class.
        /// </summary>
        /// <param name="output">XUnit Logging output helper.</param>
        public SecuredWebsiteTests(ITestOutputHelper output)
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
        /// <param name="requestPath">relative URL to test.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Theory]
        [MemberData(nameof(GetReturnsSuccessAndCorrectContentTypeTestSource))]
        public async Task GetReturnsSuccessAndCorrectContentTypeAsync(string requestPath)
        {
            var client = Factory.CreateClient();
            var relativeUri = new Uri(requestPath, UriKind.Relative);
            var response = await client.GetAsync(relativeUri).ConfigureAwait(false);

            _ = response.EnsureSuccessStatusCode();
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
