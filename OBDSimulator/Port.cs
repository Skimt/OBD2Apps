using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;

namespace OBDSimulator
{
	[DesignerCategory("Code")]
	internal partial class Port : SerialPort
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="Port"/> class using the specified port name. 
		/// An extention of the <see cref="SerialPort"/> class. 
		/// </summary>
		internal Port(string portName) : base(portName)
		{
			BaudRate = 38400;
			Parity = Parity.None;
			DataBits = 8;
			StopBits = StopBits.One;
			Handshake = Handshake.None;
			ReadTimeout = 5000;
			WriteTimeout = 5000;
		}

		/// <summary>
		/// Opens the port if closed, and writes the given text. Also adds '\r' at the end of every command. 
		/// </summary>
		internal new void WriteLine(string text)
		{

			try
			{
				Open();
				base.WriteLine(text);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

		}

		/// <summary>
		/// Opens the port if closed, and returns ReadLine(). 
		/// </summary>
		internal new string ReadLine()
		{

			string response = "";

			try
			{
				Open();
				response = base.ReadLine();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			return response;

		}

		/// <summary>
		/// Attempts to open a new serial port connection.
		/// </summary>
		internal new void Open()
		{
			if (!IsOpen)
			{
				try
				{
					base.Open();
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
			}
		}

		/// <summary>
		/// List of available communication ports. 
		/// </summary>
		internal static List<Port> GetPorts()
		{
			return GetPortNames().Select(portName => new Port(portName)).ToList();
		}

	}
}