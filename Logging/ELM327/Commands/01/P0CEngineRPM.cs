namespace Logging.ELM327.Commands
{
	public class P0CEngineRPM : Command
	{

		public override double Value => ((256 * A) + B) / 4;
		public override double Min => 0;
		public override double Max => 16383.75;

		public P0CEngineRPM() : base ("01", "0C", 2, UnitType.RevolutionsPerMinute)
		{

		}

	}
}
