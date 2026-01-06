// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.IO.Abstractions;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Whipstaff.Mermaid.HttpServer;
using Whipstaff.Playwright;
using Whipstaff.Runtime.Extensions;

namespace Whipstaff.Mermaid.Playwright
{
    /// <summary>
    /// Interface for the Playwright Mermaid Renderer Browser Instance.
    /// </summary>
    public interface IPlaywrightRendererBrowserInstance : IDisposable
    {
        /// <summary>
        /// Gets the SVG for the Mermaid Diagram from a File and writes to another file.
        /// </summary>
        /// <param name="sourceFile">File containing the diagram markdown to convert.</param>
        /// <param name="targetFile">Destination file to write the diagram content to.</param>
        /// <returns>SVG diagram.</returns>
        Task CreateDiagramAndWriteToFileAsync(
            IFileInfo sourceFile,
            IFileInfo targetFile);

        /// <summary>
        /// Gets the SVG for the Mermaid Diagram from a File.
        /// </summary>
        /// <param name="sourceFileInfo">File containing the diagram markdown to convert.</param>
        /// <returns>SVG diagram.</returns>
        Task<GetDiagramResponseModel?> GetDiagramAsync(IFileInfo sourceFileInfo);

        /// <summary>
        /// Gets the SVG for the Mermaid Diagram from a <see cref="TextReader"/>.
        /// </summary>
        /// <param name="textReader">File containing the diagram markdown to convert.</param>
        /// <returns>SVG diagram.</returns>
        Task<GetDiagramResponseModel?> GetDiagramAsync(TextReader textReader);

        /// <summary>
        /// Gets the diagram from the page using the provided markdown.
        /// </summary>
        /// <param name="markdown">Markdown to process.</param>
        /// <returns>Diagram model.</returns>
        Task<GetDiagramResponseModel?> GetDiagramAsync(string markdown);
    }
}
