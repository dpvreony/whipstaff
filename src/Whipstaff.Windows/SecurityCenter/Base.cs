using Whipstaff.Windows;
using Whipstaff.Windows.SecurityCenter;

namespace Dhgms.Whipstaff.Model.Helper.SecurityCenter
{
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;

    using Dhgms.Whipstaff.Core.Model;

    /// <summary>
    /// Base class for checking the status of products registered with
    /// Security Center
    /// </summary>
    public abstract class Base
    {
        private readonly SecurityProvider securityProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Base"/> class.
        /// </summary>
        /// <param name="securityProvider">
        /// The security provider we want information for.
        /// </param>
        protected Base(SecurityProvider securityProvider)
        {
            this.securityProvider = securityProvider;
        }

        /// <summary>
        /// Check to make sure the product status is ok
        /// </summary>
        public void CheckOk()
        {
            var health =SecurityProviderHelpers.GetHealthStatus(securityProvider);

            if (health != SecurityProviderHealth.WscSecurityProviderHealthGood)
            {
                this.OnBadHealthState((SecurityProviderHealth)health);
            }
        }

        protected abstract void OnBadHealthState(SecurityProviderHealth health);
    }
}
