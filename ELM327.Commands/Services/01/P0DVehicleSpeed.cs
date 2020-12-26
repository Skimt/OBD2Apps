namespace ELM327.Commands
{
	internal class P0DVehicleSpeed : Command, IP0DVehicleSpeed
	{

		public P0DVehicleSpeed() : base("01", "0D", 1, UnitType.KilometerPerHour)
		{

		}

		public override double Value => A;
		public override double Min => 0;
		public override double Max => 255;

	}
}
