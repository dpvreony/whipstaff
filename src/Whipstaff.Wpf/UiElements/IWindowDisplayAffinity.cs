// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveUI;
using Windows.Win32.UI.WindowsAndMessaging;

namespace Whipstaff.Wpf.UiElements
{
    /// <summary>
    /// Represents a view model that can set the display affinity for a window.
    /// </summary>
    public interface IWindowDisplayAffinity
    {
        /// <summary>
        /// Gets or sets the display affinity for the window.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The display affinity setting specifies where the content of the window can be displayed.
        /// </para>
        /// <para>
        /// See <see href="https://learn.microsoft.com/windows/win32/api/winuser/nf-winuser-setwindowdisplayaffinity#parameters">SetWindowDisplayAffinity Parameters</see> for more information.
        /// </para>
        /// </remarks>
        global::Windows.Win32.UI.WindowsAndMessaging.WINDOW_DISPLAY_AFFINITY WindowDisplayAffinity { get; set; }
    }
}
