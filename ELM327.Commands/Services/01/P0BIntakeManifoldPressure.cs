namespace ELM327.Commands
{
	internal class P0BIntakeManifoldPressure : Command, IP0BIntakeManifoldPressure
	{

		public P0BIntakeManifoldPressure() : base("01", "0B", 1, UnitType.KiloPascal)
		{

		}

		public override double Value => A;
		public override double Min => 0;
		public override double Max => 255;

	}
}
