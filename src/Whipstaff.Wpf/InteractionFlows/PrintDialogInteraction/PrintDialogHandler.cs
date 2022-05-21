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
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Task<PrintDialogResult> OnPrintDialog(PrintDialogRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            PrintDialog dialog = new ();

            var dialogResult = dialog.ShowDialog() ?? false;

            if (!dialogResult)
            {
                return Task.FromResult(PrintDialogResult.Cancelled());
            }

            return Task.FromResult(PrintDialogResult.Proceed());
        }
    }
}
