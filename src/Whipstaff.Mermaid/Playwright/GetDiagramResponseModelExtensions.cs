// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO.Abstractions;
using System.Threading.Tasks;

namespace Whipstaff.Mermaid.Playwright
{
    /// <summary>
    /// Extension methods for <see cref="GetDiagramResponseModel"/>.
    /// </summary>
    public static class GetDiagramResponseModelExtensions
    {
        /// <summary>
        /// Write the diagram to a file.
        /// </summary>
        /// <param name="diagramResponseModel">Diagram response model to save.</param>
        /// <param name="targetFile">Target file to write to.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task ToFileAsync(
            this GetDiagramResponseModel diagramResponseModel,
            IFileInfo targetFile)
        {
            ArgumentNullException.ThrowIfNull(diagramResponseModel);
            ArgumentNullException.ThrowIfNull(targetFile);
            if (targetFile.Exists)
            {
                throw new ArgumentException("Target file already exists", nameof(targetFile));
            }

            return diagramResponseModel.InternalToFileAsync(
                targetFile);
        }

        internal static async Task InternalToFileAsync(
            this GetDiagramResponseModel diagramResponseModel,
            IFileInfo targetFile)
        {
            using (var textWriter = targetFile.CreateText())
            {
                await textWriter.WriteAsync(diagramResponseModel.Svg)
                    .ConfigureAwait(false);
            }
        }
    }
}
