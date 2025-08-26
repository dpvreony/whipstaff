// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using Windows.Win32.Foundation;

namespace Whipstaff.Windows.PInvoke
{
    /// <summary>
    /// Helper methods for PInvoke to User32.dll Native Methods.
    /// </summary>
    public static class User32Helpers
    {
        /// <summary>
        /// Flashes the specified window, it does not change the active state of the window.
        /// </summary>
        /// <param name="hWnd">Pointer to the native window handle.</param>
        /// <returns>boolean stating whether the native FlashWindowEx call succeeded.</returns>
#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
        public static bool FlashWindowEx(IntPtr hWnd)
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
        {
            var h = new global::Windows.Win32.Foundation.HWND(hWnd);

            global::Windows.Win32.UI.WindowsAndMessaging.FLASHWINFO fInfo = new()
            {
                hwnd = h,
                dwFlags = global::Windows.Win32.UI.WindowsAndMessaging.FLASHWINFO_FLAGS.FLASHW_ALL,
                uCount = int.MaxValue,
                dwTimeout = 0,
            };

            fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));

            return global::Windows.Win32.PInvoke.FlashWindowEx(in fInfo);
        }

        /// <summary>
        /// Sets the display affinity for a window.
        /// </summary>
        /// <param name="hWnd">Pointer to the native window handle.</param>
        /// <param name="dwAffinity">
        /// <para>Type: <b>DWORD</b> The display affinity setting that specifies where the content of the window can be displayed.</para>
        /// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/nf-winuser-setwindowdisplayaffinity#parameters">Read more on docs.microsoft.com</see>.</para>
        /// </param>
        /// <returns>
        /// <para>Type: <b>BOOL</b> If the function succeeds, it returns <b>TRUE</b>; otherwise, it returns <b>FALSE</b> when, for example, the function call is made on a non top-level window. To get extended error information, call <a href="https://docs.microsoft.com/windows/desktop/api/errhandlingapi/nf-errhandlingapi-getlasterror">GetLastError</a>.</para>
        /// </returns>
        public static bool SetWindowDisplayAffinity(
            IntPtr hWnd,
            global::Windows.Win32.UI.WindowsAndMessaging.WINDOW_DISPLAY_AFFINITY dwAffinity)
        {
            var h = new global::Windows.Win32.Foundation.HWND(hWnd);

            return global::Windows.Win32.PInvoke.SetWindowDisplayAffinity(
                h,
                dwAffinity);
        }
    }
}
