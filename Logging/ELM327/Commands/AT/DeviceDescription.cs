namespace Logging.ELM327.Commands
{
	public class DeviceDescription : Command
	{

		public DeviceDescription() : base("AT", "@1", 4)
		{

		}

	}
}
