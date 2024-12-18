// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Playwright
{
    /// <summary>
    /// Gets the selected browser type for Playwright.
    /// </summary>
    public enum PlaywrightBrowserType
    {
        /// <summary>
        /// No value passed.
        /// </summary>
        None,

        /// <summary>
        /// Chromium browser.
        /// </summary>
        Chromium,

        /// <summary>
        /// Firefox browser.
        /// </summary>
        Firefox,

        /// <summary>
        /// WebKit browser.
        /// </summary>
        WebKit
    }
}
