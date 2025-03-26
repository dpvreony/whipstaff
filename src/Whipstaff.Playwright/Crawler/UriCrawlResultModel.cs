// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Microsoft.Playwright;
using Whipstaff.Runtime.Counters;

namespace Whipstaff.Playwright.Crawler
{
    /// <summary>
    /// Model for the result of crawling a URI.
    /// </summary>
    /// <param name="StatusCode">The status code returned for the request.</param>
    /// <param name="ConsoleMessages">Console messages returned by the request, if any.</param>
    /// <param name="PageErrors">Page errors returned by the request, if any.</param>
    /// <param name="RequestFailures">Request failures relating to the original request, these will be resource (css, img, js, etc.) related.</param>
    public sealed record UriCrawlResultModel(int StatusCode, IList<IConsoleMessage> ConsoleMessages, IList<string> PageErrors, IList<IRequest> RequestFailures)
    {
        /// <summary>
        /// Gets the hit count for the URI.
        /// </summary>
        public IncrementOnlyInt HitCount { get; } = new(1);
    }
}
