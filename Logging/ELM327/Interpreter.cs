using System;
using System.Text.RegularExpressions;

namespace Logging.ELM327
{
	public class Interpreter
	{

		private readonly Regex PatternWhitespace = new Regex(@"(\s+)|(\>+)", RegexOptions.IgnoreCase);
		private readonly Regex PatternBytesValue = new Regex(@"(\w|\d){3}(?=\d\:)", RegexOptions.IgnoreCase);
		private readonly Regex PatternBytesLength = new Regex(@"(\w|\d){3}(\d\:)", RegexOptions.IgnoreCase);
		private readonly Regex PatternIndexes = new Regex(@"(\d\:)", RegexOptions.IgnoreCase);

		/// <summary>
		/// Returns a clean response string. 
		/// </summary>
		public string GetCleanResponse(string response)
		{
			response = RemoveWhitespace(response);
			CheckAvailability(response);
			CheckReliability(response);
			response = RemoveMarkers(response);
			CheckLength(response);
			return response;
		}

		/// <summary>
		/// Removes whitespace, newlines, and right angle brackets from the given string. 
		/// Whitespace and newlines are an unreliable source of information.
		/// </summary>
		private string RemoveWhitespace(string response)
		{
			return PatternWhitespace.Replace(response, string.Empty);
		}

		/// <summary>
		/// Removes bytes value, index values, semicolons, and main response code. 
		/// </summary>
		private string RemoveMarkers(string response)
		{
			Match bytes = GetBytes(response); // This NEEDS to be retrieved PRIOR to Replace(). 
			response = PatternBytesLength.Replace(response, string.Empty, 1); // Remove the bytes value. 
			response = PatternIndexes.Replace(response, string.Empty); // Remove indexes and semicolons. 
			return response.Substring(bytes.Index + 2, (Convert.ToInt32(bytes.Value, 16) * 2) - 2); // Main response code.
		}

		/// <summary>
		/// This finds out how many bytes the OBD responded with (1+2 digit hexadecimals). 
		/// Beware that this regular expression can find more than one match.
		/// </summary>
		private Match GetBytes(string response)
		{

			Match bytes = PatternBytesValue.Match(response);

			if (!bytes.Success)
			{
				throw new Exception("Unable to read index or length of bytes in response: " + response);
			}

			return bytes;

		}

		private void CheckAvailability(string response)
		{
			if (response.Contains("SEARCHING...STOPPED") || response.Contains("NODATA"))
			{
				throw new Exception("Unable to retrieve data in response: " + response);
			}
		}

		private void CheckReliability(string response)
		{

			if (response.Contains("<RXERROR"))
			{
				throw new Exception("Presuming the car's ignition is being turned off in the response: " + response);
			}

		}

		private void CheckLength(string response)
		{
			if (!(response.Length % 2 == 0))
			{
				throw new Exception("Expected whole bytes, but found half-byte in the response: " + response);
			}
		}

	}
}