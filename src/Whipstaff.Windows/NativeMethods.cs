// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Whipstaff.Windows
{
    internal static class NativeMethods
    {
        [DllImport("Wscapi.dll")]
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        public static extern HRESULT WscGetSecurityProviderHealth(int providers, out int healthState);
    }
}
