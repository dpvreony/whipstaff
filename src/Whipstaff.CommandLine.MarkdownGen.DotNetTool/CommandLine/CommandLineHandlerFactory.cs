// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.CommandLine;
using System.IO;

namespace Whipstaff.CommandLine.MarkdownGen.DotNetTool.CommandLine
{
    /// <summary>
    /// Factory for creating the root command and binder.
    /// </summary>
    public static class CommandLineHandlerFactory
    {
        /// <summary>
        /// Gets the root command and binder for running the CLI tool.
        /// </summary>
        /// <returns>Root command and binder.</returns>
        public static RootCommandAndBinderModel<CommandLineArgModelBinder> GetRootCommandAndBinder()
        {
#pragma warning disable CA1861 // Avoid constant arrays as arguments
            var assemblyOption = new Option<FileInfo>(
                [
                    "--assembly-path",
                    "-a"
                ],
                "Path to the assembly containing the Command Line Information")
            {
                IsRequired = true
            }.SpecificFileExtensionsOnly(
                [
                    ".exe",
                    ".dll"
                ])
                .ExistingOnly();

            var outputFilePathOption = new Option<FileInfo>(
                [
                    "--output-file-path",
                    "-o"
                ],
                "Path to the output file")
            {
                IsRequired = true
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
