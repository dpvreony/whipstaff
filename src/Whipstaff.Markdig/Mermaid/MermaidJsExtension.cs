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
        private readonly MarkdownJsExtensionSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="MermaidJsExtension"/> class.
        /// </summary>
        /// <param name="settings">Settings for the Markdown JS extension.</param>
        public MermaidJsExtension(MarkdownJsExtensionSettings settings)
        {
            ArgumentNullException.ThrowIfNull(settings);

            _settings = settings;
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
        public void Setup(
            MarkdownPipeline pipeline,
            IMarkdownRenderer renderer)
        {
            ArgumentNullException.ThrowIfNull(pipeline);
            ArgumentNullException.ThrowIfNull(renderer);

            if (renderer is HtmlRenderer htmlRenderer)
            {
                // Must be inserted before FencedCodeBlockRenderer
                var htmlMermaidJsRenderer = HtmlMermaidJsRenderer.CreateAsync(_settings).WaitAndUnwrapException();

                htmlRenderer.ObjectRenderers.Insert(0, htmlMermaidJsRenderer);
            }
        }
    }
}
