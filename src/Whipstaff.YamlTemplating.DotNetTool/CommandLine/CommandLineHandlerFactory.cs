// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using Whipstaff.CommandLine;

namespace Whipstaff.YamlTemplating.DotNetTool.CommandLine
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
            var templateOption = new Option<FileInfo>(
                "--template",
                "-t")
            {
                Description = "Path to the YAML template file",
                Required = true
            }.SpecificFileExtensionsOnly(
                fileSystem,
                [
                    ".yaml",
                    ".yml"
                ])
                .ExistingOnly(fileSystem);

            var contentOption = new Option<FileInfo>(
                "--content",
                "-c")
            {
                Description = "Path to the YAML content file to merge",
                Required = true
            }.SpecificFileExtensionsOnly(
                fileSystem,
                [
                    ".yaml",
                    ".yml"
                ])
                .ExistingOnly(fileSystem);

            var outputOption = new Option<FileInfo>(
                "--output",
                "-o")
            {
                Description = "Path to the output YAML file",
                Required = true
            };

#pragma warning restore CA1861 // Avoid constant arrays as arguments

            var pathOption = new Option<string?>(
                "--path",
                "-p")
            {
                Description = "Dot-notation path within the content file where the template will be injected (e.g. steps.build.tasks). When omitted, the template and content files are merged at the root level.",
                Required = false
            };

            pathOption.Validators.Add(result =>
            {
                var value = result.GetValueOrDefault<string>();
                if (value is null)
                {
                    return;
                }

                var segments = value.Split('.');
                if (segments.Any(string.IsNullOrWhiteSpace))
                {
                    result.AddError($"Invalid path '{value}'. Each segment must be non-empty and must not contain whitespace. Use dot notation, e.g. 'steps.build.tasks'.");
                }
            });

            var rootCommand = new RootCommand("Generates YAML files by merging template and content files.")
                {
                    templateOption,
                    contentOption,
                    outputOption,
                    pathOption,
                };

            return new RootCommandAndBinderModel<CommandLineArgModelBinder>(
                rootCommand,
                new CommandLineArgModelBinder(templateOption, contentOption, outputOption, pathOption));
        }
    }
}
