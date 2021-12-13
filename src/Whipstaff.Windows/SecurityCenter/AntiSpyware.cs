using Dhgms.Whipstaff.Model.Excptn.Security.AntiSpyware;
using Whipstaff.Windows.SecurityCenter.AntiSpyware;

namespace Dhgms.Whipstaff.Model.Helper.SecurityCenter
{
    using Dhgms.Whipstaff.Core.Model;

    /// <summary>
    /// Checks on the security center status for the anti spyware
    /// </summary>
    public class AntiSpyware : Base
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AntiSpyware"/> class.
        /// </summary>
        public AntiSpyware()
            : base(SecurityProvider.WscSecurityProviderAntispyware)
        {
        }

        protected override void OnBadHealthState(SecurityProviderHealth health)
        {
            throw new UnexpectedProductStateException(health);
        }
    }
}
