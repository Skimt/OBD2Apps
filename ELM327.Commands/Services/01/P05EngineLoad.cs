namespace ELM327.Commands
{
	internal class P05EngineLoad : Command, IP05EngineLoad
	{

		public P05EngineLoad() : base("01", "05", 1, UnitType.Percent)
		{

		}

		public override double Value => A / 2.55;
		public override double Min => 0;
		public override double Max => 100;

	}
}
