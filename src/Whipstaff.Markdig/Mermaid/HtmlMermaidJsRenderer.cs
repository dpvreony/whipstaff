// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Microsoft.Extensions.Logging;
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
        private readonly IPlaywrightRendererBrowserInstance _browserSession;
        private readonly MarkdownJsExtensionSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlMermaidJsRenderer"/> class.
        /// </summary>
        /// <param name="browserSession">Browser session to render diagrams. Passed in as a cached object to reduce time on rendering multiple diagrams.</param>
        /// <param name="settings">MermaidJS extension settings.</param>
#pragma warning disable GR0027 // Constructor should have a logging framework instance as the final parameter.
        private HtmlMermaidJsRenderer(
            IPlaywrightRendererBrowserInstance browserSession,
            MarkdownJsExtensionSettings settings)
        {
            ArgumentNullException.ThrowIfNull(browserSession);
            ArgumentNullException.ThrowIfNull(settings);

            _browserSession = browserSession;
            _settings = settings;
        }
#pragma warning restore GR0027 // Constructor should have a logging framework instance as the final parameter.

        /// <summary>
        /// Creates a new instance of the <see cref="HtmlMermaidJsRenderer"/> class.
        /// </summary>
        /// <param name="settings">MermaidJS extension settings.</param>
        /// <param name="loggerFactory">Logger factory instance to hook up to. Typically, the one being used by the host application.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static Task<HtmlMermaidJsRenderer> CreateAsync(
            MarkdownJsExtensionSettings settings,
            ILoggerFactory loggerFactory)
        {
            ArgumentNullException.ThrowIfNull(settings);
            ArgumentNullException.ThrowIfNull(loggerFactory);

            return Task.FromResult(new HtmlMermaidJsRenderer(
                settings.BrowserSession,
                settings));
        }

        /// <inheritdoc/>
        protected override void Write(HtmlRenderer renderer, MermaidCodeBlock obj)
        {
            ArgumentNullException.ThrowIfNull(renderer);
            ArgumentNullException.ThrowIfNull(obj);
            _ = renderer.EnsureLine();

            var mermaidMarkup = obj.Lines.ToSlice().Text;
            var responseModel = _browserSession.GetDiagramAsync(mermaidMarkup)
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
                    new("alt", "Mermaid Diagram"), new("src", $"data:image/png;base64,{imageBase64}")
                };

                var attributes = new HtmlAttributes { Properties = properties };

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
