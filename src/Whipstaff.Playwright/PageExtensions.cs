// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Playwright;

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
