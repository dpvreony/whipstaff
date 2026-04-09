// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using VitaeCyclum.Actions.Tools.YamlGenerator.CommandLine;
using Whipstaff.CommandLine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace VitaeCyclum.Actions.Tools.YamlGenerator
{
    /// <summary>
    /// Command line job for handling the generation of YAML files by merging template and content files.
    /// </summary>
    internal sealed class CommandLineJob : AbstractCommandLineHandler<CommandLineArgModel, CommandLineJobLogMessageActionsWrapper>
    {
        private readonly IFileSystem _fileSystem;
        private readonly TimeProvider _timeProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineJob"/> class.
        /// </summary>
        /// <param name="fileSystem">File System abstraction.</param>
        /// <param name="timeProvider">Time provider for obtaining the current UTC date.</param>
        /// <param name="commandLineJobLogMessageActionsWrapper">Wrapper for logging framework messages.</param>
        public CommandLineJob(
            IFileSystem fileSystem,
            TimeProvider timeProvider,
            CommandLineJobLogMessageActionsWrapper commandLineJobLogMessageActionsWrapper)
            : base(commandLineJobLogMessageActionsWrapper)
        {
            ArgumentNullException.ThrowIfNull(fileSystem);
            ArgumentNullException.ThrowIfNull(timeProvider);

            _fileSystem = fileSystem;
            _timeProvider = timeProvider;
        }

        /// <inheritdoc/>
        protected override Task<int> OnHandleCommandAsync(CommandLineArgModel commandLineArgModel, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(commandLineArgModel);

            return Task.Run(
                () =>
                {
                    LogMessageActionsWrapper.StartingHandleCommand();

                    var deserializer = new DeserializerBuilder()
                        .WithNamingConvention(NullNamingConvention.Instance)
                        .Build();

                    string templateContent;
                    try
                    {
                        LogMessageActionsWrapper.ReadingTemplateFile();
                        templateContent = _fileSystem.File.ReadAllText(commandLineArgModel.TemplatePath.FullName);
                    }
                    catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
                    {
                        LogMessageActionsWrapper.FailedToReadTemplate(ex);
                        return 1;
                    }

                    string contentYaml;
                    try
                    {
                        LogMessageActionsWrapper.ReadingContentFile();
                        contentYaml = _fileSystem.File.ReadAllText(commandLineArgModel.ContentPath.FullName);
                    }
                    catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
                    {
                        LogMessageActionsWrapper.FailedToReadContent(ex);
                        return 1;
                    }

                    Dictionary<object, object> merged;
                    try
                    {
                        if (commandLineArgModel.YamlPath is not null)
                        {
                            LogMessageActionsWrapper.ValidatingYamlPath(commandLineArgModel.YamlPath);

                            var contentDict = deserializer.Deserialize<Dictionary<object, object>>(contentYaml)
                                ?? new Dictionary<object, object>();
                            var templateValue = deserializer.Deserialize<object>(templateContent);

                            var segments = commandLineArgModel.YamlPath.Split('.');
                            if (!TrySetAtPath(contentDict, segments, templateValue))
                            {
                                LogMessageActionsWrapper.InvalidYamlPath(commandLineArgModel.YamlPath);
                                return 1;
                            }

                            LogMessageActionsWrapper.InjectingAtPath(commandLineArgModel.YamlPath);
                            merged = contentDict;
                        }
                        else
                        {
                            LogMessageActionsWrapper.MergingYamlContent();

                            var templateDict = deserializer.Deserialize<Dictionary<object, object>>(templateContent)
                                ?? new Dictionary<object, object>();
                            var contentDict = deserializer.Deserialize<Dictionary<object, object>>(contentYaml)
                                ?? new Dictionary<object, object>();

                            merged = MergeDictionaries(templateDict, contentDict);
                        }
                    }
                    catch (Exception ex) when (ex is YamlDotNet.Core.YamlException or InvalidCastException)
                    {
                        LogMessageActionsWrapper.FailedToMerge(ex);
                        return 1;
                    }

                    try
                    {
                        LogMessageActionsWrapper.WritingOutputFile();

                        var serializer = new SerializerBuilder()
                            .WithNamingConvention(NullNamingConvention.Instance)
                            .Build();

                        var rawYaml = serializer.Serialize(merged);
                        var assembly = typeof(CommandLineJob).Assembly;
                        var toolName = assembly.GetName().Name ?? "vitaecyclum-yamlgen";
                        var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                            ?.InformationalVersion ?? "unknown";
                        var generationDate = _timeProvider.GetUtcNow().ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                        var header = $"# Generated by {toolName} v{version} on {generationDate}\n";
                        var outputYaml = header + rawYaml;
                        var outputPath = commandLineArgModel.OutputPath;
                        _ = _fileSystem.Directory.CreateDirectory(outputPath.DirectoryName!);
                        _fileSystem.File.WriteAllText(outputPath.FullName, outputYaml);
                    }
                    catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
                    {
                        LogMessageActionsWrapper.FailedToWriteOutput(ex);
                        return 1;
                    }

                    return 0;
                },
                cancellationToken);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Gripe", "GR0033:Do not use Object in a parameter declaration", Justification = "YamlDotNet untyped deserialization produces Dictionary<object, object>.")]
        private static bool TrySetAtPath(
            Dictionary<object, object> root,
            string[] segments,
            object? value)
        {
            if (segments.Length == 1)
            {
                root[segments[0]] = value!;
                return true;
            }

            if (!root.TryGetValue(segments[0], out var next)
                || next is not Dictionary<object, object> nextDict)
            {
                return false;
            }

            return TrySetAtPath(nextDict, segments[1..], value);
        }

        private static Dictionary<object, object> MergeDictionaries(
            Dictionary<object, object> template,
            Dictionary<object, object> content)
        {
            var result = new Dictionary<object, object>(template);

            foreach (var kvp in content)
            {
                if (result.TryGetValue(kvp.Key, out var existingValue)
                    && existingValue is Dictionary<object, object> existingDict
                    && kvp.Value is Dictionary<object, object> contentDict)
                {
                    result[kvp.Key] = MergeDictionaries(existingDict, contentDict);
                }
                else
                {
                    result[kvp.Key] = kvp.Value;
                }
            }

            return result;
        }
    }
}
