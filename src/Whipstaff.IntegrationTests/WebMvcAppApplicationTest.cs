﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Examples.WebMvcApp;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Dhgms.AspNetCoreContrib.IntegrationTests
{
    /// <summary>
    /// Unit Tests for a Web MVC app startup.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class WebMvcAppApplicationTest : BaseWebApplicationTest<Dhgms.AspNetCoreContrib.Examples.WebMvcApp.Startup>
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
            var requestUri = new Uri(requestPath, UriKind.Relative);
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
