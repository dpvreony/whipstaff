// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Whipstaff.Runtime.Extensions;
using Whipstaff.Wpf.InteractionFlows.FileDialogInteraction;

namespace Whipstaff.Wpf.InteractionFlows.SaveFileDialogInteraction
{
    /// <summary>
    /// Handling logic for a Save File Dialog interaction request.
    /// </summary>
    public static class SaveFileDialogHandler
    {
        /// <summary>
        /// Helper for handling the Windows Save File Dialog.
        /// </summary>
        /// <param name="request">Request settings for the Save File Dialog.</param>
        /// <returns>Whether the Save File Dialog was confirmed or cancelled.</returns>
        public static Task<SaveFileDialogResult> OnSaveFileDialogAsync(SaveFileDialogRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var dialog = new Microsoft.Win32.SaveFileDialog();

            request.Title.ActIfNotNullOrWhiteSpace(x => dialog.Title = x);
            dialog.ApplyRequestModel(request);

            if (request.CreatePrompt.HasValue)
            {
                dialog.CreatePrompt = request.CreatePrompt.Value;
            }

            if (request.CreateTestFile.HasValue)
            {
                dialog.CreateTestFile = request.CreateTestFile.Value;
            }

            if (request.OverwritePrompt.HasValue)
            {
                dialog.OverwritePrompt = request.OverwritePrompt.Value;
            }

            var dialogResult = dialog.ShowDialog() ?? false;

            if (!dialogResult)
            {
                return Task.FromResult(SaveFileDialogResult.Cancelled());
            }

            var fileName = dialog.FileName;
            return Task.FromResult(SaveFileDialogResult.Proceed(fileName));
        }
    }
}
