// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Playwright;
using NetTestRegimentation;
using NetTestRegimentation.XUnit.Theories.ArgumentNullException;
using Rocks;
using Whipstaff.Playwright;
using Whipstaff.Playwright.Crawler;
using Xunit;
using Xunit.Abstractions;

[assembly: Rock(typeof(IBrowser), BuildType.Create)]

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
        public sealed class CrawlSiteAsyncUriPlaywrightBrowserTypeAndChannelCancellationTokenMethod : Foundatio.Xunit.TestWithLoggingBase, ITestAsyncMethodWithNullableParameters<Uri, PlaywrightBrowserTypeAndChannel>
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
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestSource))]
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
        public sealed class CrawlSiteAsyncStringPlaywrightBrowserTypeAndChannelCancellationTokenMethod : Foundatio.Xunit.TestWithLoggingBase, ITestAsyncMethodWithNullableParameters<string, PlaywrightBrowserTypeAndChannel>
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
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestSource))]
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
        public sealed class CrawlSiteAsyncStringIBrowserCancellationTokenMethod : Foundatio.Xunit.TestWithLoggingBase, ITestAsyncMethodWithNullableParameters<string, IBrowser>
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
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestSource))]
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
    }
}
