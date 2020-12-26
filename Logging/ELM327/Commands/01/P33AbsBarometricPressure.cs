namespace Logging.ELM327.Commands
{
	public class P33AbsBarometricPressure : Command
	{

		public override double Value => A;
		public override double Min => 0;
		public override double Max => 255;

		public P33AbsBarometricPressure() : base("01", "33", 1, UnitType.KiloPascal)
		{

		}

	}
}
