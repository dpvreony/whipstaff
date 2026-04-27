using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using Microsoft.Win32;
using Whipstaff.Runtime.Extensions;
using Whipstaff.Wpf.InteractionFlows.OpenFileDialogInteraction;

namespace Whipstaff.Wpf.InteractionFlows.FileDialogInteraction
{
    /// <summary>
    /// Extension methods for <see cref="FileDialog"/>.
    /// </summary>
    public static class FileDialogExtensions
    {
        /// <summary>
        /// Applies the state of the given <see cref="AbstractFileDialogRequest"/> to the given <see cref="FileDialog"/>.
        /// </summary>
        /// <param name="dialog">File dialog instance to modify.</param>
        /// <param name="request">Abstract request model with state to apply.</param>
        public static void ApplyRequestModel(this FileDialog dialog, AbstractFileDialogRequest request)
        {
            ArgumentNullException.ThrowIfNull(dialog);
            ArgumentNullException.ThrowIfNull(request);

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

            request.InitialDirectory.ActIfNotNullOrWhiteSpace(x => dialog.InitialDirectory = x);

            request.RootDirectory.ActIfNotNullOrWhiteSpace(x => dialog.RootDirectory = x);

            if (request.ShowHiddenItems.HasValue)
            {
                dialog.ShowHiddenItems = request.ShowHiddenItems.Value;
            }

            request.Tag.ActIfNotNullOrWhiteSpace(x => dialog.Tag = x);

            if (request.ValidateNames.HasValue)
            {
                dialog.ValidateNames = request.ValidateNames.Value;
            }
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
