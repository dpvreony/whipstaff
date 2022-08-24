// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Input;

namespace Whipstaff.Wpf.Input
{
    /// <summary>
    /// Extension methods for <see cref="KeyEventArgs"/>.
    /// </summary>
    public static class KeyEventArgsExtensions
    {
        /// <summary>
        /// Checks to see if a control sequence has been triggered for copying to the clipboard.
        /// </summary>
        /// <param name="keyEventArgs">The key event triggering the check.</param>
        /// <returns>Whether the sequence matches a copy command.</returns>
        public static bool IsCopyCommand(this KeyEventArgs keyEventArgs)
        {
            ArgumentNullException.ThrowIfNull(keyEventArgs);

            return keyEventArgs.Key switch
            {
                Key.LeftCtrl or Key.RightCtrl => keyEventArgs.KeyboardDevice.IsKeyDown(Key.C) ||
                                                 keyEventArgs.KeyboardDevice.IsKeyDown(Key.Insert),
                Key.C or Key.Insert => keyEventArgs.KeyboardDevice.IsKeyDown(Key.LeftCtrl) ||
                                       keyEventArgs.KeyboardDevice.IsKeyDown(Key.RightCtrl),
                _ => keyEventArgs.SystemKey == Key.OemCopy
            };
        }

        /// <summary>
        /// Ability to handle the copy to clipboard command for text.
        /// </summary>
        /// <param name="keyEventArgs">The key event triggering the check.</param>
        /// <param name="getTextFunc">The function to fetch text.</param>
        /// <returns>Whether the event triggered.</returns>
        public static bool OnCopyToClipboardAsText(
            this KeyEventArgs keyEventArgs,
            Func<string> getTextFunc)
        {
            ArgumentNullException.ThrowIfNull(keyEventArgs);
            ArgumentNullException.ThrowIfNull(getTextFunc);

            if (!keyEventArgs.IsCopyCommand())
            {
                return false;
            }

            var text = getTextFunc();
            Clipboard.SetText(text);

            return true;
        }

        /// <summary>
        /// Ability to handle the copy to clipboard command for text.
        /// </summary>
        /// <param name="keyEventArgs">The key event triggering the check.</param>
        /// <param name="getTextAndDataFormatFunc">The function to fetch text and what format the text should be treated as.</param>
        /// <returns>Whether the event triggered.</returns>
        public static bool OnCopyToClipboardAsText(
            this KeyEventArgs keyEventArgs,
            Func<(string Text, TextDataFormat TextDataFormat)> getTextAndDataFormatFunc)
        {
            ArgumentNullException.ThrowIfNull(keyEventArgs);
            ArgumentNullException.ThrowIfNull(getTextAndDataFormatFunc);

            if (!keyEventArgs.IsCopyCommand())
            {
                return false;
            }

            var (text, textDataFormat) = getTextAndDataFormatFunc();
            Clipboard.SetText(text, textDataFormat);

            return true;
        }
    }
}
