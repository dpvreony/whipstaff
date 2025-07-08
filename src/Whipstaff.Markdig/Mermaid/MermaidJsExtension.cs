// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Markdig;
using Markdig.Renderers;
using Microsoft.Extensions.Logging;
using Nito.AsyncEx.Synchronous;
using Whipstaff.Markdig.Settings;
using Whipstaff.Mermaid.HttpServer;
using Whipstaff.Mermaid.Playwright;
using Whipstaff.Playwright;

namespace Whipstaff.Markdig.Mermaid
{
    /// <summary>
    /// Handles the registration of the MermaidJs plugin into Markdig.
    /// </summary>
    public sealed class MermaidJsExtension : IMarkdownExtension
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly MarkdownJsExtensionSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="MermaidJsExtension"/> class.
        /// </summary>
        /// <param name="settings">Settings for the Markdown JS extension.</param>
        /// <param name="loggerFactory">NET core logging factory.</param>
        public MermaidJsExtension(
            MarkdownJsExtensionSettings settings,
            ILoggerFactory loggerFactory)
        {
            ArgumentNullException.ThrowIfNull(settings);
            ArgumentNullException.ThrowIfNull(loggerFactory);

            _settings = settings;
            _loggerFactory = loggerFactory;
        }

        /// <inheritdoc/>
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            ArgumentNullException.ThrowIfNull(pipeline);

            if (pipeline.BlockParsers.Contains<MermaidJsBlockParser>())
            {
                return;
            }

            // we need it before the fenced block code parser.
            pipeline.BlockParsers.Insert(0, new MermaidJsBlockParser());
        }

        /// <inheritdoc/>
        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            ArgumentNullException.ThrowIfNull(pipeline);
            ArgumentNullException.ThrowIfNull(renderer);

            if (renderer is HtmlRenderer htmlRenderer)
            {
                // Must be inserted before FencedCodeBlockRenderer
                var htmlMermaidJsRenderer = HtmlMermaidJsRenderer.CreateAsync(
                    _settings,
                    _loggerFactory)
                    .WaitAndUnwrapException();

                htmlRenderer.ObjectRenderers.Insert(0, htmlMermaidJsRenderer);
            }
        }
    }
}
