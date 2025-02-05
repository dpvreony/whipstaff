using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Whipstaff.Playwright.Crawler
{
    public sealed class EventCrawlingWebCrawler
    {
        private readonly HashSet<string> visitedUrls = new();
        private readonly Queue<string> urlQueue = new();
        private readonly string baseUrl;
        private readonly IPlaywright playwright;
        private readonly IBrowser browser;

        public EventCrawlingWebCrawler(string startUrl)
        {
            baseUrl = new Uri(startUrl).GetLeftPart(UriPartial.Authority);
            urlQueue.Enqueue(startUrl);
        }

        public async Task StartCrawlingWithEventsAsync()
        {
            playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });

            while (urlQueue.Count > 0)
            {
                var url = urlQueue.Dequeue();
                if (!visitedUrls.Add(url))
                {
                    continue; // Skip if URL has already been visited
                }

                Console.WriteLine($"Crawling: {url}");
                await CrawlPageWithEventsAsync(url);
            }

            await browser.CloseAsync();
            playwright.Dispose();
        }

        private async Task CrawlPageWithEventsAsync(string url)
        {
            var page = await browser.NewPageAsync();

            // Register event listeners for rendering events
            page.Request += (sender, e) => Console.WriteLine($"Network request for {e.Request.Url}");

            page.Load += (sender, e) => Console.WriteLine("Page load event fired");
            page.DOMContentLoaded += (sender, e) => Console.WriteLine("DOMContentLoaded event fired");

            await page.GotoAsync(url);

            // Inject MutationObserver for finer DOM change monitoring
            await page.EvaluateAsync(@"() => {
            const observer = new MutationObserver(mutations => {
                mutations.forEach(mutation => {
                    console.log(`DOM mutation observed: ${mutation.type}`);
                });
            });
            observer.observe(document.body, { attributes: true, childList: true, subtree: true });
        }");

            // Wait for rendering events to ensure page is fully rendered
            await page.WaitForLoadStateAsync(LoadState.Load);

            // Extract and queue new links on the page
            var links = await page.EvaluateAsync<string[]>(
                "Array.from(document.querySelectorAll('a')).map(a => a.href)");
            foreach (var link in links)
            {
                if (IsSameDomain(link) && !visitedUrls.Contains(link))
                {
                    urlQueue.Enqueue(link);
                }
            }

            await page.CloseAsync();
        }

        private bool IsSameDomain(string url)
        {
            // Only crawl URLs in the same domain
            return url.StartsWith(baseUrl, StringComparison.OrdinalIgnoreCase);
        }
    }
}
