// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.CommandLine;
using System.IO;
using System.IO.Abstractions;
using Whipstaff.CommandLine;

namespace Whipstaff.EntityFramework.Diagram.DotNetTool.CommandLine
{
    /// <summary>
    /// Factory for creating the root command and binder.
    /// </summary>
    internal sealed class CommandLineHandlerFactory : IRootCommandAndBinderFactory<CommandLineArgModelBinder>
    {
        /// <inheritdoc/>
        public RootCommandAndBinderModel<CommandLineArgModelBinder> GetRootCommandAndBinder(IFileSystem fileSystem)
        {
#pragma warning disable CA1861 // Avoid constant arrays as arguments
            var assemblyOption = new Option<FileInfo>(
                "--assembly-path",
                "-a")
            {
                Description = "Path to the assembly containing the DbContext",
                Required = true
            }.SpecificFileExtensionsOnly(
                fileSystem,
                [
                    ".exe",
                    ".dll"
                ])
                .ExistingOnly(fileSystem);

            var diagramTypeOption = new Option<string>(
                "--diagram-type",
                "-t")
            {
                Description = "Type of diagram to generate",
                Required = true
            };

            var outputFilePathOption = new Option<FileInfo>(
                "--output-file-path",
                "-o")
            {
                Description = "Path to the output file",
                Required = true
            };

            var dbContextNameOption = new Option<string>(
                "--db-context-name",
                "-n")
            {
                Description = "Name of the DbContext to use",
                Required = true
            };
#pragma warning restore CA1861 // Avoid constant arrays as arguments

            var rootCommand = new RootCommand("Creates an Entity Framework Diagram from a DbContext")
                {
                    assemblyOption,
                    dbContextNameOption,
                    outputFilePathOption,
                    diagramTypeOption,
                };

            return new RootCommandAndBinderModel<CommandLineArgModelBinder>(
                rootCommand,
                new CommandLineArgModelBinder(assemblyOption, dbContextNameOption, outputFilePathOption, diagramTypeOption));
        }
    }
}
