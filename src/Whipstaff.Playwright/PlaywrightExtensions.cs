// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Playwright;
using Whipstaff.Mermaid.Playwright;

namespace Whipstaff.Playwright
{
    /// <summary>
    /// Extension methods for <see cref="IPlaywright"/>.
    /// </summary>
    public static class PlaywrightExtensions
    {
        /// <summary>
        /// Gets the browser type for the specified <see cref="PlaywrightBrowserType"/>.
        /// </summary>
        /// <param name="playwright">Playwright instance to get the browser type from.</param>
        /// <param name="playwrightBrowserType">The browser type to create.</param>
        /// <returns>A Playwright <see cref="IBrowserType"/> instance.</returns>
        public static IBrowserType GetBrowserType(this IPlaywright playwright, PlaywrightBrowserType playwrightBrowserType)
        {
            ArgumentNullException.ThrowIfNull(playwright);

            return playwrightBrowserType switch
            {
                PlaywrightBrowserType.Chromium => playwright.Chromium,
                PlaywrightBrowserType.Firefox => playwright.Firefox,
                PlaywrightBrowserType.WebKit => playwright.Webkit,
                _ => throw new ArgumentException("Failed to map PlaywrightBrowserType", nameof(playwrightBrowserType)),
            };
        }
    }
}
