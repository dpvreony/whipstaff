// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.CommandLine.Binding;
using System.IO;
using Whipstaff.CommandLine;

namespace Whipstaff.EntityFramework.Diagram.DotNetTool.CommandLine
{
    /// <summary>
    /// Binding logic for the command line arguments.
    /// </summary>
    internal sealed class CommandLineArgModelBinder : IBinderBase<CommandLineArgModel>
    {
        private readonly Option<FileInfo> _assemblyOption;
        private readonly Option<string> _diagramTypeOption;
        private readonly Option<FileInfo> _outputFilePathOption;
        private readonly Option<string> _dbContextNameOption;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineArgModelBinder"/> class.
        /// </summary>
        /// <param name="assemblyOption">Assembly to parse and bind against.</param>
        /// <param name="dbContextNameOption">Name of the db context to parse and bind against.</param>
        /// <param name="outputFilePathOption">Output file path to parse and bind against.</param>
        /// <param name="diagramTypeOption">Diagram type to parse and bind against.</param>
        public CommandLineArgModelBinder(
            Option<FileInfo> assemblyOption,
            Option<string> dbContextNameOption,
            Option<FileInfo> outputFilePathOption,
            Option<string> diagramTypeOption)
        {
            ArgumentNullException.ThrowIfNull(assemblyOption);
            ArgumentNullException.ThrowIfNull(dbContextNameOption);
            ArgumentNullException.ThrowIfNull(outputFilePathOption);
            ArgumentNullException.ThrowIfNull(diagramTypeOption);

            _assemblyOption = assemblyOption;
            _dbContextNameOption = dbContextNameOption;
            _outputFilePathOption = outputFilePathOption;
            _diagramTypeOption = diagramTypeOption;
        }

        /// <inheritdoc/>
        public CommandLineArgModel GetBoundValue(ParseResult parseResult)
        {
            ArgumentNullException.ThrowIfNull(parseResult);

            var assembly = parseResult.GetValue(_assemblyOption);
            var dbContextName = parseResult.GetValue(_dbContextNameOption);
            var outputFilePath = parseResult.GetValue(_outputFilePathOption);
            var diagramType = parseResult.GetValue(_diagramTypeOption);

            return new CommandLineArgModel(
                assembly!,
                dbContextName!,
                outputFilePath!,
                diagramType!);
        }
    }
}
