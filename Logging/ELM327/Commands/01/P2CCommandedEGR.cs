namespace Logging.ELM327.Commands
{
	/// <summary>
	/// https://en.wikipedia.org/wiki/Exhaust_gas_recirculation
	/// Exhaust gas recirculation (EGR) is a NOx emissions reduction technique used in engines.
	/// </summary>
	public class P2CCommandedEGR : Command
	{

		public override double Value => A / 2.55;
		public override double Min => 0;
		public override double Max => 100;

		public P2CCommandedEGR() : base("01", "2C", 1, UnitType.Percent)
		{

		}

	}
}
