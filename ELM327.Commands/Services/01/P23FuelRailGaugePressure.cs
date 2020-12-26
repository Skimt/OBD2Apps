namespace ELM327.Commands
{
	internal class P23FuelRailGaugePressure : Command, IP23FuelRailGaugePressure
	{

		public P23FuelRailGaugePressure() : base("01", "23", 2, UnitType.KiloPascal)
		{

		}

		public override double Value => 10 * ((256 * A) + B);
		public override double Min => 0;
		public override double Max => 655350;

	}
}
