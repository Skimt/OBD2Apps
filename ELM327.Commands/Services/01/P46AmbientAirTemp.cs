namespace ELM327.Commands
{
	internal class P46AmbientAirTemp : Command, IP46AmbientAirTemp
	{

		public P46AmbientAirTemp() : base("01", "46", 1, UnitType.Celsius)
		{

		}

		public override double Value => A - 40;
		public override double Min => -40;
		public override double Max => 215;

	}
}
