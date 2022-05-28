// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using ReactiveUI;
using Whipstaff.Runtime.Extensions;

namespace Whipstaff.Wpf.InteractionFlows.OpenFileDialogInteraction
{
    /// <summary>
    /// Handling logic for a FileOpenDialog interaction request.
    /// </summary>
    public static class OpenFileDialogHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Task<OpenFileDialogResult> OnOpenFileDialog(OpenFileDialogRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var dialog = new Microsoft.Win32.OpenFileDialog();

            request.Title.ActIfNotNullOrWhiteSpace(x => dialog.Title = x);

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
