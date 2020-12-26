namespace Logging.ELM327.Commands
{
	/// <summary>
	/// 5E returns NO DATA error in Hyundai i30 1.6 2012
	/// </summary>
	public class P5EEngineFuelRate : Command
	{

		public override double Value => ((256 * A) + B) / 20;
		public override double Min => 0;
		public override double Max => 3212.75;

		public P5EEngineFuelRate() : base("01", "5E", 2, UnitType.LiterHour)
		{

		}

	}
}
