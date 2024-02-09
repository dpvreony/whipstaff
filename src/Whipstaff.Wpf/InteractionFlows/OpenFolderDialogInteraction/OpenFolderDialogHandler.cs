// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Whipstaff.Runtime.Extensions;

namespace Whipstaff.Wpf.InteractionFlows.OpenFolderDialogInteraction
{
    /// <summary>
    /// Handling logic for a FileOpenDialog interaction request.
    /// </summary>
    public static class OpenFolderDialogHandler
    {
        /// <summary>
        /// Helper for handling the Windows Open File Dialog.
        /// </summary>
        /// <param name="request">Request settings for the Open File Dialog.</param>
        /// <returns>Whether the Open File Dialog was confirmed or cancelled.</returns>
        public static Task<OpenFolderDialogResult> OnOpenFolderDialogAsync(OpenFolderDialogRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var dialog = new Microsoft.Win32.OpenFolderDialog();

            request.Title.ActIfNotNullOrWhiteSpace(x => dialog.Title = x);

            var dialogResult = dialog.ShowDialog() ?? false;

            if (!dialogResult)
            {
                return Task.FromResult(OpenFolderDialogResult.Cancelled());
            }

            var safeFolderNames = dialog.SafeFolderNames;
            return Task.FromResult(OpenFolderDialogResult.Proceed(safeFolderNames));
        }
    }
}
