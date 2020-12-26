using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.ELM327
{

	/// <summary>
	/// Contains the method used with <see cref="SerialPort.DataReceived"/>.
	/// </summary>
	public class Receiver
	{

		/// <summary>
		/// Contains storage...
		/// </summary>
		public readonly Storager Storager;

		private readonly DataManager dataManager;
		private readonly Interpreter interpreter;
		private string dataStream;

		/// <summary>
		/// Initializes a new instance of the <see cref="Receiver"/> class. 
		/// </summary>
		public Receiver(DataManager dataManager)
		{
			this.dataManager = dataManager;
			interpreter = new Interpreter();
			Storager = new Storager(dataManager);
		}

		/// <summary>
		/// Triggers when the OBD responds to the request sent to it. 
		/// </summary>
		public void DataManager_OnDataReceived(object sender, SerialDataReceivedEventArgs e)
		{

			Sender port = sender as Sender;

			try
			{

				// Start the data stream. 
				dataStream += port.ReadExisting();

				// Response is still streaming. 
				if (!dataStream.Contains(">"))
				{
					return;
				}

				if (dataManager.RequestType == RequestType.OBD)
				{

					// OBD found. 
					if (dataStream.Contains("OBDII to RS232 Interpreter"))
					{
						dataManager.Sender = port;
						dataManager.Log(string.Format("Found the OBD device on the port '{0}'.", port.PortName));
					}

				}
				else
				{

					// Interpret stream. 
					dataStream = interpreter.GetCleanResponse(dataStream); 

					// Save stream to storage. 
					Storager.Save(dataStream);

				}

			}
			catch (Exception ex)
			{

				dataManager.Log(ex.Message);

			}

			// Reset. 
			ResetDataStream();

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
