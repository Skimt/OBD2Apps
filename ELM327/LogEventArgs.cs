using System;

namespace ELM327
{
	/// <summary>
	/// LogEventArgs is used with EventHandler in ELM327.Manager
	/// </summary>
	public class LogEventArgs : EventArgs
	{

		public LogEventArgs(string message)
		{
			Message = message;
		}

		public string Message { get; set; }

	}
}
