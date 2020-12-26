namespace ELM327.Commands
{
	internal class P04EngineCoolantTemp : Command, IP04EngineCoolantTemp
	{

		public P04EngineCoolantTemp() : base("01", "04", 1, UnitType.Celsius)
		{

		}

		public override double Value => A - 40;
		public override double Min => -40;
		public override double Max => 215;

	}
}
