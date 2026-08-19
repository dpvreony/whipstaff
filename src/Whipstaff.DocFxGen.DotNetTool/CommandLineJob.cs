// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO.Abstractions;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Docfx;
using Docfx.Dotnet;
using Microsoft.Extensions.Logging;
using Whipstaff.CommandLine;
using Whipstaff.DocFxGen.DotNetTool.CommandLine;
using Whipstaff.Markdig.Mermaid;
using Whipstaff.Markdig.Settings;
using Whipstaff.Mermaid.Playwright;
using Whipstaff.Playwright;

namespace Whipstaff.DocFxGen.DotNetTool
{
    /// <summary>
    /// Command line job for handling the processing of DocFX.
    /// </summary>
    internal sealed class CommandLineJob : AbstractCommandLineHandler<CommandLineArgModel, CommandLineJobLogMessageActionsWrapper>
    {
        private readonly IFileSystem _fileSystem;
        private readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineJob"/> class.
        /// </summary>
        /// <param name="fileSystem">File system abstraction instance.</param>
        /// <param name="loggerFactory">Microsoft Logging Logger factory instance.</param>
        /// <param name="commandLineJobLogMessageActionsWrapper">Wrapper for logging framework messages.</param>
        public CommandLineJob(
            IFileSystem fileSystem,
            ILoggerFactory loggerFactory,
            CommandLineJobLogMessageActionsWrapper commandLineJobLogMessageActionsWrapper)
            : base(commandLineJobLogMessageActionsWrapper)
        {
            ArgumentNullException.ThrowIfNull(fileSystem);
            ArgumentNullException.ThrowIfNull(loggerFactory);

            _fileSystem = fileSystem;
            _loggerFactory = loggerFactory;
        }

        /// <inheritdoc/>
        protected override async Task<int> OnHandleCommandAsync(CommandLineArgModel commandLineArgModel, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(commandLineArgModel);

            var docfxJsonPath = commandLineArgModel.DocfxJsonPath.FullName;

            await DotnetApiCatalog.GenerateManagedReferenceYamlFiles(docfxJsonPath)
                .ConfigureAwait(false);

            var playwrightBrowserTypeAndChannel = PlaywrightBrowserTypeAndChannelHelper.GetPlaywrightBrowserTypeAndChannel(commandLineArgModel.PlaywrightBrowserTypeAndChannel);

            var playwrightRenderer = PlaywrightRenderer.Default(_loggerFactory);

            var browserSession = await playwrightRenderer.GetBrowserSessionAsync(playwrightBrowserTypeAndChannel)
                .ConfigureAwait(false);

            var markdownJsExtensionSettings = new MarkdownJsExtensionSettings(
                browserSession,
                OutputMode.Svg);

            var options = new BuildOptions
            {
                // Enable MermaidJS markdown extension
                ConfigureMarkdig = pipeline => pipeline.UseMermaidJsExtension(
                    markdownJsExtensionSettings,
                    _loggerFactory)
            };

            await Docset.Build(docfxJsonPath, options);

            var doPdf = commandLineArgModel.GeneratePdf;
            if (doPdf == null)
            {
                // scan docfx.json for pdf option
                doPdf = CheckDocFxForPdfSection(docfxJsonPath);
            }

            if (doPdf == true)
            {
                await Docset.Pdf(docfxJsonPath, options);
            }

            return 0;
        }

        private bool CheckDocFxForPdfSection(string docfxJsonPath)
        {
            try
            {
                using (var fileStream = _fileSystem.File.OpenRead(docfxJsonPath))
                using (var jsonReader = System.Text.Json.JsonDocument.Parse(fileStream))
                {
                    if (jsonReader.RootElement.ValueKind == JsonValueKind.Object
                        && jsonReader.RootElement.TryGetProperty("pdf", out var _))
                    {
                        return true;
                    }
                }
            }
#pragma warning disable RCS1075
#pragma warning disable CA1031
            catch
#pragma warning restore CA1031
#pragma warning restore RCS1075
            {
                // no op
            }

            return false;
        }
    }
}
