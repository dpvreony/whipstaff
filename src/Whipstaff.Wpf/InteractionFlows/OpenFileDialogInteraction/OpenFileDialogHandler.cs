﻿using System;
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
        public static OpenFileDialogResult OnOpenFileDialog(OpenFileDialogRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var dialog = new Microsoft.Win32.OpenFileDialog();

            request.Title.ActIfNotNullOrWhiteSpace(x => dialog.Title = x);

            var dialogResult = dialog.ShowDialog() ?? false;

            if (!dialogResult)
            {
                return OpenFileDialogResult.Cancelled();
            }

            var fileNames = dialog.FileNames;
            return OpenFileDialogResult.Proceed(fileNames);
        }
    }
}
