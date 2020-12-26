using System;
using System.Text.RegularExpressions;

namespace ELM327
{
	internal class ResponseHandler
	{

		private readonly Regex PatternWhitespace = new Regex(@"(\s+)|(\>+)", RegexOptions.IgnoreCase);
		private readonly Regex PatternBytesValue = new Regex(@"(\w|\d){3}(?=\d\:)", RegexOptions.IgnoreCase);
		private readonly Regex PatternBytesLength = new Regex(@"(\w|\d){3}(\d\:)", RegexOptions.IgnoreCase);
		private readonly Regex PatternIndexes = new Regex(@"(\d\:)", RegexOptions.IgnoreCase);

		/// <summary>
		/// Returns a clean response string, free from whitespace, markers, etc. 
		/// </summary>
		protected string Clean(string response)
		{

			response = RemoveWhitespace(response);

			CheckAvailability(response);
			CheckReliability(response);

			if (response.Contains(":"))
			{
				response = GetLongResponse(response);
			}
			else
			{
				response = GetShortResponse(response);
			}

			CheckBytes(response);

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
		/// Used when the response length is so long that it contains indexation. 
		/// Removes the request, bytes value, index values, semicolons, and main response code ("41"). 
		/// </summary>
		private string GetLongResponse(string response)
		{
			Match bytes = GetBytes(response);															// This NEEDS to be retrieved PRIOR to Replace(). 
			response = PatternBytesLength.Replace(response, string.Empty, 1);							// Removes the bytes value. 
			response = PatternIndexes.Replace(response, string.Empty);									// Removes indexes and semicolons. 
			return response.Substring(bytes.Index + 2, (Convert.ToInt32(bytes.Value, 16) * 2) - 2);		// Returns the main response.
		}

		/// <summary>
		/// Used when the response length is so short that it doesn't contain indexation. 
		/// Removes the request, and the main response code ("41"). 
		/// </summary>
		private string GetShortResponse(string response)
		{

			CheckLength(response);

			int responseIndex = 0;

			// Should be the first PID in the sequence. 
			string firstPID = GetHex(response, 2); 

			// Iterate through the response string. 
			for (int i = 0; i < response.Length - 1; i++)
			{

				// Hexadecimals are represented as... 
				string hex = GetHex(response, i);

				// This is not a "41" response code. 
				if (!hex.Contains("41"))
				{
					continue;
				}

				string firstResponsePID = GetHex(response, i + 2);

				// PIDs match. This should indeed be "41". 
				if (firstPID == firstResponsePID)
				{
					responseIndex = i;
					break;
				}

			}

			return response.Substring(responseIndex + 2, response.Length - responseIndex - 2);

		}

		/// <summary>
		/// Returns the hext at the given index. 
		/// </summary>
		private string GetHex(string response, int index)
		{
			return response[index].ToString() + response[index + 1].ToString();
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
				throw new Exception("Unable to read index or length of bytes in long response: " + response);
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
			if (response.Length < 10)
			{
				throw new Exception("The following response is too short to be genuine: " + response);
			}
		}

		private void CheckBytes(string response)
		{
			if (!(response.Length % 2 == 0))
			{
				throw new Exception("Expected whole bytes, but found half-byte in the response: " + response);
			}
		}

	}
}