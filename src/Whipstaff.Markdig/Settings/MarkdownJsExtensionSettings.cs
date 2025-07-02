// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Whipstaff.Mermaid.Playwright;

namespace Whipstaff.Markdig.Settings
{
    /// <summary>
    /// Represents the settings for the MarkdownJs extension.
    /// </summary>
    /// <param name="BrowserSession">Browser session to render diagrams. Passed in as a cached object to reduce time on rendering multiple diagrams.</param>
    /// <param name="OutputMode">Output format to use.</param>
    public sealed record MarkdownJsExtensionSettings(IPlaywrightRendererBrowserInstance BrowserSession, OutputMode OutputMode);
}
