namespace Logging.ELM327.Commands
{
	/// <summary>
	/// Static value on Hyundai i30 1.6 2012 (Stuck at 12% throttle when car is running, and 80% when shutting off.)
	/// </summary>
	public class P11ThrottlePosition : Command
	{

		public override double Value => A / 2.55;
		public override double Min => 0;
		public override double Max => 100;

		public P11ThrottlePosition() : base("01", "11", 1, UnitType.Percent)
		{

		}

	}
}
