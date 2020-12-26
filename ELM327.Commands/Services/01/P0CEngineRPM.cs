namespace ELM327.Commands
{
	internal class P0CEngineRPM : Command, IP0CEngineRPM
	{

		public P0CEngineRPM() : base ("01", "0C", 2, UnitType.RevolutionsPerMinute)
		{

		}

		public override double Value => ((256 * A) + B) / 4;
		public override double Min => 0;
		public override double Max => 16383.75;

	}
}
