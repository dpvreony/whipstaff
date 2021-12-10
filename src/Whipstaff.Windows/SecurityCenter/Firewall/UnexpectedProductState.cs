namespace Dhgms.Whipstaff.Model.Excptn.Security.Firewall
{
    using Dhgms.Whipstaff.Core.Model;

    public class UnexpectedProductState
		: System.Exception
	{
        public UnexpectedProductState(SecurityProviderHealth healthState)
            : base("Firewall in unexpected Product State: " + (int)healthState)
        {
        }
	}
}
