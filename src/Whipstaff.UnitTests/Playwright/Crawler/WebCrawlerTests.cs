// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using NetTestRegimentation;
using NetTestRegimentation.XUnit.Theories.ArgumentNullException;
using Rocks;
using Whipstaff.Playwright;
using Whipstaff.Playwright.Crawler;
using Whipstaff.Testing.Logging;
using Xunit;

[assembly: Rock(typeof(IBrowser), BuildType.Create)]
[assembly: Rock(typeof(IPage), BuildType.Create)]

namespace Whipstaff.UnitTests.Playwright.Crawler
{
    /// <summary>
    /// Unit tests for <see cref="WebCrawler"/>.
    /// </summary>
    public static class WebCrawlerTests
    {
        /// <summary>
        /// Unit tests for <see cref="WebCrawler.CrawlSiteAsync(Uri, PlaywrightBrowserTypeAndChannel, System.Threading.CancellationToken)"/>.
        /// </summary>
        public sealed class CrawlSiteAsyncUriPlaywrightBrowserTypeAndChannelCancellationTokenMethod : TestWithLoggingBase, ITestAsyncMethodWithNullableParameters<Uri, PlaywrightBrowserTypeAndChannel>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CrawlSiteAsyncUriPlaywrightBrowserTypeAndChannelCancellationTokenMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output instance.</param>
            public CrawlSiteAsyncUriPlaywrightBrowserTypeAndChannelCancellationTokenMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc />
            [Theory]
#pragma warning disable xUnit1045
#pragma warning disable xUnit1044
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestSource))]
#pragma warning restore xUnit1044
#pragma warning restore xUnit1045
            public async Task ThrowsArgumentNullExceptionAsync(Uri arg1, PlaywrightBrowserTypeAndChannel arg2, string expectedParameterNameForException)
            {
                _ = await Assert.ThrowsAsync<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => WebCrawler.CrawlSiteAsync(arg1, arg2, CancellationToken.None));
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullExceptionAsync"/>.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionAsyncTestSource : ArgumentNullExceptionTheoryData<Uri, PlaywrightBrowserTypeAndChannel>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionAsyncTestSource"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionAsyncTestSource()
                    : base(
                        new NamedParameterInput<Uri>("startUrl", static () => new Uri("https://localhost")),
                        new NamedParameterInput<PlaywrightBrowserTypeAndChannel>("playwrightBrowserTypeAndChannel", static () => PlaywrightBrowserTypeAndChannel.Chrome()))
                {
                }
            }
        }

        /// <summary>
        /// Unit tests for <see cref="WebCrawler.CrawlSiteAsync(string, PlaywrightBrowserTypeAndChannel, System.Threading.CancellationToken)"/>.
        /// </summary>
        public sealed class CrawlSiteAsyncStringPlaywrightBrowserTypeAndChannelCancellationTokenMethod : TestWithLoggingBase, ITestAsyncMethodWithNullableParameters<string, PlaywrightBrowserTypeAndChannel>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CrawlSiteAsyncStringPlaywrightBrowserTypeAndChannelCancellationTokenMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output instance.</param>
            public CrawlSiteAsyncStringPlaywrightBrowserTypeAndChannelCancellationTokenMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc />
            [Theory]
#pragma warning disable xUnit1044
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestSource))]
#pragma warning restore xUnit1044
            public async Task ThrowsArgumentNullExceptionAsync(string arg1, PlaywrightBrowserTypeAndChannel arg2, string expectedParameterNameForException)
            {
#pragma warning disable CA2234 // Pass system uri objects instead of strings
                _ = await Assert.ThrowsAsync<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => WebCrawler.CrawlSiteAsync(arg1, arg2, CancellationToken.None));
#pragma warning restore CA2234 // Pass system uri objects instead of strings
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullExceptionAsync"/>.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionAsyncTestSource : ArgumentNullExceptionTheoryData<string, PlaywrightBrowserTypeAndChannel>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionAsyncTestSource"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionAsyncTestSource()
                    : base(
                        new NamedParameterInput<string>("startUrl", static () => "https://localhost"),
                        new NamedParameterInput<PlaywrightBrowserTypeAndChannel>("playwrightBrowserTypeAndChannel", static () => PlaywrightBrowserTypeAndChannel.Chrome()))
                {
                }
            }
        }

        /// <summary>
        /// Unit tests for <see cref="WebCrawler.CrawlSiteAsync(string, IBrowser, System.Threading.CancellationToken)"/>.
        /// </summary>
        public sealed class CrawlSiteAsyncStringIBrowserCancellationTokenMethod : TestWithLoggingBase, ITestAsyncMethodWithNullableParameters<string, IBrowser>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CrawlSiteAsyncStringIBrowserCancellationTokenMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output instance.</param>
            public CrawlSiteAsyncStringIBrowserCancellationTokenMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc />
            [Theory]
