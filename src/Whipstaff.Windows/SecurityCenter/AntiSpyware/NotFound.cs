namespace Dhgms.Whipstaff.Model.Excptn.Security.AntiSpyware
{
    public class AntiSpywareNotFoundException
		: System.Exception
	{
        public AntiSpywareNotFoundException()
			: base("No Anti-Spyware product was detected!")
		{
		}
	}
}
