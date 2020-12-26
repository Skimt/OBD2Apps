using System;
using System.Windows.Forms;

namespace Logging
{
	static class Program
	{

		static Timer timer;

		[STAThread]
		static void Main()
		{

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			LoggingForm form = new LoggingForm();

			timer = new Timer
			{
				Interval = 1
			};
			timer.Tick += form.Update;
			timer.Start();

			Application.Run(form);
			
		}
	}
}
