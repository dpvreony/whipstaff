namespace Dhgms.Whipstaff.Model.Excptn.Security.Firewall
{
    public class FirewallNotFoundException
        : System.Exception
    {
        public FirewallNotFoundException()
            : base("No firewall product was detected!")
        {
        }
    }
}
