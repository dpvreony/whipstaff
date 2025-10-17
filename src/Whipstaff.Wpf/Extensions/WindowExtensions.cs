// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Wpf.Extensions
{
    /// <summary>
    /// Extensions for working with WPF windows.
    /// </summary>
    public static class WindowExtensions
    {
        /// <summary>Flashes the specified window. It does not change the active state of the window.</summary>
        /// <param name="window">WPF window to flash.</param>
        /// <returns>
        ///     The return value specifies the window's state before the call to the FlashWindowEx function. If the window
        ///     caption was drawn as active before the call, the return value is nonzero. Otherwise, the return value is zero.
        /// </returns>
        public static bool FlashWindow(this System.Windows.Window window)
        {
            var windowInteropHelper = new System.Windows.Interop.WindowInteropHelper(window);
            return Whipstaff.Windows.PInvoke.User32Helpers.FlashWindowEx(windowInteropHelper.Handle);
        }

        /// <summary>Flashes the specified window. It does not change the active state of the window.</summary>
        /// <param name="window">WPF window to adjust the display affinity for.</param>
        /// <param name="dwAffinity">
        /// <para>Type: <b>DWORD</b> The display affinity setting that specifies where the content of the window can be displayed.</para>
        /// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/nf-winuser-setwindowdisplayaffinity#parameters">Read more on docs.microsoft.com</see>.</para>
        /// </param>
        /// <returns>
        /// <para>Type: <b>BOOL</b> If the function succeeds, it returns <b>TRUE</b>; otherwise, it returns <b>FALSE</b> when, for example, the function call is made on a non top-level window. To get extended error information, call <a href="https://docs.microsoft.com/windows/desktop/api/errhandlingapi/nf-errhandlingapi-getlasterror">GetLastError</a>.</para>
        /// </returns>
        public static bool SetWindowDisplayAffinity(
            this System.Windows.Window window,
            global::Windows.Win32.UI.WindowsAndMessaging.WINDOW_DISPLAY_AFFINITY dwAffinity)
        {
            var windowInteropHelper = new System.Windows.Interop.WindowInteropHelper(window);
            return Whipstaff.Windows.PInvoke.User32Helpers.SetWindowDisplayAffinity(
                windowInteropHelper.Handle,
                dwAffinity);
        }
    }
}
