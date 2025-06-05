// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dhgms.DocFx.MermaidJs.Plugin.Settings;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Nito.AsyncEx.Synchronous;
using Whipstaff.Mermaid.Playwright;
using Whipstaff.Playwright;

namespace Dhgms.DocFx.MermaidJs.Plugin.Markdig
{
    /// <summary>
    /// HTML renderer for MermaidJS Code Blocks.
    /// </summary>
    public sealed class HtmlMermaidJsRenderer : HtmlObjectRenderer<MermaidCodeBlock>
    {
        private readonly PlaywrightRenderer _playwrightRenderer;
        private readonly PlaywrightRendererBrowserInstance _browserSession;
        private readonly MarkdownJsExtensionSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlMermaidJsRenderer"/> class.
        /// </summary>
        /// <param name="markdownContext">DocFX Markdown context.</param>
        /// <param name="playwrightRenderer">Playwright Renderer used to generate mermaid.</param>
        /// <param name="browserSession">Browser session to render diagrams. Passed in as a cached object to reduce time on rendering multiple diagrams.</param>
        /// <param name="settings">MermaidJS extension settings.</param>
        private HtmlMermaidJsRenderer(
            PlaywrightRenderer playwrightRenderer,
            PlaywrightRendererBrowserInstance browserSession,
            MarkdownJsExtensionSettings settings)
        {
            ArgumentNullException.ThrowIfNull(playwrightRenderer);
            ArgumentNullException.ThrowIfNull(browserSession);
            ArgumentNullException.ThrowIfNull(settings);

            _playwrightRenderer = playwrightRenderer;
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
                playwrightRenderer,
                await playwrightRenderer.GetBrowserSessionAsync(PlaywrightBrowserTypeAndChannel.ChromiumDefault()),
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
