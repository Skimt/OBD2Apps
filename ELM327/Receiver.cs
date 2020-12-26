using ELM327.Commands;
using System;
using System.IO.Ports;

namespace ELM327
{

	/// <summary>
	/// Contains the method used with <see cref="SerialPort.DataReceived"/>.
	/// </summary>
	internal class Receiver : ResponseHandler
	{

		private readonly ELM327Manager dataManager;
		private string dataStream;

		/// <summary>
		/// Initializes a new instance of the <see cref="Receiver"/> class. 
		/// </summary>
		internal Receiver(ELM327Manager dataManager)
		{
			this.dataManager = dataManager;
		}

		/// <summary>
		/// Returns true if the stream is finished. 
		/// </summary>
		private bool IsStreamFinished => dataStream.Contains(">") ? true : false;

		/// <summary>
		/// Triggers when the OBD responds to the request sent to it. 
		/// </summary>
		internal void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
		{

			dataManager.ResponseCount++;
			Sender port = sender as Sender;

			try
			{

				// Start stream. 
				dataStream += port.ReadExisting();

				if (!IsStreamFinished)
				{
					return;
				}

				// Handle response based on type. 
				switch (dataManager.RequestType)
				{
					case RequestType.OBD:
						SetSender(dataStream, port);
						break;
					case RequestType.DataStream:
						SaveToStorage(dataStream);
						break;
				}

			}
			catch (Exception ex)
			{

				dataManager.Log(ex.Message);

			}

			// Reset stream. 
			ResetDataStream();

		}

		/// <summary>
		/// Sets the Sender if the response contains the correct information.
		/// </summary>
		private void SetSender(string response, Sender sender)
		{

			if (!response.Contains("OBDII to RS232 Interpreter"))
			{
				return;
			}

			sender.Open();
			dataManager.Sender = sender;
			dataManager.Log(string.Format("Found the OBD device on the port '{0}'.", sender.PortName));
			
		}

		/// <summary>
		/// Saves stream to local storage. 
		/// </summary>
		private void SaveToStorage(string response)
		{

			// Do not proceed if SessionId is still 0. 
			if (dataManager.Storage == null)
			{
				return;
			}

			// Clean the response. 
			response = Clean(response);

			// Iterate through the response string.
			for (int i = 0; i < response.Length / 2; i++)
			{

				// Get the current command. 
				Command command = GetCalculatedCommand(i, response);

				// Add the command values to storage. 
				dataManager.Storage.Rows.Add(new object[]
				{
					DateTime.Now,
					command.PID,
					command.Value
					//dataManager.SessionId
				});

				// Shift the iteration forward to where the next PID should be located.
				i += command.BytesCount;

			}

		}

		/// <summary>
		/// Uses indexation to find the PID in the response string, then calculates and returns the command object of said PID. 
		/// </summary>
		private Command GetCalculatedCommand(int index, string response)
		{

			// Where the pid is supposed to be located in the given string. 
			int pidStartIndex = index * 2;

			string pid = response.Substring(pidStartIndex, 2);
			Command command = dataManager.GetCommand(pid);

			// All of the hexadecimals that should follow this PID in the given string.
			string hexValues = response.Substring(pidStartIndex + 2, 2 * command.BytesCount);

			// Wrong byte size. 
			if (command.BytesCount != hexValues.Length / 2)
			{
				throw new Exception(string.Format("Expected {0} bytes for the PID '{1}', but found {2} bytes in the response.", command.BytesCount, pid, hexValues.Length / 2));
			}

			// Convert hexadecimals to bytes (the command will automatically calculate the value). 
			command.ConvertToBytes(hexValues);

			return command;

		}

		/// <summary>
		/// End of stream. Reset the data stream.
		/// </summary>
		private void ResetDataStream()
		{
			if (dataStream.Contains(">"))
			{
				dataStream = string.Empty;
			}
		}

	}
}
