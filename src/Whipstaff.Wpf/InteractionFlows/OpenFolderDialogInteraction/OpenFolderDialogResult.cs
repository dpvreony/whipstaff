// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Whipstaff.Wpf.InteractionFlows.OpenFolderDialogInteraction
{
    /// <summary>
    /// Represents the result of an Open Folder Dialog interaction.
    /// </summary>
    public sealed record OpenFolderDialogResult
    {
        private OpenFolderDialogResult(
            bool userProceeded,
            IReadOnlyCollection<string>? safeFolderNames)
        {
            UserProceeded = userProceeded;
            SafeFolderNames = safeFolderNames;
        }

        /// <summary>
        /// Gets a value indicating whether the user chose to proceed with the file open dialog.
        /// </summary>
        public bool UserProceeded { get; }

        /// <summary>
        /// Gets an array of folder names, if the user chose to proceed.
        /// </summary>
        public IReadOnlyCollection<string>? SafeFolderNames { get; }

        /// <summary>
        /// Creates an instance of <see cref="OpenFolderDialogResult"/> indicating the user chose to cancel opening a file.
        /// </summary>
        /// <returns>An instance of <see cref="OpenFolderDialogResult"/>.</returns>
        public static OpenFolderDialogResult Cancelled()
        {
            return new OpenFolderDialogResult(
                false,
                null);
        }

        /// <summary>
        /// Creates an instance of <see cref="OpenFolderDialogResult"/> indicating the user chose to proceed with opening a file.
        /// </summary>
        /// <param name="safeFolderNames">An array of folder names chosen by the user.</param>
        /// <returns>An instance of <see cref="OpenFolderDialogResult"/>.</returns>
        public static OpenFolderDialogResult Proceed(IReadOnlyCollection<string> safeFolderNames)
        {
            return new OpenFolderDialogResult(
                true,
                safeFolderNames);
        }
    }
}
