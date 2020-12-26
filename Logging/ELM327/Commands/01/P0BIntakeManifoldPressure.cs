namespace Logging.ELM327.Commands
{
	/// <summary>
	/// The '0B' PID doesn't return in the response code on Hyundai i30 1.6 2012...
	/// </summary>
	public class P0BIntakeManifoldPressure : Command
	{

		public override double Value => A;
		public override double Min => 0;
		public override double Max => 255;

		public P0BIntakeManifoldPressure() : base("01", "0B", 1, UnitType.KiloPascal)
		{

		}

	}
}
