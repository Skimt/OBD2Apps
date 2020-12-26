namespace Logging.ELM327.Commands
{
	public class P25OxygenSensor02 : Command
	{

		public override double Value => 2 / 65536 * ((256 * A) + B); // Only ratio. Voltage is not calculated from C and D, yet.
		public override double Min => 0;
		public override double Max => 2;

		public P25OxygenSensor02() : base("01", "25", 4, UnitType.Ratio)
		{

		}

	}
}
