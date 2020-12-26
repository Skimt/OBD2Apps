namespace Logging.ELM327.Commands
{
	public class P0DVehicleSpeed : Command
	{

		public override double Value => A;
		public override double Min => 0;
		public override double Max => 255;

		public P0DVehicleSpeed() : base("01", "0D", 1, UnitType.KilometerPerHour)
		{

		}

	}
}
