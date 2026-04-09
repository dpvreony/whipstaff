// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.IO;
using System.IO.Abstractions;
using Whipstaff.CommandLine;

namespace Whipstaff.YamlTemplating.DotNetTool.CommandLine
{
    /// <summary>
    /// Binding logic for the command line arguments.
    /// </summary>
    internal sealed class CommandLineArgModelBinder : IBinderBase<CommandLineArgModel>
    {
        private readonly Option<IFileInfo> _templateOption;
        private readonly Option<IFileInfo> _contentOption;
        private readonly Option<IFileInfo> _outputOption;
        private readonly Option<string?> _pathOption;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineArgModelBinder"/> class.
        /// </summary>
        /// <param name="templateOption">Template file option to parse and bind against.</param>
        /// <param name="contentOption">Content file option to parse and bind against.</param>
        /// <param name="outputOption">Output file option to parse and bind against.</param>
        /// <param name="pathOption">Optional YAML dot-notation path option to parse and bind against.</param>
#pragma warning disable GR0027 // Constructor should have a logging framework instance as the final parameter.
        public CommandLineArgModelBinder(
            Option<IFileInfo> templateOption,
            Option<IFileInfo> contentOption,
            Option<IFileInfo> outputOption,
            Option<string?> pathOption)
        {
            ArgumentNullException.ThrowIfNull(templateOption);
            ArgumentNullException.ThrowIfNull(contentOption);
            ArgumentNullException.ThrowIfNull(outputOption);
            ArgumentNullException.ThrowIfNull(pathOption);

            _templateOption = templateOption;
            _contentOption = contentOption;
            _outputOption = outputOption;
            _pathOption = pathOption;
        }
#pragma warning restore GR0027 // Constructor should have a logging framework instance as the final parameter.

        /// <inheritdoc/>
        public CommandLineArgModel GetBoundValue(ParseResult parseResult)
        {
            ArgumentNullException.ThrowIfNull(parseResult);

            var templatePath = parseResult.GetRequiredValue(_templateOption);
            var contentPath = parseResult.GetRequiredValue(_contentOption);
            var outputPath = parseResult.GetRequiredValue(_outputOption);
            var yamlPath = parseResult.GetValue(_pathOption);

            return new CommandLineArgModel(
                templatePath,
                contentPath,
                outputPath,
                yamlPath);
        }
    }
}
