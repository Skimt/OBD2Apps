namespace Logging.ELM327.Commands
{
	/// <summary>
	/// 5C returns NO DATA error in Hyundai i30 1.6 2012
	/// </summary>
	public class P5CEngineOilTemp : Command
	{

		public override double Value => A - 40;
		public override double Min => -40;
		public override double Max => 215;

		public P5CEngineOilTemp() : base("01", "5C", 1, UnitType.Celsius)
		{

		}

	}
}
