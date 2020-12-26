namespace ELM327.Commands
{
	internal class P1FRuntimeEngineStart : Command, IP1FRuntimeEngineStart
	{

		public P1FRuntimeEngineStart() : base("01", "1F", 2, UnitType.Seconds)
		{

		}

		public override double Value => (256 * A) + B;
		public override double Min => 0;
		public override double Max => 65535;

	}
}
