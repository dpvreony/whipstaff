// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;
using PInvoke;

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
            global::PInvoke.User32.FLASHWINFO fInfo = new()
            {
                hwnd = hWnd,
                dwFlags = User32.FlashWindowFlags.FLASHW_ALL,
                uCount = int.MaxValue,
                dwTimeout = 0,
            };

            fInfo.cbSize = Convert.ToInt32(Marshal.SizeOf(fInfo));

            return global::PInvoke.User32.FlashWindowEx(ref fInfo);
        }
    }
}
