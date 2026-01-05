// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using NetTestRegimentation;
using NetTestRegimentation.XUnit.Theories.ArgumentNullException;
using Whipstaff.Playwright;
using Whipstaff.Testing.Logging;
using Xunit;

namespace Whipstaff.UnitTests.Playwright
{
    /// <summary>
    /// Unit tests for <see cref="PageExtensions"/>.
    /// </summary>
    public static class PageExtensionsTests
    {
        private static async Task WithPlayWrightPage(Func<IPage, Task> actionFunc)
        {
            await WithPlayWrightBrowser(async browser =>
            {
                var page = await browser.NewPageAsync();
                await actionFunc(page);
            });
        }

        private static async Task WithPlayWrightBrowser(Func<IBrowser, Task> actionFunc)
        {
            var playwrightBrowserTypeAndChannel = PlaywrightBrowserTypeAndChannel.Chrome();
            using (var playwright = await Microsoft.Playwright.Playwright.CreateAsync())
            await using (var browser = await playwright.GetBrowserAsync(playwrightBrowserTypeAndChannel).ConfigureAwait(false))
            {
                await actionFunc(browser);

                // Close the browser
                await browser.CloseAsync();
            }
        }

        private static async Task PlaywrightToTestServerRouteHandler(IRoute route, TestServer testServer)
        {
            using (var client = testServer.CreateClient())
            using (var request = GetRequestFromRoute(route))
            {
                var response = await client.SendAsync(request)
                    .ConfigureAwait(false);
                var routeFulfillOptions = new RouteFulfillOptions
                {
                    Status = (int)response.StatusCode,
                    Body = await response.Content.ReadAsStringAsync().ConfigureAwait(false),
                };

                if (response.Content.Headers.ContentType != null)
                {
                    routeFulfillOptions.ContentType = response.Content.Headers.ContentType.ToString();
                }

                await route.FulfillAsync(routeFulfillOptions)
                    .ConfigureAwait(false);
            }
        }

        private static HttpRequestMessage GetRequestFromRoute(IRoute route)
        {
            var httpRequestMessage = new HttpRequestMessage();

            var request = route.Request;

            httpRequestMessage.RequestUri = new Uri(request.Url);
            httpRequestMessage.Method = HttpMethod.Get;

            return httpRequestMessage;
        }

        private static async Task ValidAltTagHandler(HttpContext context)
        {
            context.Response.StatusCode = 200;
            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync("<html><body><img src=\"test.jpg\" alt=\"Some description.\" /></body></html>");
        }

