using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using ReactiveUI;
using Whipstaff.Wpf.InteractionFlows.OpenFileDialogInteraction;
using Whipstaff.Wpf.InteractionFlows.PrintDialogInteraction;
using Whipstaff.Wpf.InteractionFlows.SaveFileDialogInteraction;

namespace Whipstaff.Wpf.InteractionFlows
{
    /// <summary>
    /// Common ReactiveUI Interactions for WPF.
    /// </summary>
    public sealed class Interactions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileOpenDialogHandler">The handling logic for a File Open Dialog Interaction request.</param>
        /// <param name="fileSaveDialogHandler">The handling logic for a File Save Dialog Interaction request.</param>
        /// <param name="printDialogHandler">The handling logic for a Print Dialog Interaction request.</param>
        /// <param name="compositeDisposable">The composite disposable handler to register handler disposal management on.</param>
        /// <param name="handlerScheduler">The scheduler to register the interactions on, useful for overriding in unit\integration test scenarios.</param>
        public Interactions(
            Func<OpenFileDialogRequest, Task<OpenFileDialogResult>>? fileOpenDialogHandler,
            Func<SaveFileDialogRequest, Task<SaveFileDialogResult>>? fileSaveDialogHandler,
            Func<PrintDialogRequest, Task<PrintDialogResult>>? printDialogHandler,
            CompositeDisposable compositeDisposable,
            IScheduler? handlerScheduler = null)
        {
            FileOpenDialog = new Interaction<OpenFileDialogRequest, OpenFileDialogResult>(handlerScheduler);
            FileSaveDialog = new Interaction<SaveFileDialogRequest, SaveFileDialogResult>(handlerScheduler);
            PrintDialog = new Interaction<PrintDialogRequest, PrintDialogResult>(handlerScheduler);

            if (fileOpenDialogHandler != null)
            {
                _ = FileOpenDialog.RegisterHandlerToOutputFunc(fileOpenDialogHandler)
                    .DisposeWith(compositeDisposable);
            }

            if (fileSaveDialogHandler != null)
            {
                _ = FileSaveDialog.RegisterHandlerToOutputFunc(fileSaveDialogHandler)
                    .DisposeWith(compositeDisposable);
            }

            if (printDialogHandler != null)
            {
                _ = PrintDialog.RegisterHandlerToOutputFunc(printDialogHandler)
                    .DisposeWith(compositeDisposable);
            }
        }

        /// <summary>
        /// Sets up the WPF interaction flows with default windows operating system dialogs.
        /// </summary>
        /// <param name="compositeDisposable">The composit disposable handler to register handler disposal management on.</param>
        /// <param name="handlerScheduler">The scheduler to register the interactions on, useful for overriding in unit\integration test scenarios.</param>
        /// <returns></returns>
        public static Interactions CreateWithDefaults(
            CompositeDisposable compositeDisposable,
            IScheduler? handlerScheduler = null)
        {
            return new Interactions(
                OpenFileDialogHandler.OnOpenFileDialog,
                SaveFileDialogHandler.OnSaveFileDialog,
                PrintDialogHandler.OnPrintDialog,
                compositeDisposable,
                handlerScheduler);
        }

        /// <summary>
        /// Gets the interaction handler for a WPF File Open Dialog.
        /// </summary>
        public Interaction<OpenFileDialogRequest, OpenFileDialogResult> FileOpenDialog { get; }

        /// <summary>
        /// Gets the interaction handler for a WPF File Save Dialog.
        /// </summary>
        public Interaction<SaveFileDialogRequest, SaveFileDialogResult> FileSaveDialog { get; }

        /// <summary>
        /// Gets the interaction handler for a WPF Print Dialog.
        /// </summary>
        public Interaction<PrintDialogRequest, PrintDialogResult> PrintDialog { get; }
    }
}
