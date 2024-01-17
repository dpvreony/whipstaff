// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Whipstaff.Wpf.InteractionFlows.PrintDialogInteraction
{
    /// <summary>
    /// Handling logic for a FileOpenDialog interaction request.
    /// </summary>
    public static class PrintDialogHandler
    {
        /// <summary>
        /// Helper for handling the Windows Print Dialog.
        /// </summary>
        /// <param name="request">Request settings for the Print Dialog.</param>
        /// <returns>Whether the Print Dialog was confirmed or cancelled.</returns>
        public static Task<PrintDialogResult> OnPrintDialogAsync(PrintDialogRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var dialog = GetPrintDialog(request);

            var dialogResult = dialog.ShowDialog() ?? false;

            return Task.FromResult(dialogResult ? PrintDialogResult.Proceed() : PrintDialogResult.Cancelled());
        }

        private static PrintDialog GetPrintDialog(PrintDialogRequest request)
        {
            PrintDialog dialog = new();
            if (request.CurrentPageEnabled.HasValue)
            {
                dialog.CurrentPageEnabled = request.CurrentPageEnabled.Value;
            }

            if (request.MaxPage.HasValue)
            {
                dialog.MaxPage = request.MaxPage.Value;
            }

            if (request.MinPage.HasValue)
            {
                dialog.MinPage = request.MinPage.Value;
            }

            if (request.PageRange.HasValue)
            {
                dialog.PageRange = request.PageRange.Value;
            }

            if (request.PageRangeSelection.HasValue)
            {
                dialog.PageRangeSelection = request.PageRangeSelection.Value;
            }

            if (request.PrintQueue != null)
            {
                dialog.PrintQueue = request.PrintQueue;
            }

            if (request.SelectedPagesEnabled.HasValue)
            {
                dialog.SelectedPagesEnabled = request.SelectedPagesEnabled.Value;
            }

            if (request.UserPageRangeEnabled.HasValue)
            {
                dialog.UserPageRangeEnabled = request.UserPageRangeEnabled.Value;
            }

            return dialog;
        }
    }
}
