// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DynamicData;
using Whipstaff.Runtime.Extensions;

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

            if (request.AddExtension.HasValue)
            {
                dialog.AddExtension = request.AddExtension.Value;
            }

            if (request.AddToRecent.HasValue)
            {
                dialog.AddToRecent = request.AddToRecent.Value;
            }

            if (request.CheckFileExists.HasValue)
            {
                dialog.CheckFileExists = request.CheckFileExists.Value;
            }

            if (request.CheckPathExists.HasValue)
            {
                dialog.CheckPathExists = request.CheckPathExists.Value;
            }

            if (request.ClientGuid.HasValue)
            {
                dialog.ClientGuid = request.ClientGuid.Value;
            }

            ActIfNotEmpty(request.CustomPlaces, x => dialog.CustomPlaces.AddRange(x));
            request.DefaultDirectory.ActIfNotNullOrWhiteSpace(x => dialog.DefaultDirectory = x);
            request.DefaultExt.ActIfNotNullOrWhiteSpace(x => dialog.DefaultExt = x);

            if (request.DereferenceLinks.HasValue)
            {
                dialog.DereferenceLinks = request.DereferenceLinks.Value;
            }

            request.FileName.ActIfNotNullOrWhiteSpace(x => dialog.FileName = x);
            ActIfNotEmpty(request.FileNames, x => dialog.FileNames.AddRange(x));
            request.Filter.ActIfNotNullOrWhiteSpace(x => dialog.Filter = x);

            if (request.FilterIndex.HasValue)
            {
                dialog.FilterIndex = request.FilterIndex.Value;
            }

            if (request.ForcePreviewPane.HasValue)
            {
                dialog.ForcePreviewPane = request.ForcePreviewPane.Value;
            }

            request.InitialDirectory.ActIfNotNullOrWhiteSpace(x => dialog.InitialDirectory = x);

            if (request.Multiselect.HasValue)
            {
                dialog.Multiselect = request.Multiselect.Value;
            }

            if (request.ReadOnlyChecked.HasValue)
            {
                dialog.ReadOnlyChecked = request.ReadOnlyChecked.Value;
            }

            request.RootDirectory.ActIfNotNullOrWhiteSpace(x => dialog.RootDirectory = x);

            if (request.ShowHiddenItems.HasValue)
            {
                dialog.ShowHiddenItems = request.ShowHiddenItems.Value;
            }

            if (request.ShowReadOnly.HasValue)
            {
                dialog.ShowReadOnly = request.ShowReadOnly.Value;
            }

            request.Tag.ActIfNotNullOrWhiteSpace(x => dialog.Tag = x);

            if (request.ValidateNames.HasValue)
            {
                dialog.ValidateNames = request.ValidateNames.Value;
            }

            var dialogResult = dialog.ShowDialog() ?? false;

            if (!dialogResult)
            {
                return Task.FromResult(OpenFileDialogResult.Cancelled());
            }

            var fileNames = dialog.FileNames;
            return Task.FromResult(OpenFileDialogResult.Proceed(fileNames));
        }

        private static void ActIfNotEmpty<T>(IList<T>? requestFileNames, Action<IList<T>> action)
        {
            if (requestFileNames is not null && requestFileNames.Count > 0)
            {
                action(requestFileNames);
            }
        }
    }
}
