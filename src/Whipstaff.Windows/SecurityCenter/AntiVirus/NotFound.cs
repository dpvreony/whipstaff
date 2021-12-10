namespace Dhgms.Whipstaff.Model.Excptn.Security.AntiVirus
{
    public class AntiVirusNotFoundException
		: System.Exception
	{
        public AntiVirusNotFoundException()
			: base("No Anti-Virus product was detected!")
		{
		}
	}
}
