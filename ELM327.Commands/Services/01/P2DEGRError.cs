namespace ELM327.Commands
{
	internal class P2DEGRError : Command, IP2DEGRError
	{

		public P2DEGRError() : base("01", "2D", 1, UnitType.Percent)
		{

		}

		public override double Value => (A / 1.28) - 100;
		public override double Min => -100;
		public override double Max => 99.2;

	}
}
