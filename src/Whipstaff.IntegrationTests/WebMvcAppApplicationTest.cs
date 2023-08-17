﻿// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Example.WebMvcApp;
using Xunit;
using Xunit.Abstractions;

namespace Whipstaff.IntegrationTests
{
    /// <summary>
    /// Unit Tests for a Web MVC app startup.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class WebMvcAppApplicationTest : BaseWebApplicationTest<Startup>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebMvcAppApplicationTest"/> class.
        /// </summary>
        /// <param name="output">XUnit Logging output helper.</param>
        public WebMvcAppApplicationTest(ITestOutputHelper output)
            : base(output)
        {
        }

        /// <summary>
        /// Gets the XUnit test source for checking GET requests succeed.
        /// </summary>
        public static TheoryData<string> GetReturnsSuccessAndCorrectContentTypeTestSource =>
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
            await WithWebApplicationFactory(async factory =>
            {
                var client = factory.CreateClient();
                var requestUri = new Uri(requestPath, UriKind.Absolute);
                var response = await client.GetAsync(requestUri).ConfigureAwait(false);

                _ = response.EnsureSuccessStatusCode();

                var contentType = response.Content.Headers.ContentType;
                Assert.NotNull(contentType);

                Assert.Equal(
                    "text/html; charset=utf-8",
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    contentType.ToString());
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                await LogResponseAsync(response).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        private static TheoryData<string> GetGetReturnsSuccessAndCorrectContentTypeTestSource()
        {
            return new TheoryData<string>
            {
                "https://localhost/fakecrud",
                "https://localhost/fakecrud/1",
            };
        }
    }
}