#pragma warning disable xUnit1045
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestSource))]
#pragma warning restore xUnit1045
            public async Task ThrowsArgumentNullExceptionAsync(string arg1, IBrowser arg2, string expectedParameterNameForException)
            {
#pragma warning disable CA2234 // Pass system uri objects instead of strings
                _ = await Assert.ThrowsAsync<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => WebCrawler.CrawlSiteAsync(arg1, arg2, CancellationToken.None));
#pragma warning restore CA2234 // Pass system uri objects instead of strings
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullExceptionAsync"/>.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionAsyncTestSource : ArgumentNullExceptionTheoryData<string, IBrowser>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionAsyncTestSource"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionAsyncTestSource()
                    : base(
                        new NamedParameterInput<string>("startUrl", static () => "https://localhost"),
                        new NamedParameterInput<IBrowser>("browser", static () => new IBrowserCreateExpectations().Instance()))
                {
                }
            }
        }

        /// <summary>
        /// Unit tests for <see cref="WebCrawler.CrawlSiteAsync(Uri, IBrowser, System.Threading.CancellationToken)"/>.
        /// </summary>
        public sealed class CrawlSiteAsyncUriIBrowserCancellationTokenMethod : TestWithLoggingBase, ITestAsyncMethodWithNullableParameters<Uri, IBrowser>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CrawlSiteAsyncUriIBrowserCancellationTokenMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output instance.</param>
            public CrawlSiteAsyncUriIBrowserCancellationTokenMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc />
            [Theory]
#pragma warning disable xUnit1045
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestSource))]
#pragma warning restore xUnit1045
            public async Task ThrowsArgumentNullExceptionAsync(Uri arg1, IBrowser arg2, string expectedParameterNameForException)
            {
#pragma warning disable CA2234 // Pass system uri objects instead of strings
                _ = await Assert.ThrowsAsync<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => WebCrawler.CrawlSiteAsync(arg1, arg2, CancellationToken.None));
#pragma warning restore CA2234 // Pass system uri objects instead of strings
            }

            /// <summary>
            /// Test to ensure the crawler returns results.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
            [Fact]
            public async Task ReturnsResultAsync()
            {
                using (var playwright = await Microsoft.Playwright.Playwright.CreateAsync())
                await using (var browser =
                             await playwright.GetBrowser(PlaywrightBrowserTypeAndChannel.Chrome()))
                {
                    var context = await browser.NewContextAsync();
                    await context.RouteAsync(
                        "**/*",
                        static route => DefaultHandler(route));

#pragma warning disable CA2234 // Pass system uri objects instead of strings
                    var crawlResults = await WebCrawler.CrawlSiteAsync("https://localhost", context.Browser!, CancellationToken.None);
#pragma warning restore CA2234 // Pass system uri objects instead of strings

                    foreach (var uriCrawlResultModel in crawlResults)
                    {
                        _logger.LogInformation($"{uriCrawlResultModel.Key}: {uriCrawlResultModel.Value.StatusCode} {uriCrawlResultModel.Value.PageErrors.Count}");
                    }

                    Assert.NotNull(crawlResults);
                    Assert.NotEmpty(crawlResults);
                }
            }

            private static async Task DefaultHandler(IRoute route)
            {
                if (route.Request.Url.Equals("https://localhost/", StringComparison.Ordinal))
                {
                    var routeFulfillOptions = new RouteFulfillOptions
                    {
                        Status = 200,
                        ContentType = "text/html; charset=utf-8",
                        Body = """
                               <html>
                               <head>
                               </head>
                               <body>
                               <a href="https://localhost/1">1</a>
                                 <a href="https://localhost/2">2</a>
                               </body>
                               </html>
                               """,
                    };
                    await route.FulfillAsync(routeFulfillOptions).ConfigureAwait(false);
                    return;
                }

                if (route.Request.Url.Equals("https://localhost/1", StringComparison.Ordinal))
                {
                    var routeFulfillOptions = new RouteFulfillOptions
                    {
                        Status = 200,
                        ContentType = "text/html; charset=utf-8",
                        Body = """
                               <html>
                               <head>
                               </head>
                               <body>
                                 <a href="https://localhost/1">1</a>
                               <a href="https://localhost/2">2</a>
                               </body>
                               </html>
                               """,
                    };
                    await route.FulfillAsync(routeFulfillOptions).ConfigureAwait(false);
                }

                await route.FulfillAsync(new RouteFulfillOptions
                {
                    Status = 404
                })
                    .ConfigureAwait(false);
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullExceptionAsync"/>.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionAsyncTestSource : ArgumentNullExceptionTheoryData<Uri, IBrowser>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionAsyncTestSource"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionAsyncTestSource()
                    : base(
                        new NamedParameterInput<Uri>("startUrl", static () => new Uri("https://localhost")),
                        new NamedParameterInput<IBrowser>("browser", static () => new IBrowserCreateExpectations().Instance()))
                {
                }
            }
        }

        /// <summary>
        /// Unit tests for <see cref="WebCrawler.CrawlSiteAsync(string, IPage, System.Threading.CancellationToken)"/>.
        /// </summary>
        public sealed class CrawlSiteAsyncStringIPageCancellationTokenMethod : TestWithLoggingBase, ITestAsyncMethodWithNullableParameters<string, IPage>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CrawlSiteAsyncStringIPageCancellationTokenMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output instance.</param>
            public CrawlSiteAsyncStringIPageCancellationTokenMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc />
            [Theory]
