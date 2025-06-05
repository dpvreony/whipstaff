// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.IO.Abstractions;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using Whipstaff.Mermaid.HttpServer;
using Whipstaff.Playwright;
using Whipstaff.Runtime.Extensions;

namespace Whipstaff.Mermaid.Playwright
{
    /// <summary>
    /// Markdown renderer using Playwright.
    /// </summary>
    public sealed class PlaywrightRenderer
    {
        private readonly MermaidHttpServer _mermaidHttpServerFactory;
        private readonly PlaywrightRendererLogMessageActionsWrapper _logMessageActionsWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaywrightRenderer"/> class.
        /// </summary>
        /// <param name="mermaidHttpServer">In memory http server instance for Mermaid.</param>
        /// <param name="logMessageActionsWrapper">Log message actions wrapper.</param>
        public PlaywrightRenderer(
            MermaidHttpServer mermaidHttpServer,
            PlaywrightRendererLogMessageActionsWrapper logMessageActionsWrapper)
        {
            ArgumentNullException.ThrowIfNull(mermaidHttpServer);
            ArgumentNullException.ThrowIfNull(logMessageActionsWrapper);
            _mermaidHttpServerFactory = mermaidHttpServer;
            _logMessageActionsWrapper = logMessageActionsWrapper;
        }

        /// <summary>
        /// Create a default instance of the PlaywrightRenderer using the InMemory Test Http Server.
        /// </summary>
        /// <param name="loggerFactory">Logger factory instance to hook up to. Typically, the one being used by the host application.</param>
        /// <returns>Instance of the <see cref="PlaywrightRenderer"/> class.</returns>
        public static PlaywrightRenderer Default(ILoggerFactory loggerFactory)
        {
            ArgumentNullException.ThrowIfNull(loggerFactory);

            return new(
                MermaidHttpServerFactory.GetTestServer(loggerFactory),
                new PlaywrightRendererLogMessageActionsWrapper(
                    new PlaywrightRendererLogMessageActions(),
                    loggerFactory.CreateLogger<PlaywrightRenderer>()));
        }

        /// <summary>
        /// Gets a new browser session for the specified browser type and channel. Use this method if you want to generate multiple diagrams as it reduces the browser operations.
        /// </summary>
        /// <param name="playwrightBrowserTypeAndChannel">Browser and channel type to use.</param>
        /// <returns>Browser session.</returns>
        public async Task<PlaywrightRendererBrowserInstance> GetBrowserSessionAsync(PlaywrightBrowserTypeAndChannel playwrightBrowserTypeAndChannel)
        {
            return await PlaywrightRendererBrowserInstance.GetBrowserInstance(
                _mermaidHttpServerFactory,
                playwrightBrowserTypeAndChannel,
                _logMessageActionsWrapper);
        }
    }
}
