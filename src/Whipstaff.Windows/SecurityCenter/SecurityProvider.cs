// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Whipstaff.Windows.SecurityCenter
{
    /// <summary>
    /// Enumeration representing the available security providers.
    /// </summary>
    [Flags]
    public enum SecurityProvider
    {
        /// <summary>
        /// None Selected.
        /// </summary>
        None = 0,

        /// <summary>
        /// Firewall.
        /// </summary>
        WscSecurityProviderFirewall = 1,

        /// <summary>
        /// Auto update Settings.
        /// </summary>
        WscSecurityProviderAutoupdateSettings = 2,

        /// <summary>
        /// Anti Virus.
        /// </summary>
        WscSecurityProviderAntivirus = 4,

        /// <summary>
        /// Anti Spyware.
        /// </summary>
        WscSecurityProviderAntispyware = 8,

        /// <summary>
        /// Internet Settings.
        /// </summary>
        WscSecurityProviderInternetSettings = 16,

        /// <summary>
        /// User Account Control.
        /// </summary>
        WscSecurityProviderUserAccountControl = 32,

        /// <summary>
        /// Service.
        /// </summary>
        WscSecurityProviderService = 64,

        /// <summary>
        /// Selects all the other flags.
        /// </summary>
        WscSecurityProviderAll = WscSecurityProviderFirewall | WscSecurityProviderAutoupdateSettings |
            WscSecurityProviderAntivirus | WscSecurityProviderAntispyware |
            WscSecurityProviderInternetSettings | WscSecurityProviderUserAccountControl |
            WscSecurityProviderService
    }
}
