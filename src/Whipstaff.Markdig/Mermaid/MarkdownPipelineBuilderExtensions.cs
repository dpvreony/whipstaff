﻿// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Markdig;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Whipstaff.Markdig.Settings;
using Whipstaff.Playwright;

namespace Whipstaff.Markdig.Mermaid
{
    /// <summary>
    /// Extension methods for <see cref="MarkdownPipelineBuilder"/>.
    /// </summary>
    public static class MarkdownPipelineBuilderExtensions
    {
        /// <summary>
        /// Adds the MermaidJs plugin to the pipeline.
        /// </summary>
        /// <param name="pipeline">Markdown Pipeline Builder to modify.</param>
        /// <param name="playwrightBrowserTypeAndChannel">Browser and channel type to use.</param>
        /// <returns>Modified Pipeline Builder.</returns>
        public static MarkdownPipelineBuilder UseMermaidJsExtension(
            this MarkdownPipelineBuilder pipeline,
            PlaywrightBrowserTypeAndChannel playwrightBrowserTypeAndChannel)
        {
            var defaultSettings = new MarkdownJsExtensionSettings(playwrightBrowserTypeAndChannel, OutputMode.Png);
            return UseMermaidJsExtension(
                pipeline,
                defaultSettings,
                new NullLoggerFactory());
        }

        /// <summary>
        /// Adds the MermaidJs plugin to the pipeline.
        /// </summary>
        /// <param name="pipeline">Markdown Pipeline Builder to modify.</param>
        /// <param name="settings">Settings to use for the extension.</param>
        /// <param name="loggerFactory">Logger Factory instance to use.</param>
        /// <returns>Modified Pipeline Builder.</returns>
        public static MarkdownPipelineBuilder UseMermaidJsExtension(
            this MarkdownPipelineBuilder pipeline,
            MarkdownJsExtensionSettings settings,
            ILoggerFactory loggerFactory)
        {
            ArgumentNullException.ThrowIfNull(pipeline);
            pipeline.Extensions.AddIfNotAlready(new MermaidJsExtension(
                settings,
                loggerFactory));
            return pipeline;
        }
    }
}
