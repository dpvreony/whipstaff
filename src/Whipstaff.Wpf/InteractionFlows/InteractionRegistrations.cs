// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Threading.Tasks;
using ReactiveUI;
using Whipstaff.Wpf.InteractionFlows.OpenFileDialogInteraction;
using Whipstaff.Wpf.InteractionFlows.OpenFolderDialogInteraction;

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
            _ = interactions.FileOpenDialog.RegisterHandlerToOutputFunc(request => OpenFileDialogHandler.OnOpenFileDialogAsync(request))
                .DisposeWith(compositeDisposable);
        }

        /// <summary>
        /// Registers the WPF Folder Open Dialog interaction flow.
        /// </summary>
        /// <param name="compositeDisposable">The composite disposable to use to track the lifetime of the registration.</param>
        /// <param name="interactions">WPF Interaction Helper.</param>
        public static void RegisterFolderOpenDialog(CompositeDisposable compositeDisposable, Interactions interactions)
        {
            ArgumentNullException.ThrowIfNull(compositeDisposable);
            ArgumentNullException.ThrowIfNull(interactions);

            // ReSharper disable once ConvertClosureToMethodGroup
            _ = interactions.FolderOpenDialog.RegisterHandlerToOutputFunc(request => OpenFolderDialogHandler.OnOpenFolderDialogAsync(request))
                .DisposeWith(compositeDisposable);
        }

        /// <summary>
        /// Registers an interaction to trigger a func and map the result to the interaction output.
        /// </summary>
        /// <typeparam name="TInput">The type for the input passed into interaction.</typeparam>
        /// <typeparam name="TOutput">The output returned from the interaction.</typeparam>
        /// <param name="interaction">The interaction that takes an input and passes an output.</param>
        /// <param name="outputFunc">Function to apply to the output from the interaction.</param>
        /// <returns>Disposable subscription for the handler.</returns>
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
