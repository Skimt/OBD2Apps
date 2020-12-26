namespace Logging.ELM327.Commands
{
	public class P0FIntakeAirTemp : Command
	{

		public override double Value => A - 40;
		public override double Min => -40;
		public override double Max => 215;

		public P0FIntakeAirTemp() : base("01", "0F", 1, UnitType.Celsius)
		{

		}

	}
}
