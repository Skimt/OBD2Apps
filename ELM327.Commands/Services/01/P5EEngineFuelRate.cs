namespace ELM327.Commands
{
	internal class P5EEngineFuelRate : Command, IP5EEngineFuelRate
	{

		public P5EEngineFuelRate() : base("01", "5E", 2, UnitType.LiterHour)
		{

		}

		public override double Value => ((256 * A) + B) / 20;
		public override double Min => 0;
		public override double Max => 3212.75;

	}
}
