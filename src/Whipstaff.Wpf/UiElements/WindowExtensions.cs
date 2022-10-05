// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Windows;
using ReactiveMarbles.ObservableEvents;
using Whipstaff.Wpf.Input;

namespace Whipstaff.Wpf.UiElements
{
    /// <summary>
    /// Extensions Methods for <see cref="Window"/>.
    /// </summary>
    public static class WindowExtensions
    {
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
