﻿// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Whipstaff.Playwright;

namespace Whipstaff.Markdig.Settings
{
    /// <summary>
    /// Represents the settings for the MarkdownJs extension.
    /// </summary>
    /// <param name="PlaywrightBrowserTypeAndChannel">Browser and channel type to use.</param>
    /// <param name="OutputMode">Output format to use.</param>
    public sealed record MarkdownJsExtensionSettings(PlaywrightBrowserTypeAndChannel PlaywrightBrowserTypeAndChannel, OutputMode OutputMode);
}
