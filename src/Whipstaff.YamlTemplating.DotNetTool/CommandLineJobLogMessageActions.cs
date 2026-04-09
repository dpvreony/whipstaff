// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;
using Whipstaff.Core.Logging;

namespace VitaeCyclum.Actions.Tools.YamlGenerator
{
    /// <summary>
    /// Log message actions for <see cref="CommandLineJob"/>.
    /// </summary>
    internal sealed class CommandLineJobLogMessageActions : ILogMessageActions<CommandLineJob>
    {
        private readonly Action<ILogger, Exception?> _startingHandleCommand;
        private readonly Action<ILogger, Exception?> _readingTemplateFile;
        private readonly Action<ILogger, Exception?> _readingContentFile;
        private readonly Action<ILogger, Exception?> _mergingYamlContent;
        private readonly Action<ILogger, Exception?> _writingOutputFile;
        private readonly Action<ILogger, Exception?> _failedToReadTemplate;
        private readonly Action<ILogger, Exception?> _failedToReadContent;
        private readonly Action<ILogger, Exception?> _failedToMerge;
        private readonly Action<ILogger, Exception?> _failedToWriteOutput;
        private readonly Action<ILogger, Exception?> _unhandledException;
        private readonly Action<ILogger, string, Exception?> _validatingYamlPath;
        private readonly Action<ILogger, string, Exception?> _invalidYamlPath;
        private readonly Action<ILogger, string, Exception?> _injectingAtPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineJobLogMessageActions"/> class.
        /// </summary>
        public CommandLineJobLogMessageActions()
        {
            _startingHandleCommand = LoggerMessage.Define(
                LogLevel.Information,
                JobEventIdFactory.StartingHandleCommand(),
                "Starting execution of CLI job handler");

            _readingTemplateFile = LoggerMessage.Define(
                LogLevel.Information,
                JobEventIdFactory.ReadingTemplateFile(),
                "Reading template YAML file");

            _readingContentFile = LoggerMessage.Define(
                LogLevel.Information,
                JobEventIdFactory.ReadingContentFile(),
                "Reading content YAML file");

            _mergingYamlContent = LoggerMessage.Define(
                LogLevel.Information,
                JobEventIdFactory.MergingYamlContent(),
                "Merging YAML content");

            _writingOutputFile = LoggerMessage.Define(
                LogLevel.Information,
                JobEventIdFactory.WritingOutputFile(),
                "Writing output YAML file");

            _failedToReadTemplate = LoggerMessage.Define(
                LogLevel.Error,
                JobEventIdFactory.FailedToReadTemplate(),
                "Failed to read template file.");

            _failedToReadContent = LoggerMessage.Define(
                LogLevel.Error,
                JobEventIdFactory.FailedToReadContent(),
                "Failed to read content file.");

            _failedToMerge = LoggerMessage.Define(
                LogLevel.Error,
                JobEventIdFactory.FailedToMerge(),
                "Failed to merge YAML content.");

            _failedToWriteOutput = LoggerMessage.Define(
                LogLevel.Error,
                JobEventIdFactory.FailedToWriteOutput(),
                "Failed to write output file.");

            _unhandledException = LoggerMessage.Define(
                LogLevel.Error,
                JobEventIdFactory.UnhandledException(),
                "Unhandled Exception.");

            _validatingYamlPath = LoggerMessage.Define<string>(
                LogLevel.Information,
                JobEventIdFactory.ValidatingYamlPath(),
                "Validating YAML path '{YamlPath}' against content file");

            _invalidYamlPath = LoggerMessage.Define<string>(
                LogLevel.Error,
                JobEventIdFactory.InvalidYamlPath(),
                "YAML path '{YamlPath}' does not exist in the content file");

            _injectingAtPath = LoggerMessage.Define<string>(
                LogLevel.Information,
                JobEventIdFactory.InjectingAtPath(),
                "Injecting template content at YAML path '{YamlPath}'");
        }

        internal void StartingHandleCommand(ILogger<CommandLineJob> logger)
        {
            _startingHandleCommand(logger, null);
        }

        internal void ReadingTemplateFile(ILogger<CommandLineJob> logger)
        {
            _readingTemplateFile(logger, null);
        }

        internal void ReadingContentFile(ILogger<CommandLineJob> logger)
        {
            _readingContentFile(logger, null);
        }

        internal void MergingYamlContent(ILogger<CommandLineJob> logger)
        {
            _mergingYamlContent(logger, null);
        }

        internal void WritingOutputFile(ILogger<CommandLineJob> logger)
        {
            _writingOutputFile(logger, null);
        }

        internal void FailedToReadTemplate(ILogger<CommandLineJob> logger, Exception exception)
        {
            _failedToReadTemplate(logger, exception);
        }

        internal void FailedToReadContent(ILogger<CommandLineJob> logger, Exception exception)
        {
            _failedToReadContent(logger, exception);
        }

        internal void FailedToMerge(ILogger<CommandLineJob> logger, Exception exception)
        {
            _failedToMerge(logger, exception);
        }

        internal void FailedToWriteOutput(ILogger<CommandLineJob> logger, Exception exception)
        {
            _failedToWriteOutput(logger, exception);
        }

        internal void UnhandledException(ILogger<CommandLineJob> logger, Exception exception)
        {
            _unhandledException(logger, exception);
        }

        internal void ValidatingYamlPath(ILogger<CommandLineJob> logger, string yamlPath)
        {
            _validatingYamlPath(logger, yamlPath, null);
        }

        internal void InvalidYamlPath(ILogger<CommandLineJob> logger, string yamlPath)
        {
            _invalidYamlPath(logger, yamlPath, null);
        }

        internal void InjectingAtPath(ILogger<CommandLineJob> logger, string yamlPath)
        {
            _injectingAtPath(logger, yamlPath, null);
        }
    }
}
