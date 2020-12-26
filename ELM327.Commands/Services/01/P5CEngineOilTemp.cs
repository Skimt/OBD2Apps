namespace ELM327.Commands
{
	internal class P5CEngineOilTemp : Command, IP5CEngineOilTemp
	{

		public P5CEngineOilTemp() : base("01", "5C", 1, UnitType.Celsius)
		{

		}

		public override double Value => A - 40;
		public override double Min => -40;
		public override double Max => 215;

	}
}
