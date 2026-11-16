// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO.Abstractions;

namespace Whipstaff.YamlTemplating.DotNetTool.CommandLine
{
    /// <summary>
    /// Model that represents the command line arguments.
    /// </summary>
    /// <param name="TemplatePath">The path to the template file.</param>
    /// <param name="ContentPath">The path to the content file.</param>
    /// <param name="OutputPath">The path to the output file.</param>
    /// <param name="YamlPath">The path to the YAML file.</param>
    public sealed record CommandLineArgModel(IFileInfo TemplatePath, IFileInfo ContentPath, IFileInfo OutputPath, string? YamlPath);
}
