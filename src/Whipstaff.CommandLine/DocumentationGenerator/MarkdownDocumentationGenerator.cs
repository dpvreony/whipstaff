// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.CommandLine;
using System.CommandLine.Help;
using System.IO;
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
            var helpBuilder = new HelpBuilder(LocalizationResources.Instance);

            var stringBuilder = new StringBuilder();

            _ = stringBuilder.AppendLine("```");

            var output = new StringWriter(stringBuilder);

            var helpContext = new HelpContext(helpBuilder, rootCommand, output);

            helpBuilder.Write(helpContext);

            _ = stringBuilder.AppendLine("```");

            _ = stringBuilder.AppendLine("## Arguments");
            _ = stringBuilder.AppendLine();
            _ = stringBuilder.AppendLine("| Argument | Description | Default |");
            _ = stringBuilder.AppendLine("| ------ | ----------- | -------- |");

            foreach (var rootCommandArgument in rootCommand.Arguments)
            {
                _ = stringBuilder.AppendLine($"| {rootCommandArgument.Name} | {rootCommandArgument.Description} | {rootCommandArgument.GetDefaultValue()} |");
            }

            _ = stringBuilder.AppendLine($"# {rootCommand.Name}");
            _ = stringBuilder.AppendLine();
            _ = stringBuilder.AppendLine("## Options");
            _ = stringBuilder.AppendLine();
            _ = stringBuilder.AppendLine("| Option | Description | Required |");
            _ = stringBuilder.AppendLine("| ------ | ----------- | -------- |");

            foreach (var option in rootCommand.Options)
            {
                _ = stringBuilder.AppendLine($"| {option.Name} | {option.Description} | {option.IsRequired} |");
            }

            return stringBuilder.ToString();
        }
    }
}
