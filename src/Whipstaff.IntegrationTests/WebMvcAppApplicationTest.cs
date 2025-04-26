// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Example.WebMvcApp;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using Whipstaff.Playwright;
using Whipstaff.Playwright.Crawler;
using Xunit;

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
            await WithWebApplicationFactoryAsync(
                async factory =>
                {
                    var client = factory.CreateClient();
                    var requestUri = new Uri(requestPath, UriKind.Absolute);
                    var response = await client.GetAsync(requestUri);

                    _ = response.EnsureSuccessStatusCode();

                    var contentType = response.Content.Headers.ContentType;
                    Assert.NotNull(contentType);

                    Assert.Equal(
                        expectedContentType,
    #pragma warning disable CS8602 // Dereference of a possibly null reference.
                        contentType.ToString());
    #pragma warning restore CS8602 // Dereference of a possibly null reference.

                    await LogResponseAsync(response, Logger);
                },
                []);
        }

        /// <summary>
        /// Test to ensure the home controller can only be accessed from the root of the site.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task GetReturnsNotFoundForHomeControllerWhenSpecifiedExplicitlyAsync()
        {
            await WithWebApplicationFactoryAsync(
                async factory =>
                {
                    var client = factory.CreateClient();
#pragma warning disable S1075 // URIs should not be hardcoded
                    var requestUri = new Uri("https://localhost/home", UriKind.Absolute);
#pragma warning restore S1075 // URIs should not be hardcoded
                    var response = await client.GetAsync(requestUri);

                    Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);

                    await LogResponseAsync(response, Logger);
                },
                []);
        }

        /// <summary>
        /// Test to ensure the site can be crawled with no errors.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task CrawlSiteWithNoErrors()
        {
            await WithWebApplicationFactoryAsync(
                async factory =>
                {
                    var client = factory.CreateClient();
#pragma warning disable S1075 // URIs should not be hardcoded
                    var requestUri = new Uri("https://localhost/", UriKind.Absolute);
#pragma warning restore S1075 // URIs should not be hardcoded

                    using (var playwright = await Microsoft.Playwright.Playwright.CreateAsync()
                               .ConfigureAwait(false))
                    await using (var browser =
                                 await playwright.GetBrowserAsync(PlaywrightBrowserTypeAndChannel.Chrome()))
                    {
                        var page = await browser.NewPageAsync()
                            .ConfigureAwait(false);
                        await page.RouteAsync(
                                "**/*",
                                route => DefaultHandler(client, route))
                            .ConfigureAwait(false);

                        var crawlResults = await WebCrawler.CrawlSiteAsync(requestUri, page, CancellationToken.None)
                            .ConfigureAwait(false);

                        foreach (var uriCrawlResultModel in crawlResults)
                        {
#pragma warning disable CA1848 // Use the LoggerMessage delegates
                            Logger.LogInformation(
                                "Crawl result for {Uri}: Status Code {StatusCode}, Page Errors Count {PageErrorsCount}",
                                uriCrawlResultModel.Key,
                                uriCrawlResultModel.Value.StatusCode,
                                uriCrawlResultModel.Value.PageErrors.Count);
#pragma warning restore CA1848 // Use the LoggerMessage delegates
                        }

                        Assert.NotNull(crawlResults);
                        Assert.NotEmpty(crawlResults);
                        Assert.All(
                            crawlResults,
                            pageResult => CheckPageResult(pageResult));
                    }
                },
                []);
        }

        private static void CheckPageResult(KeyValuePair<string, UriCrawlResultModel> pageResult)
        {
            Assert.False(string.IsNullOrWhiteSpace(pageResult.Key));
        }

        private static TheoryData<string, string> GetGetReturnsSuccessAndCorrectContentTypeTestSource()
        {
            return new TheoryData<string, string>
            {
                {
                    "https://localhost/",
                    "text/html; charset=utf-8"
                }
            };
        }

        private static async Task DefaultHandler(HttpClient client, IRoute route)
        {
            if (!route.Request.Url.StartsWith("https://localhost/", StringComparison.OrdinalIgnoreCase))
            {
                var routeFulfillOptions = new RouteFulfillOptions
                {
                    Status = 404
                };

                await route.FulfillAsync(routeFulfillOptions)
                    .ConfigureAwait(false);

                return;
            }

            using (var request = route.ToHttpRequestMessage())
            {
                var response = await client.SendAsync(request)
                    .ConfigureAwait(false);

                var routeFulfillOptions = await RouteFulfillOptionsFactory.FromHttpResponseMessageAsync(response)
                    .ConfigureAwait(false);

                await route.FulfillAsync(routeFulfillOptions)
                    .ConfigureAwait(false);
            }
        }
    }
}
