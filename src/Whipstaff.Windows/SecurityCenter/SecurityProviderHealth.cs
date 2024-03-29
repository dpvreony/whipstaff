﻿// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Windows.SecurityCenter
{
    /// <summary>
    /// Enumeration of Security Provider Health States.
    /// </summary>
    public enum SecurityProviderHealth
    {
        /// <summary>
        /// Security Provider is in good health.
        /// </summary>
        WscSecurityProviderHealthGood,

        /// <summary>
        /// Security Provider is not being monitored.
        /// </summary>
        WscSecurityProviderHealthNotMonitored,

        /// <summary>
        /// Security Provider is reporting poor health.
        /// </summary>
        WscSecurityProviderHealthPoor,

        /// <summary>
        /// Security Provider is snoozed.
        /// </summary>
        WscSecurityProviderHealthSnooze
    }
}