        /// <summary>
        /// Unit tests for <see cref="PageExtensions.GetClassDefinitionAnalysisAsync"/>.
        /// </summary>
        public sealed class GetClassDefinitionAnalysisAsyncMethod : TestWithLoggingBase,
            ITestMethodWithNullableParameters<IPage>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetClassDefinitionAnalysisAsyncMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output instance.</param>
            public GetClassDefinitionAnalysisAsyncMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc />
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionTestSource))]
            public void ThrowsArgumentNullException(
                IPage? arg,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => arg!.EnumerateImgTagsWithIncompleteAltAttributeAsync());
            }

            /// <summary>
            /// Test that the method returns a populated collection.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsResult()
            {
                await WithPlayWrightPage(async page =>
                {
                    using var host = new HostBuilder()
                        .ConfigureWebHost(webHostBuilder =>
                        {
                            _ = webHostBuilder
                                .UseTestServer() // If using TestServer
                                .Configure(app =>
                                {
                                    _ = app.Map("/index.htm", applicationBuilder => applicationBuilder.Run(CssAnalysisHandler));
                                });
                        })
                        .Build();
                    await host.StartAsync();

                    var testServer = host.GetTestServer();

                    // Navigate to a page
#pragma warning disable S1075 // URIs should not be hardcoded
                    await page.RouteAsync(
                        "https://localhost/index.htm",
                        route => PlaywrightToTestServerRouteHandler(route, testServer));
#pragma warning restore S1075 // URIs should not be hardcoded

#pragma warning disable S1075 // URIs should not be hardcoded
                    var response = await page.GotoAsync("https://localhost/index.htm");
#pragma warning restore S1075 // URIs should not be hardcoded
                    Assert.NotNull(response);
                    Assert.True(response.Ok);

                    // Get class definition analysis
                    var classDefinitionAnalysis = await page.GetClassDefinitionAnalysisAsync();

                    Assert.NotEmpty(classDefinitionAnalysis.Used);
                    Assert.NotEmpty(classDefinitionAnalysis.Defined);
                    Assert.NotEmpty(classDefinitionAnalysis.Undefined);
                });
            }

            private static async Task CssAnalysisHandler(HttpContext context)
            {
                context.Response.StatusCode = 200;
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync("""
                                                  <html>
                                                  <head>
                                                  <style>
                                                    .classA {
                                                      color: green;
                                                      font-weight: bold;
                                                    }
                                                  </style>
                                                  </head>
                                                  <body>
                                                  <h1 class="classA">This is using classA (defined)</h1>
                                                  <p class="classA">This is also using classA (defined)</h1>
                                                  <p class="classB">This is using classB (undefined)</p>
                                                  <p class="classB">This is also using classB (undefined)</p>
                                                  </body>
                                                  </html>
                                                  """);
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullException"/>.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionTestSource : ArgumentNullExceptionTheoryData<IPage>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="GetClassDefinitionAnalysisAsyncMethod.ThrowsArgumentNullExceptionTestSource"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionTestSource()
                    : base("page")
                {
                }
            }
        }

        /// <summary>
        /// Unit tests for <see cref="PageExtensions.EnumerateImgTagsWithIncompleteAltAttributeAsync"/>.
        /// </summary>
        public sealed class EnumerateImgTagsWithIncompleteAltAttributeMethod : TestWithLoggingBase, ITestMethodWithNullableParameters<IPage>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="EnumerateImgTagsWithIncompleteAltAttributeMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output instance.</param>
            public EnumerateImgTagsWithIncompleteAltAttributeMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc />
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionTestSource))]
            public void ThrowsArgumentNullException(
                IPage? arg,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => arg!.EnumerateImgTagsWithIncompleteAltAttributeAsync());
            }

            /// <summary>
            /// Test that the method returns a populated collection.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsPopulatedCollection()
            {
                await WithPlayWrightPage(async page =>
                {
                    using var host = new HostBuilder()
                        .ConfigureWebHost(webHostBuilder =>
                        {
                            _ = webHostBuilder
                                .UseTestServer() // If using TestServer
                                .Configure(app =>
                                {
                                    _ = app.Map("/index.htm", applicationBuilder => applicationBuilder.Run(InvalidAltTagHandler));
                                });
                        })
                        .Build();
                    await host.StartAsync();

                    var testServer = host.GetTestServer();

                    // Navigate to a page
#pragma warning disable S1075 // URIs should not be hardcoded
                    await page.RouteAsync(
                        "https://localhost/index.htm",
                        route => PlaywrightToTestServerRouteHandler(route, testServer));
#pragma warning restore S1075 // URIs should not be hardcoded

#pragma warning disable S1075 // URIs should not be hardcoded
                    var response = await page.GotoAsync("https://localhost/index.htm");
#pragma warning restore S1075 // URIs should not be hardcoded
                    Assert.NotNull(response);
                    Assert.True(response.Ok);

                    // Get all img elements
                    // Replace with ToArrayAsync() once upstream System.Linq.Async package is removed.
                    var images = page.EnumerateImgTagsWithIncompleteAltAttributeAsync().ToBlockingEnumerable().ToArray();

                    Assert.NotEmpty(images);
                });
            }

            /// <summary>
            /// Test that the method returns an empty collection.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsEmptyCollection()
            {
                await WithPlayWrightPage(async page =>
                {
                    using var host = new HostBuilder()
                        .ConfigureWebHost(webHostBuilder =>
                        {
                            _ = webHostBuilder
                                .UseTestServer() // If using TestServer
                                .Configure(app =>
                                {
                                    _ = app.Map("/index.htm", applicationBuilder => applicationBuilder.Run(ValidAltTagHandler));
                                });
                        })
                        .Build();
                    await host.StartAsync();

                    var testServer = host.GetTestServer();

                    // Navigate to a page
#pragma warning disable S1075 // URIs should not be hardcoded
                    await page.RouteAsync(
                        "https://localhost/index.htm",
                        route => PlaywrightToTestServerRouteHandler(route, testServer));
#pragma warning restore S1075 // URIs should not be hardcoded

#pragma warning disable S1075 // URIs should not be hardcoded
                    var response = await page.GotoAsync("https://localhost/index.htm");
#pragma warning restore S1075 // URIs should not be hardcoded
                    Assert.NotNull(response);
                    Assert.True(response.Ok);

                    // Get all img elements
                    // Replace with ToArrayAsync() once upstream System.Linq.Async package is removed.
                    var images = page.EnumerateImgTagsWithIncompleteAltAttributeAsync().ToBlockingEnumerable().ToArray();

                    Assert.Empty(images);
                });
            }

            private static async Task InvalidAltTagHandler(HttpContext context)
            {
                context.Response.StatusCode = 200;
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync("""
                                                  <html>
                                                  <body>
                                                  <img src=\"test.jpg\" alt=\"\" />
                                                  <img src=\"test.jpg\" alt=\"Missing full stop\" />
                                                  </body>
                                                  </html>
                                                  """);
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullException"/>.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionTestSource : ArgumentNullExceptionTheoryData<IPage>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionTestSource()
                    : base("page")
                {
                }
            }
        }

        /// <summary>
        /// Unit tests for <see cref="PageExtensions.GetMhtmlAsStringAsync"/>.
        /// </summary>
        public sealed class GetMhtmlAsStringAsyncMethod : TestWithLoggingBase, ITestAsyncMethodWithNullableParameters<IPage>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetMhtmlAsStringAsyncMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output instance.</param>
            public GetMhtmlAsStringAsyncMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc />
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionTestSource))]
            public async Task ThrowsArgumentNullExceptionAsync(IPage? arg, string expectedParameterNameForException)
            {
                _ = await Assert.ThrowsAsync<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => arg!.GetMhtmlAsStringAsync());
            }

            /// <summary>
            /// Test that the method returns a non-empty string.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
            [Fact]
            public async Task ReturnsDataAsync()
            {
                await WithPlayWrightPage(async page =>
                {
                    using var host = new HostBuilder()
                        .ConfigureWebHost(webHostBuilder =>
                        {
                            _ = webHostBuilder
                                .UseTestServer() // If using TestServer
                                .Configure(app =>
                                {
                                    _ = app.Map("/index.htm", applicationBuilder => applicationBuilder.Run(ValidAltTagHandler));
                                });
                        })
                        .Build();
                    await host.StartAsync();

                    var testServer = host.GetTestServer();

                    // Navigate to a page
#pragma warning disable S1075 // URIs should not be hardcoded
                    await page.RouteAsync(
                        "https://localhost/index.htm",
                        route => PlaywrightToTestServerRouteHandler(route, testServer));
#pragma warning restore S1075 // URIs should not be hardcoded

#pragma warning disable S1075 // URIs should not be hardcoded
                    var response = await page.GotoAsync("https://localhost/index.htm");
#pragma warning restore S1075 // URIs should not be hardcoded
                    Assert.NotNull(response);
                    Assert.True(response.Ok);

                    // Get all img elements
                    var mhtml = await page.GetMhtmlAsStringAsync();

                    Logger.LogInformation(mhtml);

                    Assert.NotEmpty(mhtml);
                });
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullExceptionAsync"/>.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionTestSource : ArgumentNullExceptionTheoryData<IPage>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="GetMhtmlAsStringAsyncMethod.ThrowsArgumentNullExceptionTestSource"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionTestSource()
                    : base("page")
                {
                }
            }
        }

        /// <summary>
        /// Unit tests for <see cref="PageExtensions.SaveAsMhtmlAsync"/>.
        /// </summary>
        public sealed class SaveAsMhtmlAsyncMethod : TestWithLoggingBase, ITestAsyncMethodWithNullableParameters<IPage, IFileSystem, string>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SaveAsMhtmlAsyncMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output instance.</param>
            public SaveAsMhtmlAsyncMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc />
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionTestSource))]
            public async Task ThrowsArgumentNullExceptionAsync(IPage? arg1, IFileSystem? arg2, string? arg3, string expectedParameterNameForException)
            {
                _ = await Assert.ThrowsAsync<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => arg1!.SaveAsMhtmlAsync(arg2!, arg3!, CancellationToken.None));
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullExceptionAsync"/>.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionTestSource : ArgumentNullExceptionTheoryData<IPage, IFileSystem, string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionTestSource()
                    : base(
                        new NamedParameterInput<IPage>(
                            "page",
                            () => new IPageCreateExpectations().Instance()),
                        new NamedParameterInput<IFileSystem>(
                            "fileSystem",
                            () => new MockFileSystem()),
                        new NamedParameterInput<string>(
                            "outputPath",
                            () => "z:\\somefile.mhtm"))
                {
                }
            }
        }
    }
}
