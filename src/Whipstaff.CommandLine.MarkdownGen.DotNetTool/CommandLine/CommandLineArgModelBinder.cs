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
    public sealed class CommandLineArgModelBinder : BinderBase<CommandLineArgModel>
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
        protected override CommandLineArgModel GetBoundValue(BindingContext bindingContext)
        {
            ArgumentNullException.ThrowIfNull(bindingContext);

            var assembly = bindingContext.ParseResult.GetValueForOption(_assemblyOption);
            var outputFilePath = bindingContext.ParseResult.GetValueForOption(_outputFilePathOption);

            // TODO: review how options behave when not specified
            return new CommandLineArgModel(
                assembly!,
                outputFilePath!);
        }
    }
}
