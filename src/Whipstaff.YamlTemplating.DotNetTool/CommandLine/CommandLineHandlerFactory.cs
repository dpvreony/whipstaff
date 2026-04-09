// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.IO.Abstractions;
using System.Linq;
using Whipstaff.CommandLine;
using Whipstaff.CommandLine.FileSystemAbstractions;

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

            string[] yamlExtensions =
            [
                ".yaml",
                ".yml"
            ];

            var templateOption = OptionFactory.GetFileInfoOption(
                    "--template",
                    fileSystem,
                    static option =>
                    {
                        option.Aliases.Add("-t");
                        option.Description = "Path to the YAML template file";
                        option.Required = true;
                    })
                .SpecificFileExtensionsOnly(
                    fileSystem,
                    yamlExtensions)
                .ExistingOnly(fileSystem);

            var contentOption = OptionFactory.GetFileInfoOption(
                    "--content",
                    fileSystem,
                    static option =>
                    {
                        option.Aliases.Add("-c");
                        option.Description = "Path to the YAML content file to merge";
                        option.Required = true;
                    })
                .SpecificFileExtensionsOnly(
                    fileSystem,
                    yamlExtensions)
                .ExistingOnly(fileSystem);

            var outputOption = OptionFactory.GetFileInfoOption(
                    "--output",
                    fileSystem,
                    static option =>
                    {
                        option.Aliases.Add("-o");
                        option.Description = "Path to the output YAML file";
                        option.Required = true;
                    })
                .SpecificFileExtensionsOnly(
                    fileSystem,
                    yamlExtensions);

            var pathOption = new Option<string?>(
                "--path",
                "-p")
            {
                Description = "Dot-notation path within the content file where the template will be injected (e.g. steps.build.tasks). When omitted, the template and content files are merged at the root level.",
                Required = false
            };

            pathOption.Validators.Add(static result =>
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
