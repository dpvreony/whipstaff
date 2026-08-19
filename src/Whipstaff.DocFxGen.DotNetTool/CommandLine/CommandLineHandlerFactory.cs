// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.IO.Abstractions;
using Whipstaff.CommandLine;

namespace Whipstaff.DocFxGen.DotNetTool.CommandLine
{
    /// <summary>
    /// Factory for creating the root command and binder.
    /// </summary>
    internal sealed class CommandLineHandlerFactory : IRootCommandAndBinderFactory<CommandLineArgModelBinder>
    {
        /// <inheritdoc/>
        public RootCommandAndBinderModel<CommandLineArgModelBinder> GetRootCommandAndBinder(IFileSystem fileSystem)
        {
            ArgumentNullException.ThrowIfNull(fileSystem);

#pragma warning disable CA1861 // Avoid constant arrays as arguments
            var docfxJsonPathOption = new Option<IFileInfo>(
                "--docfx-json-path",
                "-d")
            {
                Description = "Path to the Docfx JSON file",
                Required = true
            }.SpecificFileExtensionsOnly(
                fileSystem,
                [
                    ".json"
                ])
                .ExistingOnly(fileSystem);

            var playwrightBrowserTypeAndChannelOption = new Option<PlaywrightBrowserTypeAndChannelCommandLineArgument>(
                "--playwright-browser-type-and-channel")
            {
                Description = "The type and channel of the Playwright browser to use for generating the MermaidJs diagrams",
                Required = true
            };

            var generatePdfOption = new Option<bool?>(
                "--generate-pdf")
            {
                Description = "Whether to generate a PDF version of the Markdown help file",
                Required = false
            };

#pragma warning restore CA1861 // Avoid constant arrays as arguments

            var rootCommand = new RootCommand("Creates a Markdown help file from the Command Line Help Content.")
            {
                docfxJsonPathOption,
                playwrightBrowserTypeAndChannelOption,
                generatePdfOption
            };

            return new RootCommandAndBinderModel<CommandLineArgModelBinder>(
                rootCommand,
                new CommandLineArgModelBinder(
                    docfxJsonPathOption,
                    playwrightBrowserTypeAndChannelOption,
                    generatePdfOption));
        }
    }
}
