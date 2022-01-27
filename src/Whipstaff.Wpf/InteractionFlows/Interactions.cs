using ReactiveUI;
using Whipstaff.Wpf.InteractionFlows.OpenFileDialogInteraction;
using Whipstaff.Wpf.InteractionFlows.PrintDialogInteraction;
using Whipstaff.Wpf.InteractionFlows.SaveFileDialogInteraction;

namespace Whipstaff.Wpf
{
    /// <summary>
    /// Common ReactiveUI Interactions.
    /// </summary>
    public static class Interactions
    {
        public static Interaction<OpenFileDialogRequest, OpenFileDialogResult> FileOpenDialog => new ();

        public static Interaction<PrintDialogRequest, PrintDialogResult> PrintDialog => new ();

        public static Interaction<SaveFileDialogRequest, SaveFileDialogResult> FileSaveDialog => new ();
    }
}
