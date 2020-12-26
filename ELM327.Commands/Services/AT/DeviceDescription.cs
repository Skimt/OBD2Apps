namespace ELM327.Commands
{
	/// <summary>
	/// The command used to identify an OBD device. 
	/// </summary>
	internal class DeviceDescription : Command, IDeviceDescription
	{

		public DeviceDescription() : base("AT", "@1", 4)
		{

		}

	}
}
