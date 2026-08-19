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
    /// Binding logic for the command line arguments.
    /// </summary>
    internal sealed class CommandLineArgModelBinder : IBinderBase<CommandLineArgModel>
    {
        private readonly Option<IFileInfo> _docfxJsonPathOption;
        private readonly Option<PlaywrightBrowserTypeAndChannelCommandLineArgument> _playwrightBrowserTypeAndChannelOption;
        private readonly Option<bool?> _generatePdfOption;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineArgModelBinder"/> class.
        /// </summary>
        /// <param name="docfxJsonPathOption">Docfx JSON path to parse and bind against.</param>
        /// <param name="playwrightBrowserTypeAndChannelOption">Playwright browser type and channel to parse and bind against.</param>
        /// <param name="generatePdfOption">Generate PDF option to parse and bind against.</param>
#pragma warning disable GR0027 // Constructor should have a logging framework instance as the final parameter.
        public CommandLineArgModelBinder(
            Option<IFileInfo> docfxJsonPathOption,
            Option<PlaywrightBrowserTypeAndChannelCommandLineArgument> playwrightBrowserTypeAndChannelOption,
            Option<bool?> generatePdfOption)
        {
            ArgumentNullException.ThrowIfNull(docfxJsonPathOption);
            ArgumentNullException.ThrowIfNull(playwrightBrowserTypeAndChannelOption);
            ArgumentNullException.ThrowIfNull(generatePdfOption);

            _docfxJsonPathOption = docfxJsonPathOption;
            _playwrightBrowserTypeAndChannelOption = playwrightBrowserTypeAndChannelOption;
            _generatePdfOption = generatePdfOption;
        }
#pragma warning restore GR0027 // Constructor should have a logging framework instance as the final parameter.

        /// <inheritdoc/>
        public CommandLineArgModel GetBoundValue(ParseResult parseResult)
        {
            ArgumentNullException.ThrowIfNull(parseResult);

            var docfxJsonPath = parseResult.GetRequiredValue(_docfxJsonPathOption);
            var playwrightBrowserTypeAndChannel = parseResult.GetRequiredValue(_playwrightBrowserTypeAndChannelOption);
            var generatePdf = parseResult.GetRequiredValue(_generatePdfOption);

            return new CommandLineArgModel(
                docfxJsonPath,
                playwrightBrowserTypeAndChannel,
                generatePdf);
        }
    }
}
