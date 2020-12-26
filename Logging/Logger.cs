using ELM327;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Logging
{

	/// <summary>
	/// The logger serves to log the program's behavior,
	/// and is not to be confused with the form itself. 
	/// </summary>
	public class Logger
	{

		private readonly string ApplicationPath;
		private readonly StringBuilder sb;

		public Logger()
		{
			ApplicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			sb = new StringBuilder();
		}

		public int LogCount { get; private set; }

		/// <summary>
		/// Write string to log.txt
		/// </summary>
		public void Write(object sender, LogEventArgs eventArgs)
		{

			if (string.IsNullOrEmpty(ApplicationPath))
			{
				return;
			}

			try
			{
				using (StreamWriter writer = File.AppendText(ApplicationPath + "\\" + "log.txt"))
				{
					writer.Write(string.Format("{0}: {1}\r\n", DateTime.Now.ToString("yy.MM.dd HH:mm:ss.fff"), eventArgs.Message));
					writer.Close();
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message); 
			}

			LogCount++;

		}

		/// <summary>
		/// Creates log.txt and writes when the log started. 
		/// </summary>
		public void WriteHeader()
		{
			sb.Clear();
			sb.AppendLine("==== Log started successfully ====");
			sb.AppendLine(DateTime.Now.ToString());
			sb.AppendLine("==================================");
			sb.AppendLine();
			File.WriteAllText(ApplicationPath + "\\" + "log.txt", sb.ToString());
			sb.Clear();
		}

		/// <summary>
		/// Simply writes to log.txt when the log stopped. 
		/// </summary>
		public void WriteFooter()
		{
			sb.Clear();
			sb.AppendLine();
			sb.AppendLine("==== Log stopped successfully ====");
			sb.AppendLine(DateTime.Now.ToString());
			sb.AppendLine("==================================");
			try
			{
				using (StreamWriter writer = File.AppendText(ApplicationPath + "\\" + "log.txt"))
				{
					writer.WriteLine(sb.ToString());
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

	}

}
