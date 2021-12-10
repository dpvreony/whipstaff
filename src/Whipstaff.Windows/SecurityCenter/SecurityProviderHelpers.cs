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
            if (NativeMethods.WscGetSecurityProviderHealth(provider, out var health) != HRESULT.S_OK)
            {
                //throw 
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
    }
}
