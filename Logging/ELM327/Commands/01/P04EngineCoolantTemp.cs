namespace Logging.ELM327.Commands
{
	/// <summary>
	/// Returns mostly -40 to -35 celsius. 
	/// </summary>
	public class P04EngineCoolantTemp : Command
	{

		public override double Value => A - 40;
		public override double Min => -40;
		public override double Max => 215;

		public P04EngineCoolantTemp() : base("01", "04", 1, UnitType.Celsius)
		{

		}

	}
}
