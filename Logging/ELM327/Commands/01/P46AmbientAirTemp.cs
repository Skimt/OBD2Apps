namespace Logging.ELM327.Commands
{
	/// <summary>
	/// 46 returns NO DATA in Hyundai i30 1.6 2010
	/// </summary>
	public class P46AmbientAirTemp : Command
	{

		public override double Value => A - 40;
		public override double Min => -40;
		public override double Max => 215;

		public P46AmbientAirTemp() : base("01", "46", 1, UnitType.Celsius)
		{

		}

	}
}
