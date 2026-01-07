// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Statiq.Common;
using Whipstaff.Mermaid.HttpServer;
using Whipstaff.Mermaid.Playwright;
using Whipstaff.Playwright;

namespace Whipstaff.Statiq.Mermaid
{
    /// <summary>
    /// Statiq module for producing mermaid diagrams.
    /// </summary>
    public sealed class MermaidDiagramModule : Module, IDisposable
    {
        private readonly System.IO.Abstractions.IFileSystem _fileSystem;
        private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);
        private PlaywrightRendererBrowserInstance? _browser;
        private bool _disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="MermaidDiagramModule"/> class.
        /// </summary>
        /// <param name="fileSystem">File system abstraction.</param>
#pragma warning disable GR0027 // Constructor should have a logging framework instance as the final parameter.
        public MermaidDiagramModule(System.IO.Abstractions.IFileSystem fileSystem)
        {
            ArgumentNullException.ThrowIfNull(fileSystem);
            _fileSystem = fileSystem;
        }
#pragma warning restore GR0027 // Constructor should have a logging framework instance as the final parameter.

        /// <inheritdoc/>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
        }

        /// <inheritdoc />
        protected override async Task<IEnumerable<IDocument>> ExecuteInputAsync(IDocument input, IExecutionContext context)
        {
            ArgumentNullException.ThrowIfNull(input);
            ArgumentNullException.ThrowIfNull(context);

            context.LogInformation(input, "Starting Mermaid Diagram CLI Module");

            var path = _fileSystem.Path;
            var inputFilename = path.GetFullPath(input.Source.FullPath);

            var outputFullPath = path.GetFullPath(context.FileSystem.OutputPath.FullPath);

            var destination = input.Destination.FullPath;

            // this is taking the input folder, the output folder
            var outputFilename = path.Combine(
                outputFullPath,
                destination);

            // and reversing the path separator on the output
            outputFilename = path.GetFullPath(outputFilename);

            // finally replace the file extension
            // could be a setting for now assume svg
            // might be a better way of doing all the output steps
            // within the Statiq Context would need to dig.
            outputFilename = path.ChangeExtension(
                outputFilename,
                ".svg");

            var targetDir = path.GetDirectoryName(outputFilename);

            if (targetDir == null)
            {
                throw new InvalidOperationException("failed to work out target directory");
            }

            var directory = _fileSystem.Directory;
            if (!directory.Exists(targetDir))
            {
                _ = directory.CreateDirectory(targetDir);
            }

            var browser = await GetBrowserInstanceAsync(context)
                .ConfigureAwait(false);

            var markdown = await _fileSystem.File.ReadAllTextAsync(inputFilename);

            var diagramResponse = await browser.GetDiagramAsync(markdown)
                .ConfigureAwait(false);

            if (diagramResponse == null)
            {
                throw new InvalidOperationException("Failed to generate diagram");
            }

            await _fileSystem.File.WriteAllTextAsync(
                outputFilename,
                diagramResponse.Svg)
                .ConfigureAwait(false);

            // keeping the working the same as the previous cli version
            // could return the response object in future.
            return Array.Empty<IDocument>();
        }

        private async Task<PlaywrightRendererBrowserInstance> GetBrowserInstanceAsync(IExecutionContext context)
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                var logMessageActions = new PlaywrightRendererBrowserInstanceLogMessageActions();
                var loggerFactory = context.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<PlaywrightRendererBrowserInstance>();
                var logMessageActionsWrapper = new PlaywrightRendererBrowserInstanceLogMessageActionsWrapper(logMessageActions, logger);
                var mermaidHttpServer = MermaidHttpServerFactory.GetTestServer(loggerFactory, new FileSystem());

                var playwrightRenderer = new Whipstaff.Mermaid.Playwright.PlaywrightRenderer(
                    mermaidHttpServer,
                    logMessageActionsWrapper);

                _browser = await playwrightRenderer.GetBrowserSessionAsync(PlaywrightBrowserTypeAndChannel.ChromiumDefault())
                    .ConfigureAwait(false);

                return _browser;
            }
            finally
            {
                _ = _semaphoreSlim.Release();
            }
        }

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _semaphoreSlim.Dispose();
                }

                _disposedValue = true;
            }
        }
    }
}
