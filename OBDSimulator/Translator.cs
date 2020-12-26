using ELM327.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace OBDSimulator
{
	internal class Translator
	{

		private readonly Regex PatternWhitespace = new Regex(@"(\s+)", RegexOptions.IgnoreCase);
		private readonly Random ran = new Random();

		/// <summary>
		/// Converts the command type to an integer and adds 40 to said value.
		/// </summary>
		private int GetCommandTypeValue(string commandType)
		{
			int.TryParse(commandType, out int commandNum);
			return commandNum + 40;
		}

		/// <summary>
		/// Returns a complete array containing the command type value (40 + 01 = 41), PIDs, and PID byte values. 
		/// </summary>
		public string[] GetResponseValues(string commandType, List<Command> commands, int bytesTotal)
		{

			// Create an array based on pids total and bytes total. 
			string[] values = new string[commands.Count + bytesTotal + 1];
			int valuesIndex = 1;

			// Convert the command type to a value, e.g. 40 + 1 = 41. 
			values[0] = GetCommandTypeValue(commandType).ToString();

			foreach (Command command in commands)
			{

				// Add PID
				values[valuesIndex++] = command.PID;

				// Add dummy data
				for (int i = 0; i < command.BytesCount; i++)
				{
					values[valuesIndex++] = ran.Next(0, 255).ToString("X2");
				}

			}

			return values;

		}

		/// <summary>
		/// Returns the sum total bytes of all the commands in the given list. 
		/// </summary>
		public int GetBytesTotal(List<Command> commands)
		{
			return commands.Count > 0 ? commands.Sum(command => command.BytesCount) : 0;
		}

		/// <summary>
		/// Removes whitespace, newlines, and right angle brackets from the given string. 
		/// Whitespace and newlines are an unreliable source of information.
		/// </summary>
		public string Clean(string response)
		{
			return PatternWhitespace.Replace(response, string.Empty);
		}

		/// <summary>
		/// Returns an array of hexadecimals from the given response string. 
		/// </summary>
		public string[] GetHexadecimals(string response)
		{
			string[] chunks = new string[response.Length / 2];
			for (int i = 0; i < chunks.Length; i++)
			{
				chunks[i] = response.Substring(i * 2, 2);
			}
			return chunks;
		}

	}
}
