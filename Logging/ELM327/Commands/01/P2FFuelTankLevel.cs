namespace Logging.ELM327.Commands
{
	/// <summary>
	/// The '0B' PID doesn't return in the response code on Hyundai i30 1.6 2012...
	/// </summary>
	public class P2FFuelTankLevel : Command
	{

		public override double Value => A / 2.55;
		public override double Min => 0;
		public override double Max => 100;

		public P2FFuelTankLevel() : base("01", "2F", 1, UnitType.Percent)
		{

		}

	}
}
