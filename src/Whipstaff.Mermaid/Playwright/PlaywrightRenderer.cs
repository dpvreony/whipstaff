// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Whipstaff.Mermaid.HttpServer;
using Whipstaff.Playwright;

namespace Whipstaff.Mermaid.Playwright
{
    /// <summary>
    /// Markdown renderer using Playwright.
    /// </summary>
    public sealed class PlaywrightRenderer
    {
        private readonly MermaidHttpServer _mermaidHttpServerFactory;
        private readonly PlaywrightRendererLogMessageActionsWrapper _logMessageActionsWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaywrightRenderer"/> class.
        /// </summary>
        /// <param name="mermaidHttpServer">In memory http server instance for Mermaid.</param>
        /// <param name="logMessageActionsWrapper">Log message actions wrapper.</param>
        public PlaywrightRenderer(
            MermaidHttpServer mermaidHttpServer,
            PlaywrightRendererLogMessageActionsWrapper logMessageActionsWrapper)
        {
            ArgumentNullException.ThrowIfNull(mermaidHttpServer);
            ArgumentNullException.ThrowIfNull(logMessageActionsWrapper);
            _mermaidHttpServerFactory = mermaidHttpServer;
            _logMessageActionsWrapper = logMessageActionsWrapper;
        }

        /// <summary>
        /// Gets the SVG for the Mermaid Diagram.
        /// </summary>
        /// <param name="markdown">Diagram markdown to convert.</param>
        /// <param name="playwrightBrowserType">Browser type to use.</param>
        /// <param name="browserChannel">The channel to use for the specified browser.</param>
        /// <returns>SVG diagram.</returns>
        public async Task<GetDiagramResponseModel?> GetDiagram(
            string markdown,
            PlaywrightBrowserType playwrightBrowserType,
            string? browserChannel)
        {
            if (string.IsNullOrWhiteSpace(markdown))
            {
                throw new ArgumentNullException(nameof(markdown));
            }

            using (var playwright = await Microsoft.Playwright.Playwright.CreateAsync()
                .ConfigureAwait(false))
            await using (var browser = await playwright.GetBrowserType(playwrightBrowserType).LaunchAsync(new()
                         {
                             Headless = true,
                             Channel = browserChannel
                         }))
            {
                var page = await browser.NewPageAsync()
                    .ConfigureAwait(false);

#pragma warning disable S1075
                const string pageUrl = "https://localhost/index.html";
#pragma warning restore S1075
                await page.RouteAsync(
                        pageUrl,
                        route => MermaidPostHandler(route, markdown))
                    .ConfigureAwait(false);

                await page.RouteAsync(
                        "**/*.{mjs,js}",
                        route => DefaultHandler(route))
                    .ConfigureAwait(false);

                var pageResponse = await page.GotoAsync(pageUrl, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle })
                    .ConfigureAwait(false);

                if (pageResponse == null)
                {
                    _logMessageActionsWrapper.FailedToGetPageResponse();
                    return null;
                }

                if (!pageResponse.Ok)
                {
                    _logMessageActionsWrapper.UnexpectedPageResponse(pageResponse);
                    return null;
                }

                _ = await pageResponse.FinishedAsync().ConfigureAwait(false);

                await page.WaitForLoadStateAsync(LoadState.NetworkIdle).ConfigureAwait(false);

                var mermaidElement = page.Locator("#mermaid-element");

                if (mermaidElement == null)
                {
                    _logMessageActionsWrapper.FailedToFindMermaidElement();
                    return null;
                }

                var innerText = await mermaidElement.InnerHTMLAsync();
                var png = await mermaidElement.ScreenshotAsync(new LocatorScreenshotOptions { Type = ScreenshotType.Png })
                    .ConfigureAwait(false);

                return new(innerText, png);
            }
        }

        private static HttpRequestMessage GetRequestFromRoute(IRoute route, string markdown)
        {
            var httpRequestMessage = new HttpRequestMessage();

            var request = route.Request;

            httpRequestMessage.RequestUri = new Uri(request.Url);
            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.Content = new FormUrlEncodedContent(
            [
                new("diagram", markdown)
            ]);

            return httpRequestMessage;
        }

        private async Task MermaidPostHandler(IRoute route, string diagram)
        {
            using (var client = _mermaidHttpServerFactory.CreateClient())
            using (var request = GetRequestFromRoute(route, diagram))
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

        private async Task DefaultHandler(IRoute route)
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

            using (var client = _mermaidHttpServerFactory.CreateClient())
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
