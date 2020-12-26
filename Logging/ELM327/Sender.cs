using Logging.ELM327.Commands;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.ELM327
{
	/// <summary>
	/// Is an extention of the <see cref="SerialPort"/> class, which represents a serial port resource. 
	/// To browser the .NET Framework source code for this type, see the Reference Source. 
	/// </summary>
	public class Sender : SerialPort
	{

		private readonly Config Config;

		/// <summary>
		/// Initializes a new instance of the <see cref="Sender"/> class using the specified port name. 
		/// An extention of the <see cref="SerialPort"/> class. 
		/// </summary>
		public Sender(string portName) : base(portName) 
		{

			Config = new Config();

			BaudRate = Config.BaudRate;
			Parity = Config.Parity;
			DataBits = Config.DataBits;
			StopBits = Config.StopBits;
			Handshake = Config.Handshake;
			ReadTimeout = Config.ReadTimeout;
			WriteTimeout = Config.WriteTimeout;

		}

		/// <summary>
		/// Sends a single request through the serial port using the requested command.
		/// </summary>
		public void Request(Command command)
		{
			Write(command.CommandType + " " + command.PID);
		}

		/// <summary>
		/// Sends multiple requests through the serial port using the requested commands.
		/// </summary>
		public void Request(List<Command> commands, int maxCommands = 6)
		{

			if (commands.Count > maxCommands)
			{
				throw new Exception(string.Format("Too many commands. Can only send a maximum of {0} commands per request.", maxCommands));
			}

			string initialCommandType = commands.FirstOrDefault().CommandType;
			string commandString = initialCommandType + " ";
			int count = 0;

			foreach (Command command in commands)
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

			Write(commandString);

		}

		/// <summary>
		/// Opens the port if closed, and writes the given text. Also adds '\r' at the end of every command. 
		/// </summary>
		public new void Write(string text)
		{

			if (!IsOpen)
			{
				Open();
			}

			base.Write(text + "\r");

		}

		/// <summary>
		/// Opens the port if closed, and returns ReadExisting(). 
		/// </summary>
		public new string ReadExisting()
		{

			if (!IsOpen)
			{
				Open();
			}

			return base.ReadExisting();

		}

		/// <summary>
		/// List of available communication ports. 
		/// </summary>
		public static List<Sender> GetPorts()
		{
			return GetPortNames().Select(portName => new Sender(portName)).ToList();
		}

		[Obsolete]
		public new void WriteLine(string text) { }

		[Obsolete]
		public new void Read(byte[] buffer, int offset, int count) { }

		[Obsolete]
		public new void ReadLine() { }

		[Obsolete]
		public new void ReadChar() { }

		[Obsolete]
		public new void ReadByte() { }

		[Obsolete]
		public new void ReadTo(string value) { }

	}
}
