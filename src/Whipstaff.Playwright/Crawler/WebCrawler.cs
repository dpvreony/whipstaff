﻿// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
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
            var visitedUrls = new Dictionary<string, (int StatusCode, int HitCount)>();
            var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });

            while (urlQueue.Count > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var url = urlQueue.Dequeue();
                if (visitedUrls.TryGetValue(url, out var info))
                {
                    // If already visited, increment hit count
                    visitedUrls[url] = (info.StatusCode, ++info.HitCount);
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
            Dictionary<string, (int StatusCode, int HitCount)> visitedUrls,
            Queue<string> urlQueue,
            string baseUrl)
        {
            var page = await browser.NewPageAsync();
            var response = await page.GotoAsync(url).ConfigureAwait(false);

            if (response == null)
            {
                return;
            }

            visitedUrls[url] = (response.Status, 1);
            if (!response.Ok)
            {
                return;
            }

            // Extract links on the page
            var links = await page.EvaluateAsync<string[]>("Array.from(document.querySelectorAll('a')).map(a => a.href)");

            foreach (var link in links)
            {
                if (IsSameDomain(link, baseUrl) && !visitedUrls.ContainsKey(link))
                {
                    urlQueue.Enqueue(link);
                }
            }

            await page.CloseAsync();
        }
    }
}
