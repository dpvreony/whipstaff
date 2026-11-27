// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;
using Windows.Win32.Foundation;

namespace Whipstaff.Windows.PInvoke
{
    /// <summary>
    /// Helper methods for checking the package identity of the current process.
    /// </summary>
    public static class PackageIdentityHelpers
    {
        /// <summary>
        /// Checks whether the current process has a package identity.
        /// </summary>
        /// <returns><c>true</c> if the process is packaged, otherwise <c>false</c>.</returns>
        /// <exception cref="Win32Exception">Thrown when the native call returns an unexpected error.</exception>
        public static bool IsPackagedProcess()
        {
            uint length = 0;
            var result = global::Windows.Win32.PInvoke.GetCurrentPackageFullName(ref length, default);

            return result switch
            {
                WIN32_ERROR.ERROR_INSUFFICIENT_BUFFER => true,
                WIN32_ERROR.APPMODEL_ERROR_NO_PACKAGE => false,
                _ => throw new Win32Exception((int)result)
            };
        }
    }
}
