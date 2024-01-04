// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.CommandLine;
using System.IO;
using Whipstaff.CommandLine;

namespace Whipstaff.EntityFramework.Diagram.DotNetTool.CommandLine
{
    /// <summary>
    /// Factory for creating the root command and binder.
    /// </summary>
    public static class CommandLineHandlerFactory
    {
        public static RootCommandAndBinderModel<CommandLineArgModelBinder> GetRootCommandAndBinder()
        {
#pragma warning disable CA1861 // Avoid constant arrays as arguments
            var assemblyOption = new Option<FileInfo>(
                [
                    "--assembly-path",
                    "-a"
                ],
                "Path to the assembly containing the DbContext")
            {
                IsRequired = true
            }.SpecificFileExtensionsOnly(
                [
                    ".exe",
                    ".dll"
                ])
                .ExistingOnly();

            var diagramTypeOption = new Option<string>(
                new[]
                {
                    "--diagram-type",
                    "-t"
                },
                "Type of diagram to generate")
            {
                IsRequired = true
            };

            var outputFilePathOption = new Option<FileInfo>(
                [
                    "--output-file-path",
                    "-o"
                ],
                "Path to the output file")
            {
                IsRequired = true
            };

            var dbContextNameOption = new Option<string>(
                [
                    "--db-context-name",
                    "-n"
                ],
                "Name of the DbContext to use")
            {
                IsRequired = true
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
