using Dhgms.Whipstaff.Core.Model;

namespace Whipstaff.Windows.SecurityCenter.AntiSpyware
{
#pragma warning disable CA1032 // Implement standard exception constructors
    public class UnexpectedProductStateException
#pragma warning restore CA1032 // Implement standard exception constructors
        : System.Exception
    {
        public UnexpectedProductStateException(SecurityProviderHealth healthState)
            : base("Anti-Spyware in unexpected Product State: " + (int)healthState)
        {
        }
    }
}
