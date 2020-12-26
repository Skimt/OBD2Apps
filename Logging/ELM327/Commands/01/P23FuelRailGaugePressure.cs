namespace Logging.ELM327.Commands
{
	public class P23FuelRailGaugePressure : Command
	{

		public override double Value => 10 * ((256 * A) + B);
		public override double Min => 0;
		public override double Max => 655350;

		public P23FuelRailGaugePressure() : base("01", "23", 2, UnitType.KiloPascal)
		{

		}

	}
}
