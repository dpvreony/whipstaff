// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Help;
using System.IO;
using System.Linq;
using System.Text;

namespace Whipstaff.CommandLine.DocumentationGenerator
{
    /// <summary>
    /// Markdown documentation generator for Command line root commands.
    /// </summary>
    public static class MarkdownDocumentationGenerator
    {
        /// <summary>
        /// Generates documentation for a root command.
        /// </summary>
        /// <param name="rootCommand">The root command to process.</param>
        /// <returns>Markdown documentation.</returns>
        public static string GenerateDocumentation(RootCommand rootCommand)
        {
            var stringBuilder = new StringBuilder();

            _ = stringBuilder.Append("# ").AppendLine(rootCommand.Name);

            if (!string.IsNullOrWhiteSpace(rootCommand.Description))
            {
                _ = stringBuilder.AppendLine(rootCommand.Description);
            }

            stringBuilder = AppendHelpDisplay(
                stringBuilder,
                rootCommand);

            stringBuilder = AppendArgumentsBlock(
                stringBuilder,
                rootCommand);

            stringBuilder = AppendOptionsBlock(
                stringBuilder,
                rootCommand);

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Produces a help display for a root command as would be displayed by the CLI tool itself.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder"/> to populate.</param>
        /// <param name="rootCommand">The root command to build from.</param>
        /// <returns>Modified string builder.</returns>
        public static StringBuilder AppendHelpDisplay(StringBuilder stringBuilder, RootCommand rootCommand)
        {
            _ = stringBuilder.AppendLine("```");

            using (var textWriter = new StringWriter(stringBuilder))
            {
                var parseResult = rootCommand.Parse("-h");

                var invocationConfiguration = new InvocationConfiguration
                {
                    Output = textWriter
                };
                _ = parseResult.Invoke(invocationConfiguration);
            }

            _ = stringBuilder.AppendLine("```");

            return stringBuilder;
        }

        /// <summary>
        /// Produces a Markdown table of the arguments.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder"/> to populate.</param>
        /// <param name="rootCommand">The root command to build from.</param>
        /// <returns>Modified string builder.</returns>
        public static StringBuilder AppendArgumentsBlock(StringBuilder stringBuilder, RootCommand rootCommand)
        {
            var arguments = rootCommand.Arguments.Where(arg => !arg.Hidden)
                .ToList();

            return AppendArgumentsBlock(stringBuilder, arguments);
        }

        /// <summary>
        /// Produces a Markdown table of the arguments.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder"/> to populate.</param>
        /// <param name="arguments">List of arguments to build from.</param>
        /// <returns>Modified string builder.</returns>
        public static StringBuilder AppendArgumentsBlock(StringBuilder stringBuilder, IReadOnlyList<Argument> arguments)
        {
            if (arguments.Any())
            {
                _ = stringBuilder.AppendLine("## Arguments");
                _ = stringBuilder.AppendLine();
                _ = stringBuilder.AppendLine("| Argument | Description | Default |");
                _ = stringBuilder.AppendLine("| ------ | ----------- | -------- |");

                foreach (var rootCommandArgument in arguments)
                {
                    _ = stringBuilder.Append("| ")
                        .Append(rootCommandArgument.Name)
                        .Append(" | ")
                        .Append(rootCommandArgument.Description)
                        .Append(" | ");

                    if (rootCommandArgument.HasDefaultValue)
                    {
                        _ = stringBuilder.Append(rootCommandArgument.GetDefaultValue());
                    }

                    _ = stringBuilder.AppendLine(" |");
                }
            }

            return stringBuilder;
        }

        /// <summary>
        /// Produces a Markdown table of the options.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder"/> to populate.</param>
        /// <param name="rootCommand">The root command to build from.</param>
        /// <returns>Modified string builder.</returns>
        public static StringBuilder AppendOptionsBlock(StringBuilder stringBuilder, Command rootCommand)
        {
            var options = rootCommand.Options.Where(option => !option.Hidden)
                .ToList();

            return AppendOptionsBlock(stringBuilder, options);
        }

        /// <summary>
        /// Produces a Markdown table of the options.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder"/> to populate.</param>
        /// <param name="options">List of options to build from.</param>
        /// <returns>Modified string builder.</returns>
        public static StringBuilder AppendOptionsBlock(
            StringBuilder stringBuilder,
            IReadOnlyList<Option> options)
        {
            if (options.Any())
            {
                _ = stringBuilder.AppendLine();
                _ = stringBuilder.AppendLine("## Options");
                _ = stringBuilder.AppendLine();
                _ = stringBuilder.AppendLine("| Option | Description | Required |");
                _ = stringBuilder.AppendLine("| ------ | ----------- | -------- |");

                foreach (var option in options)
                {
                    _ = stringBuilder.Append("| ")
                        .Append(option.Name)
                        .Append(" | ")
                        .Append(option.Description)
                        .Append(" | ")
                        .Append(option.Required)
                        .AppendLine(" |");
                }
            }

            return stringBuilder;
        }
    }
}
