﻿// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Nito.AsyncEx.Synchronous;
using Whipstaff.Markdig.Settings;
using Whipstaff.Mermaid.Playwright;
using Whipstaff.Playwright;

namespace Whipstaff.Markdig.Mermaid
{
    /// <summary>
    /// HTML renderer for MermaidJS Code Blocks.
    /// </summary>
    public sealed class HtmlMermaidJsRenderer : HtmlObjectRenderer<MermaidCodeBlock>
    {
        private readonly PlaywrightRendererBrowserInstance _browserSession;
        private readonly MarkdownJsExtensionSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlMermaidJsRenderer"/> class.
        /// </summary>
        /// <param name="browserSession">Browser session to render diagrams. Passed in as a cached object to reduce time on rendering multiple diagrams.</param>
        /// <param name="settings">MermaidJS extension settings.</param>
        private HtmlMermaidJsRenderer(
            PlaywrightRendererBrowserInstance browserSession,
            MarkdownJsExtensionSettings settings)
        {
            ArgumentNullException.ThrowIfNull(browserSession);
            ArgumentNullException.ThrowIfNull(settings);

            _browserSession = browserSession;
            _settings = settings;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="HtmlMermaidJsRenderer"/> class.
        /// </summary>
        /// <param name="playwrightRenderer">Playwright MermaidJS Renderer.</param>
        /// <param name="settings">MermaidJS extension settings.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task<HtmlMermaidJsRenderer> CreateAsync(
            PlaywrightRenderer playwrightRenderer,
            MarkdownJsExtensionSettings settings)
        {
            ArgumentNullException.ThrowIfNull(playwrightRenderer);
            ArgumentNullException.ThrowIfNull(settings);

            return new HtmlMermaidJsRenderer(
                await playwrightRenderer.GetBrowserSessionAsync(settings.PlaywrightBrowserTypeAndChannel),
                settings);
        }

        /// <inheritdoc/>
        protected override void Write(HtmlRenderer renderer, MermaidCodeBlock obj)
        {
            ArgumentNullException.ThrowIfNull(renderer);
            ArgumentNullException.ThrowIfNull(obj);
            _ = renderer.EnsureLine();

            var mermaidMarkup = obj.Lines.ToSlice().Text;
            var responseModel = _browserSession.GetDiagram(mermaidMarkup)
                .WaitAndUnwrapException();

            if (responseModel == null)
            {
                return;
            }

            if (_settings.OutputMode == OutputMode.Png)
            {
                var imageBase64 = Convert.ToBase64String(responseModel.Png);

                var properties = new List<KeyValuePair<string, string?>>
                {
                    new("alt", "Mermaid Diagram"),
                    new("src", $"data:image/png;base64,{imageBase64}")
                };

                var attributes = new HtmlAttributes
                {
                    Properties = properties
                };

                _ = renderer.Write("<img")
                    .WriteAttributes(attributes)
                    .Write('>');

                _ = renderer.EnsureLine();
            }
            else
            {
                var svg = responseModel.Svg;
                _ = renderer.Write("<div>")
                    .Write(svg)
                    .Write("</div>")
                    .EnsureLine();
            }
        }
    }
}
