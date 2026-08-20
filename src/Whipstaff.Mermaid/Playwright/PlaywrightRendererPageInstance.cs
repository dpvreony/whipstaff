// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.IO.Abstractions;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Playwright;
using Whipstaff.Playwright;
using Whipstaff.Runtime.Extensions;

namespace Whipstaff.Mermaid.Playwright
{
    /// <summary>
    /// Represents a Playwright page instance for rendering Mermaid diagrams.
    /// </summary>
    public sealed class PlaywrightRendererPageInstance : IAsyncDisposable
    {
        private readonly IPage _page;
        private readonly IAsyncDisposable _pageRoute;
        private readonly IAsyncDisposable _pageRoute2;

        private readonly PlaywrightRendererBrowserInstanceLogMessageActionsWrapper _browserInstanceLogMessageActionsWrapper;

        private bool _disposedValue;

#pragma warning disable GR0027
        private PlaywrightRendererPageInstance(
            IPage page,
            IAsyncDisposable pageRoute,
            IAsyncDisposable pageRoute2,
            PlaywrightRendererBrowserInstanceLogMessageActionsWrapper browserInstanceLogMessageActionsWrapper)
        {
            _page = page;
            _pageRoute = pageRoute;
            _pageRoute2 = pageRoute2;
            _browserInstanceLogMessageActionsWrapper = browserInstanceLogMessageActionsWrapper;
        }
#pragma warning restore GR0027

        /// <summary>
        /// Gets a new instance of the PlaywrightRendererPageInstance based on the desired browser.
        /// Sets the page up ready to render Mermaid diagrams.
        /// </summary>
        /// <param name="mermaidHttpServer">The in memory mermaid HTTP server.</param>
        /// <param name="browser">The Playwright browser instance.</param>
        /// <param name="logMessageActionsWrapper">Log message actions wrapper.</param>
        /// <returns>Page wrapper instance.</returns>
        public static async Task<PlaywrightRendererPageInstance> GetPageInstanceAsync(
            TestServer mermaidHttpServer,
            IBrowser browser,
            PlaywrightRendererBrowserInstanceLogMessageActionsWrapper logMessageActionsWrapper)
        {
            ArgumentNullException.ThrowIfNull(mermaidHttpServer);
            ArgumentNullException.ThrowIfNull(browser);
            ArgumentNullException.ThrowIfNull(logMessageActionsWrapper);

            var page = await browser.NewPageAsync()
                .ConfigureAwait(false);

#pragma warning disable S1075
            const string pageUrl = "https://localhost/index.html";
#pragma warning restore S1075

            var inMemoryHttpClient = mermaidHttpServer.CreateClient();

            var pageRoute = await page.RouteAsync(
                    pageUrl,
                    route => MermaidPostHandlerAsync(inMemoryHttpClient, route))
                .ConfigureAwait(false);

            var pageRoute2 = await page.RouteAsync(
                    "**/*.{mjs,js}",
                    route => DefaultHandlerAsync(inMemoryHttpClient, route))
                .ConfigureAwait(false);

            var pageResponse = await page.GotoAsync(pageUrl, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle })
                .ConfigureAwait(false);

            if (pageResponse == null)
            {
                logMessageActionsWrapper.FailedToGetPageResponse();
                throw new InvalidOperationException("Failed to get page response.");
            }

            if (!pageResponse.Ok)
            {
                logMessageActionsWrapper.UnexpectedPageResponse(pageResponse);
                throw new InvalidOperationException("Unexpected page response: " + pageResponse.Status + " " +
                                                    pageResponse.StatusText);
            }

