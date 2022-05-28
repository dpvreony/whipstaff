// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Whipstaff.Wpf.InteractionFlows.OpenFileDialogInteraction
{
    /// <summary>
    /// Represents the result of an Open File Dialog interaction.
    /// </summary>
    public sealed class OpenFileDialogResult
    {
        private OpenFileDialogResult(
            bool userProceeded,
            IReadOnlyCollection<string>? fileNames)
        {
            UserProceeded = userProceeded;
            FileNames = fileNames;
        }

        /// <summary>
        /// Gets a flag indicating whether the user chose to proceed with the file open dialog.
        /// </summary>
        public bool UserProceeded { get; }

        /// <summary>
        /// Gets an array of file names, if the user chose to proceed.
        /// </summary>
        public IReadOnlyCollection<string>? FileNames { get; }

        /// <summary>
        /// Creates an instance of <see cref="OpenFileDialogResult"/> indicating the user chose to cancel opening a file.
        /// </summary>
        /// <returns>An instance of <see cref="OpenFileDialogResult"/>.</returns>
        public static OpenFileDialogResult Cancelled()
        {
            return new OpenFileDialogResult(
                false,
                null);
        }

        /// <summary>
        /// Creates an instance of <see cref="OpenFileDialogResult"/> indicating the user chose to proceed with opening a file.
        /// </summary>
        /// <param name="fileNames">An array of file names chosen by the user.</param>
        /// <returns>An instance of <see cref="OpenFileDialogResult"/>.</returns>
        public static OpenFileDialogResult Proceed(IReadOnlyCollection<string> fileNames)
        {
            return new OpenFileDialogResult(
                true,
                fileNames);
        }
    }
}
