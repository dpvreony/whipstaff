// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.CommandLine.Binding;
using System.IO;

namespace Whipstaff.CommandLine.MarkdownGen.DotNetTool.CommandLine
{
    /// <summary>
    /// Binding logic for the command line arguments.
    /// </summary>
    internal sealed class CommandLineArgModelBinder : IBinderBase<CommandLineArgModel>
    {
        private readonly Option<FileInfo> _assemblyOption;
        private readonly Option<FileInfo> _outputFilePathOption;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineArgModelBinder"/> class.
        /// </summary>
        /// <param name="assemblyOption">Assembly to parse and bind against.</param>
        /// <param name="outputFilePathOption">Output file path to parse and bind against.</param>
        public CommandLineArgModelBinder(
            Option<FileInfo> assemblyOption,
            Option<FileInfo> outputFilePathOption)
        {
            ArgumentNullException.ThrowIfNull(assemblyOption);
            ArgumentNullException.ThrowIfNull(outputFilePathOption);

            _assemblyOption = assemblyOption;
            _outputFilePathOption = outputFilePathOption;
        }

        /// <inheritdoc/>
        public CommandLineArgModel GetBoundValue(ParseResult parseResult)
        {
            ArgumentNullException.ThrowIfNull(parseResult);

            var assembly = parseResult.GetRequiredValue(_assemblyOption);
            var outputFilePath = parseResult.GetRequiredValue(_outputFilePathOption);

            return new CommandLineArgModel(
                assembly,
                outputFilePath);
        }
    }
}
