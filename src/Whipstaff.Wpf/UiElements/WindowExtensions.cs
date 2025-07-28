// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Linq;
using System.Windows;
using ReactiveMarbles.ObservableEvents;
using ReactiveUI;
using Splat;
using Whipstaff.Wpf.Extensions;
using Whipstaff.Wpf.Input;

namespace Whipstaff.Wpf.UiElements
{
    /// <summary>
    /// Extensions Methods for <see cref="Window"/>.
    /// </summary>
    public static class WindowExtensions
    {
        /// <summary>
        /// Sets up the Window Display Affinity for a Window that implements <see cref="IViewFor{TViewModel}"/> and <see cref="IWindowDisplayAffinity"/>.
        /// </summary>
        /// <typeparam name="TWindow">The type for the window.</typeparam>
        /// <typeparam name="TViewModel">The type for the view model.</typeparam>
        /// <param name="window">Parent window.</param>
        /// <returns><see cref="System.IDisposable" /> object used to unsubscribe from the observable sequence.</returns>
        public static IDisposable BindSetWindowDisplayAffinityInteraction<TWindow, TViewModel>(
            this TWindow window)
            where TWindow : Window, IViewFor<TViewModel>, IEnableLogger
            where TViewModel : class, IWindowDisplayAffinity
        {
            return window.WhenAny(
                vw => vw.ViewModel!.WindowDisplayAffinity,
                change => change)
                .Select(change => change.Value)
                .Subscribe(affinity =>
                {
                    var affinityResult = window.SetWindowDisplayAffinity(affinity);
                    if (!affinityResult)
                    {
                        window.Log()
                            .Error($"Failed to set the display affinity for the window. Ensure that the window is a top-level window and that the display affinity is valid: {affinity}.");
                    }
                });
        }

        /// <summary>
        /// Hooks up the Keyboard Copy Command for use with a Window. Handy for error dialogs.
        /// </summary>
        /// <param name="window">Parent window.</param>
        /// <param name="getTextFunc">Function to call to get the text to put on the clipboard.</param>
        /// <returns><see cref="System.IDisposable" /> object used to unsubscribe from the observable sequence.</returns>
        public static IDisposable SubscribeKeyboardCopyAsText(this Window window, Func<string> getTextFunc)
        {
            return window.Events().KeyDown.Subscribe(args => args.OnCopyToClipboardAsText(getTextFunc));
        }

        /// <summary>
        /// Hooks up the Keyboard Copy Command for use with a Window. Handy for error dialogs.
        /// </summary>
        /// <param name="window">Parent window.</param>
        /// <param name="getTextFunc">Function to call to get the text to put on the clipboard.</param>
        /// <returns><see cref="System.IDisposable" /> object used to unsubscribe from the observable sequence.</returns>
        public static IDisposable SubscribeKeyboardCopyAsFormattedText(this Window window, Func<(string Text, TextDataFormat TextDataFormat)> getTextFunc)
        {
            return window.Events().KeyDown.Subscribe(args => args.OnCopyToClipboardAsText(getTextFunc));
        }
    }
}
