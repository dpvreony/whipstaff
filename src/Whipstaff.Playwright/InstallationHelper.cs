// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Playwright;

namespace Whipstaff.Playwright
{
    /// <summary>
    /// Helper class for installing Playwright browser. The out-of-the-box experience is a bit clunky.
    ///
    /// This attempts to make it a bit easier. By allowing a first run execution to install the browser.
    ///
    /// See:
    /// https://github.com/microsoft/playwright-dotnet/issues/2239
    /// https://github.com/microsoft/playwright-dotnet/issues/2286
    ///
    /// This also removes the need to have msbuild run the task which has been causing CodeQL failures.
    /// </summary>
    public sealed class InstallationHelper
    {
        private readonly Lazy<int> _lazyInstaller;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallationHelper"/> class.
        /// </summary>
        /// <param name="playwrightBrowserType">The browser to use as part of the process.</param>
        public InstallationHelper(PlaywrightBrowserType playwrightBrowserType)
        {
            var args = playwrightBrowserType switch
            {
                PlaywrightBrowserType.None => new[] { "install" },
                _ => new[] { "install", playwrightBrowserType.ToString().ToLowerInvariant() },
            };

            _lazyInstaller = new(() => Program.Main(args));
        }

        /// <summary>
        /// Wrapper to carry out an installation a single time.
        /// </summary>
        /// <returns>result code from Playwright install.</returns>
        public int InstallPlaywright()
        {
            return _lazyInstaller.Value;
        }
    }
}
