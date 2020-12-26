using ELM327.Commands;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ELM327
{
	/// <summary>
	/// The Manager manages communication with the OBD. Use <see cref="RegisterCommand"/> to register commands. 
	/// See user manual for more information.
	/// </summary>
	public partial class ELM327Manager
	{

		private readonly List<Command> assemblyCommands;
		private List<Sender> ports = new List<Sender>();
		private IEnumerator<List<ICommand>> commandSet;
		private int startTime = Environment.TickCount;
		private bool _firstRun;
		private IDeviceDescription cmdDeviceDescription = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="ELM327Manager"/> class. 
		/// </summary>
		public ELM327Manager()
		{

			assemblyCommands = Command.GetCommandsInAssembly();
			RegisteredCommands = new List<ICommand>();
			Receiver = new Receiver(this);

		}

		/// <summary>
		/// Indicates that something is ready to be logged. <para />
		/// Requires asynchronous delegation.
		/// </summary>
		public event EventHandler<LogEventArgs> LogHandler;

		/// <summary>
		/// The local storage unit. 
		/// </summary>
		public DataTable Storage { get; set; }

		/// <summary>
		/// Contains a list of commands that have been registered in DataManager. 
		/// </summary>
		public List<ICommand> RegisteredCommands { get; private set; }

		/// <summary>
		/// The current request type in process. 
		/// </summary>
		public RequestType RequestType => Sender == null ? RequestType.OBD : RequestType.DataStream;

		/// <summary>
		/// Total requests made in the application's lifetime. 
		/// </summary>
		public int RequestCount { get; internal set; }

		/// <summary>
		/// Total response successions in the application's lifetime. 
		/// </summary>
		public int ResponseCount { get; internal set; }

		internal Sender Sender { get; set; }
		private Receiver Receiver { get; set; }

		/// <summary>
		/// Returns milliseconds based on the request type. 
		/// </summary>
		private int TimeElapsed => RequestType == RequestType.OBD ? 2000 : 200 + 10;

		/// <summary>
		/// Returns true when it is time to execute requests. 
		/// </summary>
		private bool IsRequestReady {
			get {
				int now = Environment.TickCount;
				int deltaTime = now - startTime;
				if (deltaTime < TimeElapsed) return false; 
				startTime = now;
				return true;
			}
		}

		/// <summary>
		/// Default threshold is 40. 
		/// </summary>
		public int StorageThreshold { get; set; } = 40;

		/// <summary>
		/// 
		/// </summary>
		public bool IsStorageThresholdReached => Storage != null && Storage.Rows.Count > StorageThreshold;

		/// <summary>
		/// Execute requests that are regulated internally by ELM327. 
		/// </summary>
		public void Run()
		{

			InitalizeCommands();

			if (!IsRequestReady)
			{
				return;
			}

			RequestCount++;

			// Clear storage.
			if (IsStorageThresholdReached)
			{
				Storage.Rows.Clear();
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
		/// Sets the storage with the given SessionId. 
		/// </summary>
		public void SetStorage(int sessionId)
		{
			Storage = new DataTable("LogEntry");
			Storage.Columns.AddRange(new DataColumn[] {
				new DataColumn("Date", typeof(DateTime)),
				new DataColumn("PID", typeof(string)),
				new DataColumn("Value", typeof(double)),
				new DataColumn("SessionId", typeof(int))
			});

			Storage.Columns["SessionId"].DefaultValue = sessionId;
		}

		/// <summary>
		/// Register commands of service type 1.
		/// </summary>
		public void RegisterCommand<T>() where T : class, ICommand
		{

			ICommand command = assemblyCommands.FirstOrDefault(c => c is T);

			if (command == null)
			{
				throw new Exception(string.Format("Could not find command of type '{0}'.", typeof(T)));
			}

			RegisteredCommands.Add(command);

		}

		/// <summary>
		/// Triggers the <see cref="LogHandler"/> so that the log may be distributed outside the assembly. 
		/// </summary>
		internal void Log(string text)
		{
			LogHandler?.Invoke(this, new LogEventArgs(text));
		}

		/// <summary>
		/// Initializes the selected commands. 
		/// </summary>
		private void InitalizeCommands()
		{

			if (_firstRun)
			{
				return;
			}

			List<List<ICommand>> assortedCommands = GetAssortedCommands(RegisteredCommands, new string[] { "01" });
			commandSet = GetCommandSets(assortedCommands);

			_firstRun = true;

		}

		/// <summary>
		/// Request a stream of data from the OBD. 
		/// </summary>
		private void RequestDataStream()
		{

			// Reset.
			if (!commandSet.MoveNext())
			{
				commandSet.Dispose(); 
			}

			try
			{
				Sender.Request(commandSet.Current);
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

			// Dispose of (close) old connections. 
			Parallel.ForEach(ports, (port) =>
			{
				port.Dispose();
			});
			ports.Clear(); // Remove trash. 

			// Grab new ports. 
			ports = Sender.GetPorts();
			Log(string.Format("Found {0} communication ports.", ports.Count));

			// Get the command used to search for the OBD. 
			if (cmdDeviceDescription == null)
			{
				cmdDeviceDescription = GetCommandOfType<IDeviceDescription>(assemblyCommands) as IDeviceDescription;
			}

			// Search for OBD. 
			Parallel.ForEach(ports, (port) =>
			{

				port.DataReceived += Receiver.OnDataReceived;

				try
				{
					port.Request(cmdDeviceDescription);
				}
				catch (Exception e)
				{
					Log(e.Message);
				}

			});

		}

	}
}
