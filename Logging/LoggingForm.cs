using ELM327;
using ELM327.Commands;
using ELM327.MSSQL;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace Logging
{
	public partial class LoggingForm : Form
	{

		public LoggingForm()
		{

			InitializeComponent();
			InitializeBrowser();
			InitializeDatabase();

			// Setup ELM327Manager and application logger.
			Logger = new Logger();
			ELM327Manager = new ELM327Manager();
			ELM327Manager.LogHandler += LoggingForm_OnLogReceived;

			// Register desirable commands to log. 
			ELM327Manager.RegisterCommand<IP04EngineCoolantTemp>();
			ELM327Manager.RegisterCommand<IP05EngineLoad>();
			ELM327Manager.RegisterCommand<IP0CEngineRPM>();
			ELM327Manager.RegisterCommand<IP0DVehicleSpeed>();
			ELM327Manager.RegisterCommand<IP0FIntakeAirTemp>();
			ELM327Manager.RegisterCommand<IP2CCommandedEGR>();
			ELM327Manager.RegisterCommand<IP2DEGRError>();
			ELM327Manager.RegisterCommand<IP33AbsBarometricPressure>();

		}

		public Browser Browser { get; set; }
		public Logger Logger { get; }
		public ELM327Manager ELM327Manager { get; }
		public OBDSql OBDSql { get; private set; }
		public OBDConfig OBDConfig { get; private set; }

		/// <summary>
		/// The runtime of this method is configured in the <see cref="Program"/> class. 
		/// </summary>
		public void Update(object sender, EventArgs e)
		{

			ELM327Manager.Run();

			// Needs a valid configuration from the database. 
			if (OBDConfig == null)
				return;

			if (OBDConfig.IsLoggingToDb)
			{ 
				UpdateDatabase();
			}
			else
			{
				// Used when not storing to db. 
				if (ELM327Manager.Storage == null) 
					ELM327Manager.SetStorage(1);
			}

			UpdateBrowser();

		}

		/// <summary>
		/// Updates the database. 
		/// </summary>
		private void UpdateDatabase()
		{

			// Exit. Application doesn't have a valid OBD device. 
			if (ELM327Manager.RequestType != RequestType.DataStream)
				return;

			// Create storage. 
			if (ELM327Manager.Storage == null)
			{
				int sessionId = OBDSql.GetSession(OBDConfig.CarId); // Do not use try/catch. Let the program crash if it fails to retrieve session id.  
				ELM327Manager.SetStorage(sessionId);
			}

			// Insert to database when the threshold is reached. 
			if (ELM327Manager.Storage.Rows.Count >= OBDSql.StorageThreshold)
				OBDSql.Insert(ELM327Manager.Storage);

		}

		/// <summary>
		/// Updates the contents of the browser.  
		/// </summary>
		private void UpdateBrowser()
		{

			// Browser control isn't ready. 
			if (Browser.ReadyState != WebBrowserReadyState.Complete)
				return;

			// When searching for OBD.
			SetLoaderVisbility();

			// Display values from commands. 
			foreach (ICommand command in ELM327Manager.RegisteredCommands)
			{
				Browser.EditInnerHtml("pid-" + command.PID, command.Value.ToString("N0"));
			}

			// Display car id and whether storing to db or not. 
			Browser.EditInnerHtml("carid", OBDConfig.CarId.ToString());
			Browser.EditInnerHtml("is-logging", OBDConfig.IsLoggingToDb ? "True" : "False");

			// Display various count values. 
			Browser.EditInnerHtml("requests", ELM327Manager.RequestCount.ToString());
			Browser.EditInnerHtml("replies", ELM327Manager.ResponseCount.ToString());
			Browser.EditInnerHtml("stored", OBDSql.StoreTotal.ToString());
			Browser.EditInnerHtml("logs", Logger.LogCount.ToString());

		}

		/// <summary>
		/// Displays the loader when searching for OBD. 
		/// </summary>
		private void SetLoaderVisbility()
		{

			if (ELM327Manager.RequestType == RequestType.OBD)
			{
				Browser.Show("loader");
				return;
			}

			Browser.Hide("loader");

		}

		/// <summary>
		/// Sets up the obd database and config. 
		/// </summary>
		private void InitializeDatabase()
		{

			OBDSql = new OBDSql(ConfigurationManager.ConnectionStrings["OBDConnectionString"].ConnectionString);

			try
			{
				OBDConfig = OBDSql.GetConfiguration();
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}

		}

		/// <summary>
		/// Sets up the Browser. 
		/// </summary>
		private void InitializeBrowser()
		{
			Browser = new Browser();
			Controls.Add(Browser);
			Browser.InsertDocument("index.html");
			Browser.DocumentCompleted += Browser_DocumentCompleted;
		}

		/// <summary>
		/// Akin to document.ready in JavaScript. 
		/// </summary>
		private void Browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			Browser.Document.Body.MouseUp += Browser_OnMouseUp;
			Browser.Document.Body.KeyUp += Browser_OnKeyUp;
			Browser.InsertStylesheet("bootstrap.min.css");
			Browser.InsertStylesheet("style.css");
		}

		/// <summary>
		/// Triggers when the user executes an OnMouseUp event. 
		/// </summary>
		private void Browser_OnMouseUp(object sender, HtmlElementEventArgs e)
		{

			HtmlElement element = Browser.Document.ActiveElement;

			if (element == null)
				return;

			switch (element.Id)
			{
				case "close1":
				case "close2":
				case "close3":
					Close();
					break;
			}

		}

		/// <summary>
		/// Triggers when the user executes an OnKeyUp event. 
		/// </summary>
		private void Browser_OnKeyUp(object sender, HtmlElementEventArgs e)
		{

			if (e.KeyPressedCode != ASCIIKeys.Escape)
				return;

			BeginInvoke(new EventHandler(delegate
			{
				Close();
			}));

		}

	}
}
