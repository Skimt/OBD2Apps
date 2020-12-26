namespace ELM327.Commands
{
	internal class P33AbsBarometricPressure : Command, IP33AbsBarometricPressure
	{

		public P33AbsBarometricPressure() : base("01", "33", 1, UnitType.KiloPascal)
		{

		}

		public override double Value => A;
		public override double Min => 0;
		public override double Max => 255;

	}
}
