using ELM327;
using System;
using System.Windows.Forms;

namespace Logging
{
	partial class LoggingForm 
	{

		private void LoggingForm_OnLogReceived(object sender, LogEventArgs eventArgs)
		{

			if (InvokeRequired)
			{
				BeginInvoke(new EventHandler<LogEventArgs>(Logger.Write), sender, eventArgs);
			}
			else
			{
				Logger.Write(sender, eventArgs);
			}

		}

		private void LoggingForm_OnLoad(object sender, EventArgs e)
		{

			Logger.WriteHeader();

		}

		private void LoggingForm_OnClosing(object sender, FormClosingEventArgs e)
		{

			Logger.WriteFooter();

		}

	}
}
