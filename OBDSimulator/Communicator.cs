using System;
using System.IO.Ports;
using System.Text;

namespace OBDSimulator
{
	/// <summary>
	/// Contains the method used with <see cref="Port.DataReceived"/>.
	/// </summary>
	internal class Communicator
	{

		private readonly OBDSimulator OBD;
		private readonly Translator Translator;
		private readonly StringBuilder sb = new StringBuilder();

		/// <summary>
		/// Initializes a new instance of the <see cref="Communicator"/> class. 
		/// </summary>
		internal Communicator(OBDSimulator obd)
		{
			OBD = obd;
			Translator = new Translator();
		}

		public DateTime LastReceived { get; set; } = DateTime.Now;

		/// <summary>
		/// Returns whether the response contains a single PID or multiple PIDs, or if the response is false (None). 
		/// </summary>
		public ResponseType ResponseType {
			get {
				switch (OBD.Commands.Count)
				{
					case 0:
						return ResponseType.None;
					case 1:
						return ResponseType.Single;
					default:
						return ResponseType.Multiple;
				}
			}
		}

		/// <summary>
		/// Triggers when the ELM327 has passed the simulator a request. 
		/// </summary>
		internal void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
		{

			LastReceived = DateTime.Now;

			Port port = sender as Port;

			sb.Clear();

			string elm327Response = Translator.Clean(port.ReadLine());

			// Ignore requests under 4 characters long.
			if (elm327Response.Length < 4)
				return;

			string[] pids = Translator.GetHexadecimals(elm327Response);
			string commandType = pids[0]; // e.g. "AT" or "01"

			OBD.SetCommands(pids, commandType);

			if (ResponseType == ResponseType.None)
			{
				return;
			}

			if (commandType.Contains("AT") && OBD.Commands.Exists(command => command.PID.Contains("@1")))
			{
				sb.Append("OBDII to RS232 Interpreter");
				OBD.IsELM327 = true;
			}
			else
			{

				// Add the initial response that should be returned to ELM327. 
				sb.AppendLine(elm327Response);

				// Retrieve total bytes, excluding commandNumResponse. 
				int bytesTotal = Translator.GetBytesTotal(OBD.Commands);

				// Retrieve response values. 
				string[] values = Translator.GetResponseValues(commandType, OBD.Commands, bytesTotal);

				if (ResponseType == ResponseType.Single)
				{

					// Add the dummy data for this single request. 
					for (int i = 0; i < values.Length; i++)
					{

						sb.Append(values[i]);

						// Add spaces.
						if (i < values.Length - 1)
						{
							sb.Append(' ');
						}

					}

				}
				else
				{

					// Add the bytes count to the response that should be returned to ELM327. 
					sb.Append((bytesTotal + OBD.Commands.Count + 1).ToString("X3"));

					int bamIndex = 0;
					for (int i = 0; i < values.Length; i++)
					{

						// Add index
						if (i % 7 == 0)
						{
							sb.Append(Environment.NewLine + bamIndex + ": ");
							bamIndex++;
						}

						// Add value.
						sb.Append(values[i] + ' ');

					}

				}

			}

			sb.Append(Environment.NewLine + ">");
			Console.WriteLine(sb.ToString());
			port.WriteLine(sb.ToString());

		}

	}
}
