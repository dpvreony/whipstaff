// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Dhgms.DocFx.MermaidJs.Plugin.Settings
{
    /// <summary>
    /// Represents the settings for the MarkdownJs extension.
    /// </summary>
    /// <param name="OutputMode">Output format to use.</param>
    public sealed record MarkdownJsExtensionSettings(OutputMode OutputMode);
}
