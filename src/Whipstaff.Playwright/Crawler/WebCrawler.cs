// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Playwright;
using ReactiveMarbles.ObservableEvents;

namespace Whipstaff.Playwright.Crawler
{
    /// <summary>
    /// Provides a simple web crawler using Playwright.
    /// </summary>
    public static class WebCrawler
    {
        /// <summary>
        /// Crawls a website starting from the specified URL.
        /// </summary>
        /// <param name="startUrl">The url to start crawling from.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static Task CrawlSiteAsync(
            Uri startUrl,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(startUrl);

            return CrawlSiteAsync(startUrl.AbsoluteUri, cancellationToken);
        }

        /// <summary>
        /// Crawls a website starting from the specified URL.
        /// </summary>
        /// <param name="startUrl">The url to start crawling from.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task CrawlSiteAsync(
            string startUrl,
            CancellationToken cancellationToken)
        {
            var baseUrl = new Uri(startUrl).GetLeftPart(UriPartial.Authority); // Base URL for same-origin policy
            var urlQueue = new Queue<string>([startUrl]);
            var visitedUrls = new Dictionary<string, UriCrawlResultModel>();
            var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });

            while (urlQueue.Count > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var url = urlQueue.Dequeue();
                if (visitedUrls.TryGetValue(url, out var info))
                {
                    // If already visited, increment hit count
                    _ = info.HitCount.Increment();
                    continue;
                }

                Console.WriteLine($"Crawling: {url}");
                await CrawlPageAsync(url, browser, visitedUrls, urlQueue, baseUrl);
            }

            await browser.CloseAsync();
            playwright.Dispose();
        }

        private static bool IsSameDomain(string url, string baseUrl)
        {
            // Ensure URL is same domain and not external
            return url.StartsWith(baseUrl, StringComparison.OrdinalIgnoreCase);
        }

        private static async Task CrawlPageAsync(
            string url,
            IBrowser browser,
            Dictionary<string, UriCrawlResultModel> visitedUrls,
            Queue<string> urlQueue,
            string baseUrl)
        {
            var page = await browser.NewPageAsync();

            var events = page.Events();
            var consoleMessages = new List<IConsoleMessage>();
            var pageErrors = new List<string>();
            var requestFailures = new List<IRequest>();

            using (_ = new CompositeDisposable
                   {
                       events.Console.Do(message => consoleMessages.Add(message)).Subscribe(),
                       events.PageError.Do(pageError => pageErrors.Add(pageError)).Subscribe(),
                       events.RequestFailed.Do(requestFailure => requestFailures.Add(requestFailure)).Subscribe()
                   })
            {
                var response = await page.GotoAsync(url).ConfigureAwait(false);

                if (response == null)
                {
                    return;
                }

                visitedUrls[url] = new UriCrawlResultModel(response.Status, consoleMessages, pageErrors, requestFailures);
                if (!response.Ok)
                {
                    return;
                }

                // Extract links on the page
                var anchorTags = await page.QuerySelectorAllAsync("a").ConfigureAwait(false);
                foreach (var anchorTag in anchorTags)
                {
                    var link = await page.EvaluateAsync<string>(
                            "(anchor) => { const url = new URL(anchor.href, document.baseURI); return url.href; }",
                            anchorTag)
                        .ConfigureAwait(false);

                    if (!string.IsNullOrWhiteSpace(link) && !link.StartsWith('#') && IsSameDomain(link, baseUrl) && !visitedUrls.ContainsKey(link))
                    {
                        urlQueue.Enqueue(link);
                    }
                }

                await page.CloseAsync();
            }
        }
    }
}
