// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.IO.Abstractions;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Whipstaff.Mermaid.HttpServer;
using Whipstaff.Playwright;

namespace Whipstaff.Mermaid.Playwright
{
    /// <summary>
    /// Represents a Playwright browser instance for rendering Mermaid diagrams.
    /// </summary>
    public sealed class PlaywrightRendererBrowserInstance : IDisposable
    {
        private readonly IPlaywright _playwright;
        private readonly IPage _page;
        private readonly PlaywrightRendererLogMessageActionsWrapper _logMessageActionsWrapper;
        private bool _disposedValue;

        private PlaywrightRendererBrowserInstance(
            IPlaywright playwright,
            IPage page,
            PlaywrightRendererLogMessageActionsWrapper logMessageActionsWrapper)
        {
            _playwright = playwright;
            _page = page;
            _logMessageActionsWrapper = logMessageActionsWrapper;
        }

        /// <summary>
        /// Gets a new instance of the PlaywrightRendererBrowserInstance based on the desired browser.
        /// Sets the page up ready to render Mermaid diagrams.
        /// </summary>
        /// <param name="mermaidHttpServer">The in memory mermaid HTTP server.</param>
        /// <param name="playwrightBrowserTypeAndChannel">Browser and channel type to use.</param>
        /// <param name="logMessageActionsWrapper">Log message actions wrapper.</param>
        /// <returns>Browser wrapper instance.</returns>
        public static async Task<PlaywrightRendererBrowserInstance> GetBrowserInstance(
            MermaidHttpServer mermaidHttpServer,
            PlaywrightBrowserTypeAndChannel playwrightBrowserTypeAndChannel,
            PlaywrightRendererLogMessageActionsWrapper logMessageActionsWrapper)
        {
            ArgumentNullException.ThrowIfNull(mermaidHttpServer);
            ArgumentNullException.ThrowIfNull(playwrightBrowserTypeAndChannel);
            ArgumentNullException.ThrowIfNull(logMessageActionsWrapper);

            var playwright = await Microsoft.Playwright.Playwright.CreateAsync()
                .ConfigureAwait(false);
            var browser = await playwright.GetBrowserType(playwrightBrowserTypeAndChannel.PlaywrightBrowserType)
                .LaunchAsync(new() { Headless = true, Channel = playwrightBrowserTypeAndChannel.Channel });
            var page = await browser.NewPageAsync()
                .ConfigureAwait(false);

#pragma warning disable S1075
            const string pageUrl = "https://localhost/index.html";
#pragma warning restore S1075

            var inMemoryHttpClient = mermaidHttpServer.CreateClient();
            await page.RouteAsync(
                    pageUrl,
                    route => MermaidPostHandler(inMemoryHttpClient, route))
                .ConfigureAwait(false);

            await page.RouteAsync(
                    "**/*.{mjs,js}",
                    route => DefaultHandler(inMemoryHttpClient, route))
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

            return new PlaywrightRendererBrowserInstance(playwright, page, logMessageActionsWrapper);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
        }

        /// <summary>
        /// Gets the SVG for the Mermaid Diagram from a File and writes to another file.
        /// </summary>
        /// <param name="sourceFile">File containing the diagram markdown to convert.</param>
        /// <param name="targetFile">Destination file to write the diagram content to.</param>
        /// <returns>SVG diagram.</returns>
        public async Task CreateDiagramAndWriteToFileAsync(
            IFileInfo sourceFile,
            IFileInfo targetFile)
        {
            ArgumentNullException.ThrowIfNull(sourceFile);
            ArgumentNullException.ThrowIfNull(targetFile);

            if (!sourceFile.Exists)
            {
                throw new ArgumentException("Source file does not exist", nameof(sourceFile));
            }

            if (sourceFile.FullName == targetFile.FullName)
            {
                throw new ArgumentException("Source and target files cannot be the same", nameof(targetFile));
            }

            if (targetFile.Exists)
            {
                throw new ArgumentException("Target file already exists", nameof(targetFile));
            }

            var diagram = await GetDiagram(sourceFile)
                .ConfigureAwait(false);

            if (diagram == null)
            {
                throw new InvalidOperationException("Failed to get diagram");
            }

            await diagram.InternalToFileAsync(targetFile)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the SVG for the Mermaid Diagram from a File.
        /// </summary>
        /// <param name="sourceFileInfo">File containing the diagram markdown to convert.</param>
        /// <returns>SVG diagram.</returns>
        public async Task<GetDiagramResponseModel?> GetDiagram(IFileInfo sourceFileInfo)
        {
            ArgumentNullException.ThrowIfNull(sourceFileInfo);

            if (!sourceFileInfo.Exists)
            {
                throw new ArgumentException("File does not exist", nameof(sourceFileInfo));
            }

            using (var streamReader = sourceFileInfo.OpenText())
            {
                return await GetDiagram(streamReader)
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets the SVG for the Mermaid Diagram from a <see cref="TextReader"/>.
        /// </summary>
        /// <param name="textReader">File containing the diagram markdown to convert.</param>
        /// <returns>SVG diagram.</returns>
        public async Task<GetDiagramResponseModel?> GetDiagram(TextReader textReader)
        {
            ArgumentNullException.ThrowIfNull(textReader);

            var markdown = await textReader
                .ReadToEndAsync()
                .ConfigureAwait(false);

            return await GetDiagram(markdown)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the diagram from the page using the provided markdown.
        /// </summary>
        /// <param name="markdown">Markdown to process.</param>
        /// <returns>Diagram model.</returns>
        public async Task<GetDiagramResponseModel?> GetDiagram(string markdown)
        {
            var svg = await _page.EvaluateAsync<string>("(diagram) => window.renderMermaid(diagram)", markdown);

            var mermaidElement = _page.Locator("#mermaid-element svg");

            if (mermaidElement == null)
            {
                _logMessageActionsWrapper.FailedToFindMermaidElement();
                return null;
            }

            var png = await mermaidElement.ScreenshotAsync(new LocatorScreenshotOptions { Type = ScreenshotType.Png })
                .ConfigureAwait(false);

            return new(
                svg,
                png);
        }

        private static HttpRequestMessage GetRequestFromRoute(IRoute route)
        {
            var httpRequestMessage = new HttpRequestMessage();

            var request = route.Request;

            httpRequestMessage.RequestUri = new Uri(request.Url);
            httpRequestMessage.Method = HttpMethod.Get;

            return httpRequestMessage;
        }

        private static async Task MermaidPostHandler(HttpClient httpClient, IRoute route)
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

        private static async Task DefaultHandler(HttpClient httpClient, IRoute route)
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

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _playwright?.Dispose();
                }

                _disposedValue = true;
            }
        }
    }
}
