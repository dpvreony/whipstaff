// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Linq;
using Windows.Win32.System.SecurityCenter;

namespace Whipstaff.Windows.SecurityCenter
{
    /// <summary>
    /// Helpers for the Security Providers.
    /// </summary>
    public static class SecurityProviderHelpers
    {
        /// <summary>
        /// Gets the health status of a health provider.
        /// </summary>
        /// <param name="provider">The provider to query the health status of.</param>
        /// <returns>Health Status.</returns>
        public static SecurityProviderHealth GetHealthStatus(uint provider)
        {
            var health = default(WSC_SECURITY_PROVIDER_HEALTH);
            var resultCode = global::Windows.Win32.PInvoke.WscGetSecurityProviderHealth(provider, ref health);
            if (!resultCode.Succeeded)
            {
                throw new InvalidOperationException($"Call to {nameof(global::Windows.Win32.PInvoke.WscGetSecurityProviderHealth)} failed. Result code: {resultCode}");
            }

            return (SecurityProviderHealth)health;
        }

        /// <summary>
        /// Gets the health status of a health provider.
        /// </summary>
        /// <param name="provider">The provider to query the health status of.</param>
        /// <returns>Health Status.</returns>
        public static SecurityProviderHealth GetHealthStatus(SecurityProvider provider)
        {
            return GetHealthStatus(provider);
        }

        /// <summary>
        /// Gets an Observable sequence of Security Provider Health Statuses that repeats on a given time period.
        /// </summary>
        /// <param name="provider">The provider to query the health status of.</param>
        /// <param name="period">
        /// Period to produce subsequent values. If this value is equal to TimeSpan.Zero,
        ///  the timer will recur as fast as possible.
        /// </param>
        /// <returns>Observable sequence of Security Provider Health Statuses that repeats on a given time period.</returns>
        public static IObservable<SecurityProviderHealth> GetHealthStatusTimerObservable(SecurityProvider provider, TimeSpan period)
        {
            return Observable.Timer(DateTimeOffset.Now, period).Select(_ => GetHealthStatus(provider));
        }

        /// <summary>
        /// Gets an Observable sequence of Security Provider Health Statuses that repeats on a given time period.
        /// </summary>
        /// <param name="provider">The provider to query the health status of.</param>
        /// <param name="period">
        /// Period to produce subsequent values. If this value is equal to TimeSpan.Zero,
        ///  the timer will recur as fast as possible.
        /// </param>
        /// <returns>Observable sequence of Security Provider Health Statuses that repeats on a given time period.</returns>
        public static IObservable<SecurityProviderHealth> GetHealthStatusTimerObservable(uint provider, TimeSpan period)
        {
            return Observable.Timer(DateTimeOffset.Now, period).Select(_ => GetHealthStatus(provider));
        }
    }
}
