namespace Logging.ELM327.Commands
{
	/// <summary>
	/// https://en.wikipedia.org/wiki/Exhaust_gas_recirculation
	/// Exhaust gas recirculation (EGR) is a NOx emissions reduction technique used in engines.
	/// </summary>
	public class P2DEGRError : Command
	{

		public override double Value => (A / 1.28) - 100;
		public override double Min => -100;
		public override double Max => 99.2;

		public P2DEGRError() : base("01", "2D", 1, UnitType.Percent)
		{

		}

	}
}
