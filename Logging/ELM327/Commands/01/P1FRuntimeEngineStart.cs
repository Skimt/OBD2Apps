namespace Logging.ELM327.Commands
{
	public class P1FRuntimeEngineStart : Command
	{

		public override double Value => (256 * A) + B;
		public override double Min => 0;
		public override double Max => 65535;

		public P1FRuntimeEngineStart() : base("01", "1F", 2, UnitType.Seconds)
		{

		}

	}
}
