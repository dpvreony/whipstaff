// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Statiq.Common;
using Statiq.Core;

namespace Whipstaff.Statiq.Mermaid
{
    /// <summary>
    /// Statiq pipeline for processing mermaid diagram files.
    /// </summary>
    public sealed class MermaidDiagramPipeline : Pipeline
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MermaidDiagramPipeline"/> class.
        /// </summary>
        public MermaidDiagramPipeline()
        {
            // we need to check for mermaidjs-cli
            // we need to look for *.mmd files
            var patterns = new[] { "./**/*.mmd" };
            var readFiles = new ReadFiles(patterns);
            InputModules = new ModuleList(readFiles);

            ProcessModules = new ModuleList(new MermaidDiagramModule(new System.IO.Abstractions.FileSystem()));
        }
    }
}
