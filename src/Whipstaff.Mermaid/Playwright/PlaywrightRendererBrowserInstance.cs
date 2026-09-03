// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.IO.Abstractions;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Playwright;
using Whipstaff.Playwright;
using Whipstaff.Runtime.Extensions;

namespace Whipstaff.Mermaid.Playwright
{
    /// <summary>
    /// Represents a Playwright browser instance for rendering Mermaid diagrams.
    /// </summary>
    public sealed class PlaywrightRendererBrowserInstance : IPlaywrightRendererBrowserInstance
    {
        private readonly IPlaywright _playwright;
        private readonly IBrowser _browser;
        private readonly PlaywrightRendererBrowserInstanceLogMessageActionsWrapper _browserInstanceLogMessageActionsWrapper;
        private readonly TestServer _mermaidHttpServer;

        private bool _disposedValue;

        private PlaywrightRendererBrowserInstance(
            TestServer mermaidHttpServer,
            IPlaywright playwright,
            IBrowser browser,
            PlaywrightRendererBrowserInstanceLogMessageActionsWrapper browserInstanceLogMessageActionsWrapper)
        {
            _mermaidHttpServer = mermaidHttpServer;
            _playwright = playwright;
            _browser = browser;
            _browserInstanceLogMessageActionsWrapper = browserInstanceLogMessageActionsWrapper;
        }

        /// <summary>
        /// Gets a new instance of the PlaywrightRendererBrowserInstance based on the desired browser.
        /// Sets the page up ready to render Mermaid diagrams.
        /// </summary>
        /// <param name="mermaidHttpServer">The in memory mermaid HTTP server.</param>
        /// <param name="playwrightBrowserTypeAndChannel">Browser and channel type to use.</param>
        /// <param name="logMessageActionsWrapper">Log message actions wrapper.</param>
        /// <returns>Browser wrapper instance.</returns>
        public static async Task<PlaywrightRendererBrowserInstance> GetBrowserInstanceAsync(
            TestServer mermaidHttpServer,
            PlaywrightBrowserTypeAndChannel playwrightBrowserTypeAndChannel,
            PlaywrightRendererBrowserInstanceLogMessageActionsWrapper logMessageActionsWrapper)
        {
            ArgumentNullException.ThrowIfNull(mermaidHttpServer);
            ArgumentNullException.ThrowIfNull(playwrightBrowserTypeAndChannel);
            ArgumentNullException.ThrowIfNull(logMessageActionsWrapper);

            var playwright = await Microsoft.Playwright.Playwright.CreateAsync()
                .ConfigureAwait(false);
            var browser = await playwright.GetBrowserType(playwrightBrowserTypeAndChannel.PlaywrightBrowserType)
                .LaunchAsync(new() { Headless = true, Channel = playwrightBrowserTypeAndChannel.Channel });

            return new PlaywrightRendererBrowserInstance(
                mermaidHttpServer,
                playwright,
                browser,
                logMessageActionsWrapper);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);
            Dispose(disposing: false);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public async Task CreateDiagramAndWriteToFileAsync(
            IFileInfo sourceFile,
            IFileInfo targetFile)
        {
            ArgumentNullException.ThrowIfNull(sourceFile);
            ArgumentNullException.ThrowIfNull(targetFile);

            if (!sourceFile.Exists)
            {
                throw new ArgumentException("Source file does not exist", nameof(sourceFile));
            }

            if (sourceFile.FullName == targetFile.FullName)
            {
                throw new ArgumentException("Source and target files cannot be the same", nameof(targetFile));
            }

            if (targetFile.Exists)
            {
                throw new ArgumentException("Target file already exists", nameof(targetFile));
            }

            var diagram = await GetDiagramAsync(sourceFile)
                .ConfigureAwait(false);

            if (diagram == null)
            {
                throw new InvalidOperationException("Failed to get diagram");
            }

            await diagram.InternalToFileAsync(targetFile)
                .ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<GetDiagramResponseModel?> GetDiagramAsync(IFileInfo sourceFileInfo)
        {
            await using (var pageWrapper = await PlaywrightRendererPageInstance.GetPageInstanceAsync(
                _mermaidHttpServer,
                _browser,
                _browserInstanceLogMessageActionsWrapper)
                .ConfigureAwait(false))
            {
                return await pageWrapper.GetDiagramAsync(sourceFileInfo).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public async Task<GetDiagramResponseModel?> GetDiagramAsync(TextReader textReader)
        {
            await using (var pageWrapper = await PlaywrightRendererPageInstance.GetPageInstanceAsync(
                                 _mermaidHttpServer,
                                 _browser,
                                 _browserInstanceLogMessageActionsWrapper)
                             .ConfigureAwait(false))
            {
                return await pageWrapper.GetDiagramAsync(textReader).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public async Task<GetDiagramResponseModel?> GetDiagramAsync(string markdown)
        {
            await using (var pageWrapper = await PlaywrightRendererPageInstance.GetPageInstanceAsync(
                                 _mermaidHttpServer,
                                 _browser,
                                 _browserInstanceLogMessageActionsWrapper)
                             .ConfigureAwait(false))
            {
                return await pageWrapper.GetDiagramAsync(markdown).ConfigureAwait(false);
            }
        }

        private async ValueTask DisposeAsyncCore()
        {
            if (!_disposedValue)
            {
                await _browser.CloseAsync().ConfigureAwait(false);
                await _browser.DisposeAsync().ConfigureAwait(false);

                _disposedValue = true;
            }
        }

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _playwright?.Dispose();
                }

                _disposedValue = true;
            }
        }
    }
}
