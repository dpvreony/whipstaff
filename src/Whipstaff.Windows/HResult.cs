// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Windows
{
    /// <summary>
    /// HRESULT for WinApi calls.
    /// </summary>
#pragma warning disable CA1028 // Enum Storage should be Int32
    public enum HRESULT : long
#pragma warning restore CA1028 // Enum Storage should be Int32
    {
#pragma warning disable CA1707 // Identifiers should not contain underscores
        /// <summary>
        /// Windows API call succeeded.
        /// </summary>
        S_OK = 0x0000,
#pragma warning restore CA1707 // Identifiers should not contain underscores
    }
}
