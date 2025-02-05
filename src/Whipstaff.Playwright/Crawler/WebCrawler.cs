// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Playwright;

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
        public static async Task CrawlSiteAsync(
            string startUrl,
            CancellationToken cancellationToken)
        {
            var baseUrl = new Uri(startUrl).GetLeftPart(UriPartial.Authority); // Base URL for same-origin policy
            var urlQueue = new Queue<string>([startUrl]);
            var visitedUrls = new HashSet<string>();
            var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });

            while (urlQueue.Count > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var url = urlQueue.Dequeue();
                if (!visitedUrls.Add(url))
                {
                    // Skip if URL has already been visited
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

        private static async Task CrawlPageAsync(string url, IBrowser browser, HashSet<string> visitedUrls, Queue<string> urlQueue, string baseUrl)
        {
            var page = await browser.NewPageAsync();
            await page.GotoAsync(url);

            // Extract links on the page
            var links = await page.EvaluateAsync<string[]>("Array.from(document.querySelectorAll('a')).map(a => a.href)");

            foreach (var link in links)
            {
                if (IsSameDomain(link, baseUrl) && !visitedUrls.Contains(link))
                {
                    urlQueue.Enqueue(link);
                }
            }

            await page.CloseAsync();
        }
    }
}
