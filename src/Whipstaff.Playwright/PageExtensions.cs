// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Whipstaff.Runtime.Extensions;

namespace Whipstaff.Playwright
{
    /// <summary>
    /// Extensions for the <see cref="IPage"/>.
    /// </summary>
    public static class PageExtensions
    {
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

            return InternalGetMhtmlAsString(page);
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

            var mhtmlContent = await InternalGetMhtmlAsString(page);
            await fileSystem.File.WriteAllTextAsync(
                    outputPath,
                    mhtmlContent,
                    Encoding.UTF8,
                    cancellationToken)
                .ConfigureAwait(false);
        }

        private static async Task<string> InternalGetMhtmlAsString(this IPage page)
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
