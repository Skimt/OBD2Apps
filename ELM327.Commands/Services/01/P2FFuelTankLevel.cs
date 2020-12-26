namespace ELM327.Commands
{
	internal class P2FFuelTankLevel : Command, IP2FFuelTankLevel
	{

		public P2FFuelTankLevel() : base("01", "2F", 1, UnitType.Percent)
		{

		}

		public override double Value => A / 2.55;
		public override double Min => 0;
		public override double Max => 100;

	}
}
