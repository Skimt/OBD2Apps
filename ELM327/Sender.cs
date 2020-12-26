using ELM327.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;

namespace ELM327
{
	/// <summary>
	/// Is an extention of the <see cref="SerialPort"/> class, which represents a serial port resource. 
	/// To browser the .NET Framework source code for this type, see the Reference Source. 
	/// </summary>
	[DesignerCategory("Code")]
	internal partial class Sender : SerialPort
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="Sender"/> class using the specified port name. 
		/// An extention of the <see cref="SerialPort"/> class. 
		/// </summary>
		internal Sender(string portName) : base(portName) 
		{
			BaudRate = 38400; // Live: 38400, Sleep: 9600
			Parity = Parity.None;
			DataBits = 8;
			StopBits = StopBits.One;
			Handshake = Handshake.None;
			ReadTimeout = 5000;
			WriteTimeout = 5000;
		}

		/// <summary>
		/// Sends a single request through the serial port using the requested command.
		/// </summary>
		internal void Request(ICommand command)
		{
			WriteLine(command.CommandType + " " + command.PID);
		}

		/// <summary>
		/// Sends multiple requests through the serial port using the given list of commands.
		/// </summary>
		internal void Request(List<ICommand> commands, int maxCommands = 6)
		{

			if (commands.Count > maxCommands)
			{
				throw new Exception(string.Format("Too many commands. Can only send a maximum of {0} commands per request.", maxCommands));
			}

			string initialCommandType = commands.FirstOrDefault().CommandType;
			string commandString = initialCommandType + " ";
			int count = 0;

			foreach (ICommand command in commands)
			{

				if (command.CommandType != initialCommandType)
				{
					throw new Exception("Different command types found in assorted commands.");
				}

				commandString += command.PID;

				if (commands.Count != count)
				{
					commandString += " ";
				}

				count++;

			}

			WriteLine(commandString);

		}

		/// <summary>
		/// Opens the port if closed, and writes the given text. Also adds '\r' at the end of every command. 
		/// </summary>
		internal new void WriteLine(string text)
		{

			Open();
			base.WriteLine(text);

		}

		/// <summary>
		/// Opens the port if closed, and returns ReadExisting(). 
		/// </summary>
		internal new string ReadExisting()
		{

			Open();
			return base.ReadExisting();

		}

		/// <summary>
		/// Opens a new serial port connection if closed. 
		/// </summary>
		internal new void Open()
		{
			if (!IsOpen)
				base.Open();
		}

		/// <summary>
		/// List of available communication ports. 
		/// </summary>
		internal static List<Sender> GetPorts()
		{
			return GetPortNames().Select(portName => new Sender(portName)).ToList();
		}

	}
}
