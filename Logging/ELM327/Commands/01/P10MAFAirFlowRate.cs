﻿namespace Logging.ELM327.Commands
{
	/// <summary>
	/// https://en.wikipedia.org/wiki/Mass_flow_sensor 
	/// A mass (air) flow sensor (MAF) is a sensor used to determine the mass flow rate of air entering a fuel-injected internal combustion engine.
	/// </summary>
	public class P10MAFAirFlowRate : Command
	{

		public override double Value => ((256 * A) + B) / 100;
		public override double Min => 0;
		public override double Max => 655.35;

		public P10MAFAirFlowRate() : base("01", "10", 2, UnitType.GramsSecond)
		{

		}

	}
}
