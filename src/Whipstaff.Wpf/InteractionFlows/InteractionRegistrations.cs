using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using ReactiveUI;
using Whipstaff.Wpf.InteractionFlows.OpenFileDialogInteraction;

namespace Whipstaff.Wpf.InteractionFlows
{
    /// <summary>
    /// Simple Registration Logic for re-usable interaction flows.
    /// </summary>
    public static class InteractionRegistrations
    {
        /// <summary>
        /// Registers the WPF File Open Dialog interaction flow.
        /// </summary>
        /// <param name="compositeDisposable">The composite disposable to use to track the lifetime of the registration.</param>
        /// <param name="interactions">WPF Interaction Helper.</param>
        public static void RegisterFileOpenDialog(CompositeDisposable compositeDisposable, Interactions interactions)
        {
            ArgumentNullException.ThrowIfNull(compositeDisposable);
            ArgumentNullException.ThrowIfNull(interactions);

            // ReSharper disable once ConvertClosureToMethodGroup
            _ = interactions.FileOpenDialog.RegisterHandlerToOutputFunc(request => OpenFileDialogHandler.OnOpenFileDialog(request))
                .DisposeWith(compositeDisposable);
        }

        /// <summary>
        /// Registers an interaction to trigger a func and map the result to the interaction output.
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="interaction"></param>
        /// <param name="outputFunc"></param>
        /// <returns></returns>
        public static IDisposable RegisterHandlerToOutputFunc<TInput, TOutput>(this Interaction<TInput, TOutput> interaction, Func<TInput, Task<TOutput>> outputFunc)
        {
            ArgumentNullException.ThrowIfNull(interaction);

            return interaction.RegisterHandler(async x =>
                {
                    var result = await outputFunc(x.Input).ConfigureAwait(false);
                    x.SetOutput(result);
                });
        }
    }
}
