using ELM327.Commands;
using System;
using System.Collections.Generic;
using System.Threading;

namespace OBDSimulator
{
	public class Program
	{

		private const int MILLISECONDS_TIMEOUT = 24;

		private static readonly List<OBDSimulator> simulators = new List<OBDSimulator>();
		private static List<Command> assemblyCommands;
		private static int tick = 0;

		public static void Main()
		{

			assemblyCommands = Command.GetCommandsInAssembly();

			while (true)
			{
				Update();
			}

		}

		public static void Update()
		{

			// Updates every ~5.0 seconds.
			if (tick % 200 == 0)
			{
				CreateSimulators();
			}

			foreach (OBDSimulator simulator in simulators)
			{
				simulator.KeepAlive();
			}

			tick++;

			Thread.Sleep(MILLISECONDS_TIMEOUT);

		}

		/// <summary>
		/// Fills the simulators list with potential simulators that contain at least one valid serial communication port. 
		/// <para>Beware, creating new simulators inside infinite loops can cause memory leaks.</para>
		/// </summary>
		private static void CreateSimulators()
		{

			DestroySimulators();

			foreach (Port serialPort in Port.GetPorts())
			{
				Console.WriteLine("Creating simulator of port " + serialPort.PortName.ToString() + ".");
				simulators.Add(new OBDSimulator(serialPort, assemblyCommands));
			}

		}

		/// <summary>
		/// Disposes of the serial port in each and every simulator and clears the simulators list, preparing it for a new set of simulators. 
		/// </summary>
		private static void DestroySimulators()
		{

			foreach (OBDSimulator obd in simulators)
			{
				string portName = obd.Dispose();
				Console.WriteLine("Destroying simulator of port " + portName + ".");
			}

			simulators.Clear();

		}

	}
}