            _ = await pageResponse.FinishedAsync().ConfigureAwait(false);

            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded).ConfigureAwait(false);
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle).ConfigureAwait(false);
            _ = await page.WaitForFunctionAsync("() => window.mermaid !== undefined").ConfigureAwait(false);

            return new PlaywrightRendererPageInstance(
                page,
                pageRoute,
                pageRoute2,
                logMessageActionsWrapper);
        }

        /// <summary>
        /// Gets the SVG for the Mermaid Diagram from a File.
        /// </summary>
        /// <param name="sourceFileInfo">File containing the diagram markdown to convert.</param>
        /// <returns>SVG diagram.</returns>
        public async Task<GetDiagramResponseModel?> GetDiagramAsync(IFileInfo sourceFileInfo)
        {
            ArgumentNullException.ThrowIfNull(sourceFileInfo);

            if (!sourceFileInfo.Exists)
            {
                throw new ArgumentException("File does not exist", nameof(sourceFileInfo));
            }

            using (var streamReader = sourceFileInfo.OpenText())
            {
                return await GetDiagramAsync(streamReader)
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets the diagram from the page using the provided markdown.
        /// </summary>
        /// <param name="markdown">Markdown to process.</param>
        /// <returns>Diagram model.</returns>
        public async Task<GetDiagramResponseModel?> GetDiagramAsync(string markdown)
        {
            markdown.ThrowIfNullOrWhitespace();
            var svg = await _page.EvaluateAsync<string>("(diagram) => window.renderMermaid(diagram)", markdown);

            const string mermaidElementSelector = "#mermaid-element svg";
            var mermaidElement = _page.Locator(mermaidElementSelector);

            if (await mermaidElement.CountAsync().ConfigureAwait(false) == 0)
            {
                _browserInstanceLogMessageActionsWrapper.FailedToFindMermaidElement();
                return null;
            }

            var png = await TakeMermaidElementScreenshotAsync(mermaidElement).ConfigureAwait(false);

            return new(
                svg,
                png);
        }

        /// <summary>
        /// Gets the SVG for the Mermaid Diagram from a <see cref="TextReader"/>.
        /// </summary>
        /// <param name="textReader">TextReader containing the diagram markdown to convert.</param>
        /// <returns>SVG diagram.</returns>
        public async Task<GetDiagramResponseModel?> GetDiagramAsync(TextReader textReader)
        {
            ArgumentNullException.ThrowIfNull(textReader);

            var markdown = await textReader
                .ReadToEndAsync()
                .ConfigureAwait(false);

            return await GetDiagramAsync(markdown)
                .ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);
            GC.SuppressFinalize(this);
        }

        private static HttpRequestMessage GetRequestFromRoute(IRoute route)
        {
            var httpRequestMessage = new HttpRequestMessage();

            var request = route.Request;

            httpRequestMessage.RequestUri = new Uri(request.Url);
            httpRequestMessage.Method = HttpMethod.Get;

            return httpRequestMessage;
        }

        private static async Task MermaidPostHandlerAsync(HttpClient httpClient, IRoute route)
        {
            using (var request = GetRequestFromRoute(route))
            {
                var response = await httpClient.SendAsync(request)
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

        private static async Task DefaultHandlerAsync(HttpClient httpClient, IRoute route)
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
                var response = await httpClient.SendAsync(request)
                    .ConfigureAwait(false);

                var routeFulfillOptions = await RouteFulfillOptionsFactory.FromHttpResponseMessageAsync(response)
                    .ConfigureAwait(false);

                await route.FulfillAsync(routeFulfillOptions)
                    .ConfigureAwait(false);
            }
        }

        private static async Task<byte[]> TakeMermaidElementScreenshotAsync(ILocator mermaidElement)
        {
            return await mermaidElement.ScreenshotAsync(new LocatorScreenshotOptions { Type = ScreenshotType.Png })
                .ConfigureAwait(false);
        }

        private async ValueTask DisposeAsyncCore()
        {
            if (!_disposedValue)
            {
                await _pageRoute.DisposeAsync().ConfigureAwait(false);
                await _pageRoute2.DisposeAsync().ConfigureAwait(false);
                await _page.CloseAsync().ConfigureAwait(false);

                _disposedValue = true;
            }
        }
    }
}
