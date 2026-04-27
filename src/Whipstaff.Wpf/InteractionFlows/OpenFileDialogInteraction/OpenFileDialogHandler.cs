// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DynamicData;
using Whipstaff.Runtime.Extensions;
using Whipstaff.Wpf.InteractionFlows.FileDialogInteraction;

namespace Whipstaff.Wpf.InteractionFlows.OpenFileDialogInteraction
{
    /// <summary>
    /// Handling logic for a FileOpenDialog interaction request.
    /// </summary>
    public static class OpenFileDialogHandler
    {
        /// <summary>
        /// Helper for handling the Windows Open File Dialog.
        /// </summary>
        /// <param name="request">Request settings for the Open File Dialog.</param>
        /// <returns>Whether the Open File Dialog was confirmed or cancelled.</returns>
        public static Task<OpenFileDialogResult> OnOpenFileDialogAsync(OpenFileDialogRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var dialog = new Microsoft.Win32.OpenFileDialog();

            request.Title.ActIfNotNullOrWhiteSpace(x => dialog.Title = x);
            dialog.ApplyRequestModel(request);

            if (request.ForcePreviewPane.HasValue)
            {
                dialog.ForcePreviewPane = request.ForcePreviewPane.Value;
            }

            if (request.Multiselect.HasValue)
            {
                dialog.Multiselect = request.Multiselect.Value;
            }

            if (request.ReadOnlyChecked.HasValue)
            {
                dialog.ReadOnlyChecked = request.ReadOnlyChecked.Value;
            }

            if (request.ShowReadOnly.HasValue)
            {
                dialog.ShowReadOnly = request.ShowReadOnly.Value;
            }

            var dialogResult = dialog.ShowDialog() ?? false;

            if (!dialogResult)
            {
                return Task.FromResult(OpenFileDialogResult.Cancelled());
            }

            var fileNames = dialog.FileNames;
            return Task.FromResult(OpenFileDialogResult.Proceed(fileNames));
        }
    }
}
