namespace ELM327.Commands
{
	internal class P11ThrottlePosition : Command, IP11ThrottlePosition
	{

		public P11ThrottlePosition() : base("01", "11", 1, UnitType.Percent)
		{

		}

		public override double Value => A / 2.55;
		public override double Min => 0;
		public override double Max => 100;

	}
}
