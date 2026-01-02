// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Whipstaff.Playwright.CssAnalysis;
using Whipstaff.Runtime.Extensions;

namespace Whipstaff.Playwright
{
    /// <summary>
    /// Extensions for the <see cref="IPage"/>.
    /// </summary>
    public static class PageExtensions
    {
        /// <summary>
        /// Gets the CSS classes that are used in the DOM but not defined in any of the stylesheets.
        /// </summary>
        /// <param name="page">Page instance to scan.</param>
        /// <returns>Analysis of css class usage in the page.</returns>
        public static async Task<ClassDefinitionAnalysis> GetClassDefinitionAnalysisAsync(this IPage page)
        {
            ArgumentNullException.ThrowIfNull(page);

            // Run everything in the page context
            var json = await page.EvaluateAsync<string>(@"
                () => {
                    // Collect used classes in DOM
                    const used = new Set();
                    document.querySelectorAll('[class]').forEach(el => {
                        el.className.split(/\s+/).forEach(c => {
                            if (c.trim().length > 0) used.add(c.trim());
                        });
                    });

                    // Collect defined classes from stylesheets
                    const defined = new Set();
                    const classRegex = /\.([_a-zA-Z0-9-]+)/g;

                    for (const sheet of document.styleSheets) {
                        try {
                            for (const rule of sheet.cssRules || []) {
                                if (!rule.selectorText) continue;
                                let m;
                                while ((m = classRegex.exec(rule.selectorText)) !== null) {
                                    defined.add(m[1]);
                                }
                            }
                        } catch(e) {
                            // Ignore cross-origin stylesheets
                        }
                    }

                    // Return as JSON
                    return JSON.stringify({
                        used: Array.from(used),
                        defined: Array.from(defined)
                    });
                }
                ").ConfigureAwait(false);

            var doc = System.Text.Json.JsonDocument.Parse(json);
            var used = doc.RootElement.GetProperty("used").EnumerateArray().Select(x => x.GetString()!).ToHashSet();
            var defined = doc.RootElement.GetProperty("defined").EnumerateArray().Select(x => x.GetString()!).ToHashSet();
            var undefined = new HashSet<string>(used);
            undefined.ExceptWith(defined);

            return new ClassDefinitionAnalysis(used, defined, undefined);
        }

        /// <summary>
        /// Enumerates all the img tags that have an incomplete alt attribute.
        /// </summary>
        /// <param name="page">Page instance to scan.</param>
        /// <returns>Enumerator for Img tags.</returns>
        public static IAsyncEnumerable<IElementHandle> EnumerateImgTagsWithIncompleteAltAttributeAsync(this IPage page)
        {
            ArgumentNullException.ThrowIfNull(page);

            return InternalEnumerateImgTagsWithIncompleteAltAttributeAsync(page);
        }

        /// <summary>
        /// Gets the MHTML content of the page as a string.
        /// </summary>
        /// <param name="page">Page to process.</param>
        /// <returns>MHTML content as a string.</returns>
        public static Task<string> GetMhtmlAsStringAsync(this IPage page)
        {
            ArgumentNullException.ThrowIfNull(page);

            return InternalGetMhtmlAsStringAsync(page);
        }

        /// <summary>
        /// Saves a page as MHTML to the specified path.
        /// </summary>
        /// <param name="page">Page to process.</param>
        /// <param name="fileSystem">Filesystem abstraction to save the file to.</param>
        /// <param name="outputPath">Output path for the file to save to.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task SaveAsMhtmlAsync(this IPage page, IFileSystem fileSystem, string outputPath, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(page);
            ArgumentNullException.ThrowIfNull(fileSystem);
            outputPath.ThrowIfNullOrWhitespace();

            var mhtmlContent = await InternalGetMhtmlAsStringAsync(page);
            await fileSystem.File.WriteAllTextAsync(
                    outputPath,
                    mhtmlContent,
                    Encoding.UTF8,
                    cancellationToken)
                .ConfigureAwait(false);
        }

        private static async Task<string> InternalGetMhtmlAsStringAsync(this IPage page)
        {
            var context = page.Context;
            await using (var cdpSession = await context.NewCDPSessionAsync(page)
                             .ConfigureAwait(false))
            {
                _ = await cdpSession.SendAsync("Page.enable");

                var result = await cdpSession.SendAsync("Page.captureSnapshot", new Dictionary<string, object>
                {
                    {
                        "format",
                        "mhtml"
                    }
                });

                if (result == null)
                {
                    throw new InvalidOperationException("Failed to extract MHTML data.");
                }

                var mhtmlContent = result.Value.GetProperty("data").GetString();

                if (string.IsNullOrEmpty(mhtmlContent))
                {
                    throw new InvalidOperationException("MHTML data is empty.");
                }

                return mhtmlContent;
            }
        }

        private static async IAsyncEnumerable<IElementHandle> InternalEnumerateImgTagsWithIncompleteAltAttributeAsync(this IPage page)
        {
            var imgTags = await page.QuerySelectorAllAsync("img").ConfigureAwait(false);

            foreach (var imgTag in imgTags)
            {
                var altText = await imgTag.GetAttributeAsync("alt");
                if (string.IsNullOrWhiteSpace(altText) || !altText.EndsWith('.'))
                {
                    yield return imgTag;
                }
            }
        }
    }
}
