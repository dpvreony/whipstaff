// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.IO;
using System.IO.Abstractions;

namespace Whipstaff.CommandLine.MarkdownGen.DotNetTool.CommandLine
{
    /// <summary>
    /// Factory for creating the root command and binder.
    /// </summary>
    public sealed class CommandLineHandlerFactory : IRootCommandAndBinderFactory<CommandLineArgModelBinder>
    {
        /// <inheritdoc/>
        public RootCommandAndBinderModel<CommandLineArgModelBinder> GetRootCommandAndBinder(IFileSystem fileSystem)
        {
            ArgumentNullException.ThrowIfNull(fileSystem);

#pragma warning disable CA1861 // Avoid constant arrays as arguments
            var assemblyOption = new Option<FileInfo>(
                [
                    "--assembly-path",
                    "-a"
                ],
                "Path to the assembly containing the Command Line Information")
            {
                Required = true
            }.SpecificFileExtensionsOnly(
                fileSystem,
                [
                    ".exe",
                    ".dll"
                ])
                .ExistingOnly(fileSystem);

            var outputFilePathOption = new Option<FileInfo>(
                [
                    "--output-file-path",
                    "-o"
                ],
                "Path to the output file")
            {
                Required = true
            };

#pragma warning restore CA1861 // Avoid constant arrays as arguments

            var rootCommand = new RootCommand("Creates a Markdown help file from the Command Line Help Content.")
                {
                    assemblyOption,
                    outputFilePathOption,
                };

            return new RootCommandAndBinderModel<CommandLineArgModelBinder>(
                rootCommand,
                new CommandLineArgModelBinder(assemblyOption, outputFilePathOption));
        }
    }
}