#pragma warning disable xUnit1045
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestSource))]
#pragma warning restore xUnit1045
            public async Task ThrowsArgumentNullExceptionAsync(string arg1, IPage arg2, string expectedParameterNameForException)
            {
#pragma warning disable CA2234 // Pass system uri objects instead of strings
                _ = await Assert.ThrowsAsync<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => WebCrawler.CrawlSiteAsync(arg1, arg2, CancellationToken.None));
#pragma warning restore CA2234 // Pass system uri objects instead of strings
            }

            /// <summary>
            /// Test to ensure the crawler returns results.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
            [Fact]
            public async Task ReturnsResultAsync()
            {
                using (var playwright = await Microsoft.Playwright.Playwright.CreateAsync())
                await using (var browser =
                             await playwright.GetBrowser(PlaywrightBrowserTypeAndChannel.Chrome()))
                {
                    var page = await browser.NewPageAsync();
                    await page.RouteAsync(
                            "**/*",
                            static route => DefaultHandler(route));

#pragma warning disable CA2234 // Pass system uri objects instead of strings
                    var crawlResults = await WebCrawler.CrawlSiteAsync("https://localhost", page, CancellationToken.None);
#pragma warning restore CA2234 // Pass system uri objects instead of strings

                    foreach (var uriCrawlResultModel in crawlResults)
                    {
                        _logger.LogInformation($"{uriCrawlResultModel.Key}: {uriCrawlResultModel.Value.StatusCode} {uriCrawlResultModel.Value.PageErrors.Count}");
                    }

                    Assert.NotNull(crawlResults);
                    Assert.NotEmpty(crawlResults);
                }
            }

            private static async Task DefaultHandler(IRoute route)
            {
                if (route.Request.Url.Equals("https://localhost/", StringComparison.Ordinal))
                {
                    var routeFulfillOptions = new RouteFulfillOptions
                    {
                        Status = 200,
                        ContentType = "text/html; charset=utf-8",
                        Body = """
                               <html>
                               <head>
                               </head>
                               <body>
                               <a href="https://localhost/1">1</a>
                                 <a href="https://localhost/2">2</a>
                               </body>
                               </html>
                               """,
                    };
                    await route.FulfillAsync(routeFulfillOptions).ConfigureAwait(false);
                    return;
                }

                if (route.Request.Url.Equals("https://localhost/1", StringComparison.Ordinal))
                {
                    var routeFulfillOptions = new RouteFulfillOptions
                    {
                        Status = 200,
                        ContentType = "text/html; charset=utf-8",
                        Body = """
                               <html>
                               <head>
                               </head>
                               <body>
                                 <a href="https://localhost/1">1</a>
                               <a href="https://localhost/2">2</a>
                               </body>
                               </html>
                               """,
                    };
                    await route.FulfillAsync(routeFulfillOptions).ConfigureAwait(false);
                }

                await route.FulfillAsync(new RouteFulfillOptions
                    {
                        Status = 404
                    })
                    .ConfigureAwait(false);
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullExceptionAsync"/>.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionAsyncTestSource : ArgumentNullExceptionTheoryData<string, IPage>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionAsyncTestSource"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionAsyncTestSource()
                    : base(
                        new NamedParameterInput<string>("startUrl", static () => "https://localhost"),
                        new NamedParameterInput<IPage>("page", static () => new IPageCreateExpectations().Instance()))
                {
                }
            }
        }

        /// <summary>
        /// Unit tests for <see cref="WebCrawler.CrawlSiteAsync(Uri, IPage, System.Threading.CancellationToken)"/>.
        /// </summary>
        public sealed class CrawlSiteAsyncUriIPageCancellationTokenMethod : TestWithLoggingBase, ITestAsyncMethodWithNullableParameters<Uri, IPage>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CrawlSiteAsyncUriIPageCancellationTokenMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output instance.</param>
            public CrawlSiteAsyncUriIPageCancellationTokenMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc />
            [Theory]
#pragma warning disable xUnit1045
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestSource))]
#pragma warning restore xUnit1045
            public async Task ThrowsArgumentNullExceptionAsync(Uri arg1, IPage arg2, string expectedParameterNameForException)
            {
#pragma warning disable CA2234 // Pass system uri objects instead of strings
                _ = await Assert.ThrowsAsync<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => WebCrawler.CrawlSiteAsync(arg1, arg2, CancellationToken.None));
#pragma warning restore CA2234 // Pass system uri objects instead of strings
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullExceptionAsync"/>.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionAsyncTestSource : ArgumentNullExceptionTheoryData<Uri, IPage>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionAsyncTestSource"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionAsyncTestSource()
                    : base(
                        new NamedParameterInput<Uri>("startUrl", static () => new Uri("https://localhost")),
                        new NamedParameterInput<IPage>("page", static () => new IPageCreateExpectations().Instance()))
                {
                }
            }
        }
    }
}
