using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Logging
{
	/// <summary>
	/// Is an extention of the <see cref="WebBrowser"/> class 
	/// that enables the user to navigate Web pages inside the form. 
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	public class Browser : WebBrowser
	{

		private string applicationPath;

		/// <summary>
		/// Initializes a new instance of the <see cref="Browser"/> class. 
		/// </summary>
		public Browser()
		{

			applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			// WebBrowser settings. 
			AllowNavigation = false;
			AllowWebBrowserDrop = false;
			Dock = DockStyle.Fill;
			Name = "browser";
			ScriptErrorsSuppressed = true;
			ScrollBarsEnabled = false;
			TabIndex = 0;

		}

		/// <summary>
		/// Inserts the given document (name) into the web browser. <para />
		/// The document must be located in the application folder, or in child folders of the application folder. 
		/// </summary>
		public void InsertDocument(string name)
		{
			try
			{
				DocumentText = File.ReadAllText(applicationPath + "\\" + name);
			}
			catch(Exception e)
			{
				MessageBox.Show(e.Message);
			}
		}

		/// <summary>
		/// Edit the markeup in the element of the selected Id. 
		/// </summary>
		public void EditInnerHtml(string elementId, string value)
		{
			HtmlElement elem = Document.GetElementById(elementId);
			if (elem == null)
				return;
			elem.InnerHtml = value;
		}

		/// <summary>
		/// Edit the style of the HTML element of the given Id. 
		/// </summary>
		public void EditStyle(string elementId, string value)
		{
			HtmlElement elem = Document.GetElementById(elementId);
			if (elem == null)
				return;
			elem.Style = value;
		}

		/// <summary>
		/// Sets the class of this element to 'show'. 
		/// Warning: Requires the class tag 'animation'.
		/// </summary>
		public void Show(string elementId)
		{
			HtmlElement elem = Document.GetElementById(elementId);
			elem.SetAttribute("className", "show");
		}

		/// <summary>
		/// Sets the class of this element to 'hide'. 
		/// Warning: Requires the class tag 'animation'.
		/// </summary>
		public void Hide(string elementId)
		{
			HtmlElement elem = Document.GetElementById(elementId);
			elem.SetAttribute("className", "hide");
		}

		/// <summary>
		/// Inserts the given stylesheet (name) into the web browser. <para />
		/// The stylesheet must be located in the application folder, or in child folders of the application folder. 
		/// </summary>
		public void InsertStylesheet(string styleName)
		{
			HtmlElement elem = Document.CreateElement("link");
			if (elem == null)
				return;
			elem.SetAttribute("rel", "stylesheet");
			elem.SetAttribute("type", "text/css");
			elem.SetAttribute("href", applicationPath + "\\" + styleName);
			Document.Body.AppendChild(elem);
		}
	}
}
