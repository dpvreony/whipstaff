namespace Dhgms.Whipstaff.Model.Helper.SecurityCenter
{
    using Dhgms.Whipstaff.Core.Model;
    using Dhgms.Whipstaff.Model.Excptn.Security.Firewall;

    /// <summary>
    /// Checks on the security center status for the firewall
    /// </summary>
    public class Firewall : Base
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Firewall"/> class.
        /// </summary>
        public Firewall()
            : base(SecurityProvider.WscSecurityProviderFirewall)
        {
        }

        protected override void OnBadHealthState(SecurityProviderHealth health)
        {
            throw new UnexpectedProductState(health);
        }
    }
}
