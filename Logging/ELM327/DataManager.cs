using Logging.ELM327.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logging.ELM327
{
	public class DataManager : CommandTools
	{

		/// <summary>
		/// Used to send data to the OBD.  
		/// </summary>
		public Sender Sender { get; set; }

		/// <summary>
		/// Used to retrieve data from the OBD. 
		/// </summary>
		public Receiver Receiver { get; set; }

		/// <summary>
		/// The current request type in process. 
		/// </summary>
		public RequestType RequestType { get; private set; } = RequestType.OBD;

		/// <summary>
		/// Contains a list of commands that have been registered in DataManager. 
		/// </summary>
		public readonly List<Command> ActiveCommands;

		/// <summary>
		/// Indicates that something is ready to be logged. 
		/// </summary>
		public Action<string> LogReceived;

		private readonly List<Command> Commands;
		private readonly List<List<Command>> AssortedCommands;
		private readonly IEnumerator<List<Command>> CommandSet;
		private int startTime = Environment.TickCount;

		/// <summary>
		/// Initializes a new instance of the <see cref="DataManager"/> class. 
		/// </summary>
		public DataManager()
		{

			Commands = new List<Command>();

			RegisterCommandOfType<DeviceDescription>(Commands);
			RegisterCommandOfType<P04EngineCoolantTemp>(Commands);
			RegisterCommandOfType<P05EngineLoad>(Commands);
			RegisterCommandOfType<P0CEngineRPM>(Commands);
			RegisterCommandOfType<P0DVehicleSpeed>(Commands);
			RegisterCommandOfType<P0FIntakeAirTemp>(Commands);
			RegisterCommandOfType<P2CCommandedEGR>(Commands);
			RegisterCommandOfType<P2DEGRError>(Commands);
			RegisterCommandOfType<P33AbsBarometricPressure>(Commands);

			AssortedCommands = GetCommands(Commands, new string[] { "01" });
			CommandSet = GetCommandSets(AssortedCommands);
			ActiveCommands = GetActiveCommands(AssortedCommands); // TODO: This may be redundant...

			Receiver = new Receiver(this);

		}

		/// <summary>
		/// Returns true when it is time to execute requests. 
		/// </summary>
		public bool IsRequestReady {
			get {
				int now = Environment.TickCount;
				int time = now - startTime;
				if (time < RuntimeMs) return false; 
				startTime = now;
				return true;
			}
		}

		private int RuntimeMs => RequestType == RequestType.OBD ? 3000 : 200 + 10;

		/// <summary>
		/// Executes requests depending on the <see cref="ELM327.RequestType"/> that has been set. 
		/// </summary>
		public void ExecuteRequests()
		{

			RequestType = Sender == null ? RequestType.OBD : RequestType.DataStream;

			if (!IsRequestReady)
			{
				return;
			}

			switch (RequestType)
			{
				case RequestType.OBD:
					RequestOBD();
					break;
				case RequestType.DataStream:
					RequestDataStream();
					break;
			}

		}

		/// <summary>
		/// Request a stream of data from the OBD. 
		/// </summary>
		private void RequestDataStream()
		{

			// Reset.
			if (!CommandSet.MoveNext())
			{
				CommandSet.Dispose(); 
			}

			try
			{
				Sender.Request(CommandSet.Current);
			}
			catch (Exception e)
			{
				Log(e.Message);
			}

		}

		/// <summary>
		/// Request OBD from all known ports in parallel. 
		/// </summary>
		private void RequestOBD()
		{

			List<Sender> ports = Sender.GetPorts();
			Log(string.Format("Found {0} communication ports.", ports.Count));

			Parallel.ForEach(ports, (port) =>
			{

				port.DataReceived += Receiver.DataManager_OnDataReceived;
				DeviceDescription command = GetCommandOfType<DeviceDescription>(Commands);

				try
				{
					port.Request(command);
				}
				catch (Exception e)
				{
					Log(e.Message);
				}


			});

		}

		/// <summary>
		/// Retrieves command from commands list based on PID. 
		/// </summary>
		public Command GetCommand(string pid)
		{
			return Commands.FirstOrDefault(c => c.PID == pid) ?? throw new Exception("Could not retrieve command of given PID.");
		}

		/// <summary>
		/// Triggers <see cref="LogReceived"/>. 
		/// </summary>
		public void Log(string text)
		{
			LogReceived(text);
		}

	}
}
