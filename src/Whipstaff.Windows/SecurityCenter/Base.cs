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
        [DllImport("Wscapi.dll")]
        static extern HRESULT WscGetSecurityProviderHealth(int providers, out int healthState);

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
            if (Environment.OSVersion.Version < new Version(6, 1))
            {
                // can't do these checks on pre vista machines
                return;
            }

            int health;
            if (WscGetSecurityProviderHealth((int)this.securityProvider, out health) != HRESULT.S_OK)
            {
                //throw 
            }

            if ((SecurityProviderHealth)health != SecurityProviderHealth.WscSecurityProviderHealthGood)
            {
                this.OnBadHealthState((SecurityProviderHealth)health);
            }
        }

        protected abstract void OnBadHealthState(SecurityProviderHealth health);
    }
}
