using System.Reactive.Linq;
using Dhgms.Whipstaff.Core.Model;
using Dhgms.Whipstaff.Model.Helper;

namespace Whipstaff.Windows.SecurityCenter
{
    /// <summary>
    /// Helpers for the Security Providers
    /// </summary>
    public static class SecurityProviderHelpers
    {
        /// <summary>
        /// Gets the health status of a health provider
        /// </summary>
        /// <param name="provider">The provider to query the health status of</param>
        /// <returns></returns>
        public static SecurityProviderHealth GetHealthStatus(int provider)
        {
            var resultCode = NativeMethods.WscGetSecurityProviderHealth(provider, out var health);
            if (resultCode != HRESULT.S_OK)
            {
                throw new InvalidOperationException($"Call to {nameof(NativeMethods.WscGetSecurityProviderHealth)} failed. Result code: {resultCode}");
            }

            return (SecurityProviderHealth)health;
        }

        /// <summary>
        /// Gets the health status of a health provider
        /// </summary>
        /// <param name="provider">The provider to query the health status of</param>
        /// <returns></returns>
        public static SecurityProviderHealth GetHealthStatus(SecurityProvider provider)
        {
            return GetHealthStatus((int)provider);
        }

        /// <summary>
        /// Gets an Observable sequence of Security Provider Health Statuses that repeats on a given time period.
        /// </summary>
        /// <param name="provider">The provider to query the health status of</param>
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
        /// <param name="provider">The provider to query the health status of</param>
        /// <param name="period">
        /// Period to produce subsequent values. If this value is equal to TimeSpan.Zero,
        ///  the timer will recur as fast as possible.
        /// </param>
        /// <returns>Observable sequence of Security Provider Health Statuses that repeats on a given time period.</returns>
        public static IObservable<SecurityProviderHealth> GetHealthStatusTimerObservable(int provider, TimeSpan period)
        {
            return Observable.Timer(DateTimeOffset.Now, period).Select(_ => GetHealthStatus(provider));
        }
    }
}
