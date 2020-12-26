namespace Logging.ELM327.Commands
{
	public class P05EngineLoad : Command
	{

		public override double Value => A / 2.55;
		public override double Min => 0;
		public override double Max => 100;

		public P05EngineLoad() : base("01", "05", 1, UnitType.Percent)
		{

		}

	}
}
