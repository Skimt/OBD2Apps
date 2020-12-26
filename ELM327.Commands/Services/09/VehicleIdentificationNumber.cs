namespace ELM327.Commands
{
	/// <summary>
	/// Returns empty value on Hyundai i30 1.6 2012.
	/// </summary>
	internal class VehicleIdentificationNumber : Command
	{

		public VehicleIdentificationNumber() : base("09", "02", 17)
		{

		}

	}
}
