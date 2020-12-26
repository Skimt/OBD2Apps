using ELM327.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OBDSimulator
{
	internal class OBDSimulator
	{

		private readonly List<Command> assemblyCommands;
		private readonly Communicator communicator;
		private readonly Port port;

		internal OBDSimulator(Port port, List<Command> assemblyCommands)
		{

			this.assemblyCommands = assemblyCommands;
			communicator = new Communicator(this);
			Commands = new List<Command>();

			this.port = port;
			this.port.DataReceived += communicator.OnDataReceived;

		}

		public bool IsELM327 { get; set; }

		/// <summary>
		/// Contains commands that have been translated in <see cref="Communicator.OnDataReceived"/>. 
		/// </summary>
		public List<Command> Commands { get; private set; }

		/// <summary>
		/// Disposes of the port connected to this OBDSimulator allowing for new ports to be created in its place. 
		/// This method does not dispose of the OBDSimulator itself, letting the GC handle it. 
		/// Returns the name of the disposed port. 
		/// </summary>
		public string Dispose()
		{
			string name = port.PortName;
			port.Dispose();
			return name;
		}

		/// <summary>
		/// Opens the port if it is closed. 
		/// </summary>
		public void KeepAlive()
		{
			port.Open();
		}

		/// <summary>
		/// Fills the commands list. 
		/// </summary>
		internal void SetCommands(string[] pids, string commandType)
		{

			Commands.Clear();

			for (int i = 1; i < pids.Length; i++)
			{
				try
				{
					Commands.Add(GetCommand(commandType, pids[i]));
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}

		}

		/// <summary>
		/// Retrieves command from commands list based on PID. 
		/// </summary>
		private Command GetCommand(string commandType, string pid)
		{
			return assemblyCommands.FirstOrDefault(c => c.CommandType == commandType && c.PID == pid) ?? throw new Exception("Could not retrieve command of given commandtype and PID.");
		}

	}
}
