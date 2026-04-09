// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;
using Whipstaff.Core.Logging;
using Whipstaff.Core.Logging.MessageActionWrappers;

namespace Whipstaff.YamlTemplating.DotNetTool
{
    /// <summary>
    /// Log Message actions wrapper for <see cref="CommandLineJob" />.
    /// </summary>
    internal sealed class CommandLineJobLogMessageActionsWrapper : ILogMessageActionsWrapper<CommandLineJob>, IWrapLogMessageActionUnhandledException
    {
        private readonly CommandLineJobLogMessageActions _commandLineJobLogMessageActions;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineJobLogMessageActionsWrapper"/> class.
        /// </summary>
        /// <param name="commandLineJobLogMessageActions">Log Message actions for <see cref="CommandLineJob" />.</param>
        /// <param name="logger">Logging framework instance.</param>
        public CommandLineJobLogMessageActionsWrapper(
            CommandLineJobLogMessageActions commandLineJobLogMessageActions,
#pragma warning disable S6672
            ILogger<CommandLineJob> logger)
#pragma warning restore S6672
        {
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(commandLineJobLogMessageActions);

            Logger = logger;
            _commandLineJobLogMessageActions = commandLineJobLogMessageActions;
        }

        /// <inheritdoc/>
        public ILogger<CommandLineJob> Logger
        {
            get;
        }

        /// <summary>
        /// Log message action for when the command line job is starting.
        /// </summary>
        public void StartingHandleCommand()
        {
            _commandLineJobLogMessageActions.StartingHandleCommand(Logger);
        }

        /// <summary>
        /// Log message action for when reading the template file.
        /// </summary>
        public void ReadingTemplateFile()
        {
            _commandLineJobLogMessageActions.ReadingTemplateFile(Logger);
        }

        /// <summary>
        /// Log message action for when reading the content file.
        /// </summary>
        public void ReadingContentFile()
        {
            _commandLineJobLogMessageActions.ReadingContentFile(Logger);
        }

        /// <summary>
        /// Log message action for when merging YAML content.
        /// </summary>
        public void MergingYamlContent()
        {
            _commandLineJobLogMessageActions.MergingYamlContent(Logger);
        }

        /// <summary>
        /// Log message action for when writing the output file.
        /// </summary>
        public void WritingOutputFile()
        {
            _commandLineJobLogMessageActions.WritingOutputFile(Logger);
        }

        /// <summary>
        /// Log message action for when reading the template file fails.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        public void FailedToReadTemplate(Exception exception)
        {
            _commandLineJobLogMessageActions.FailedToReadTemplate(Logger, exception);
        }

        /// <summary>
        /// Log message action for when reading the content file fails.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        public void FailedToReadContent(Exception exception)
        {
            _commandLineJobLogMessageActions.FailedToReadContent(Logger, exception);
        }

        /// <summary>
        /// Log message action for when merging YAML content fails.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        public void FailedToMerge(Exception exception)
        {
            _commandLineJobLogMessageActions.FailedToMerge(Logger, exception);
        }

        /// <summary>
        /// Log message action for when writing the output file fails.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        public void FailedToWriteOutput(Exception exception)
        {
            _commandLineJobLogMessageActions.FailedToWriteOutput(Logger, exception);
        }

        /// <inheritdoc />
        public void UnhandledException(Exception exception)
        {
            _commandLineJobLogMessageActions.UnhandledException(Logger, exception);
        }

        /// <summary>
        /// Log message action for when validating the YAML path argument.
        /// </summary>
        /// <param name="yamlPath">The dot-notation YAML path being validated.</param>
        public void ValidatingYamlPath(string yamlPath)
        {
            _commandLineJobLogMessageActions.ValidatingYamlPath(Logger, yamlPath);
        }

        /// <summary>
        /// Log message action for when the YAML path does not exist in the content file.
        /// </summary>
        /// <param name="yamlPath">The dot-notation YAML path that was not found.</param>
        public void InvalidYamlPath(string yamlPath)
        {
            _commandLineJobLogMessageActions.InvalidYamlPath(Logger, yamlPath);
        }

        /// <summary>
        /// Log message action for when injecting the template content at the specified YAML path.
        /// </summary>
        /// <param name="yamlPath">The dot-notation YAML path where content is being injected.</param>
        public void InjectingAtPath(string yamlPath)
        {
            _commandLineJobLogMessageActions.InjectingAtPath(Logger, yamlPath);
        }
    }
}
