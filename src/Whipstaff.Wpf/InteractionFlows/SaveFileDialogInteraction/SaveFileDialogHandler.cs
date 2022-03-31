using System;
using System.Threading.Tasks;
using Whipstaff.Runtime.Extensions;

namespace Whipstaff.Wpf.InteractionFlows.SaveFileDialogInteraction
{
    /// <summary>
    /// Handling logic for a Save File Dialog interaction request.
    /// </summary>
    public static class SaveFileDialogHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Task<SaveFileDialogResult> OnSaveFileDialog(SaveFileDialogRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var dialog = new Microsoft.Win32.SaveFileDialog();

            request.Title.ActIfNotNullOrWhiteSpace(x => dialog.Title = x);

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
