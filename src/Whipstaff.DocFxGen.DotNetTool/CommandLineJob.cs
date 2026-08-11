// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO.Abstractions;
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
    /// Command line job for handling the creation of the Entity Framework Diagram.
    /// </summary>
    internal sealed class CommandLineJob : AbstractCommandLineHandler<CommandLineArgModel, CommandLineJobLogMessageActionsWrapper>
    {
        private readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineJob"/> class.
        /// </summary>
        /// <param name="loggerFactory">Microsoft Logging Logger factory instance.</param>
        /// <param name="commandLineJobLogMessageActionsWrapper">Wrapper for logging framework messages.</param>
        public CommandLineJob(
            ILoggerFactory loggerFactory,
            CommandLineJobLogMessageActionsWrapper commandLineJobLogMessageActionsWrapper)
            : base(commandLineJobLogMessageActionsWrapper)
        {
            ArgumentNullException.ThrowIfNull(loggerFactory);

            _loggerFactory = loggerFactory;
        }

        /// <inheritdoc/>
        protected override async Task<int> OnHandleCommandAsync(CommandLineArgModel commandLineArgModel, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(commandLineArgModel);

            const string configPath = "docfx.json";
            await DotnetApiCatalog.GenerateManagedReferenceYamlFiles(configPath).ConfigureAwait(false);

            var playwrightRenderer = PlaywrightRenderer.Default(_loggerFactory);
            var browserSession = await playwrightRenderer.GetBrowserSessionAsync(PlaywrightBrowserTypeAndChannel.Chrome())
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

            await Docset.Build("docfx.json", options);
            await Docset.Pdf("docfx.json", options);

            return 0;
        }
    }
}
