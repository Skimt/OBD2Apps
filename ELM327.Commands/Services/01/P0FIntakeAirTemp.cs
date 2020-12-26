namespace ELM327.Commands
{
	internal class P0FIntakeAirTemp : Command, IP0FIntakeAirTemp
	{

		public P0FIntakeAirTemp() : base("01", "0F", 1, UnitType.Celsius)
		{

		}

		public override double Value => A - 40;
		public override double Min => -40;
		public override double Max => 215;

	}
}
