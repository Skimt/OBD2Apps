namespace ELM327.Commands
{
	internal class P2CCommandedEGR : Command, IP2CCommandedEGR
	{

		public P2CCommandedEGR() : base("01", "2C", 1, UnitType.Percent)
		{

		}

		public override double Value => A / 2.55;
		public override double Min => 0;
		public override double Max => 100;

	}
}
