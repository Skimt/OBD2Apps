using ELM327.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELM327
{
	partial class ELM327Manager
	{

		/// <summary>
		/// Retrieves command from commands list based on PID. 
		/// </summary>
		internal Command GetCommand(string pid)
		{
			return assemblyCommands.FirstOrDefault(c => c.PID == pid) ?? throw new Exception("Could not retrieve command of given PID.");
		}

		/// <summary>
		/// Turns a list of lists into an enumerator (state machine). 
		/// This method is normally used to distribute load (to improve performance)
		/// but in this case it is used to distribute commands over time. 
		/// </summary>
		protected IEnumerator<List<ICommand>> GetCommandSets(List<List<ICommand>> assortedCommands)
		{

			int n = 0;
			do
			{
				yield return assortedCommands[n];
				n++;
				if (n > assortedCommands.Count - 1)
				{
					n = 0;
				}
			}
			while (true);

		}

		/// <summary>
		/// Return the command of the specific type. 
		/// </summary>
		protected ICommand GetCommandOfType<T>(List<Command> commands)
		{

			ICommand command = commands.FirstOrDefault(c => c is T);

			if (command == null)
			{
				throw new Exception(string.Format("Could not find command of type '{0}'.", typeof(T)));
			}

			return command;

		}

		/// <summary>
		/// Splits the given list into chunks of smaller lists. 
		/// Requires command type, e.g. "AT" or service "01". 
		/// Method rewritten from user Serj-Tm's example at https://stackoverflow.com/a/11463800.
		/// </summary>
		protected List<List<ICommand>> GetAssortedCommands(List<ICommand> commands, string[] commandTypes, int byteLength = 6)
		{

			List<List<ICommand>> list = new List<List<ICommand>>();

			foreach (string commandType in commandTypes)
			{
				for (int i = 0; i < commands.Count; i += byteLength)
				{
					list.Add(commands.GetRange(i, Math.Min(byteLength, commands.Count - i)).Where(c => c.CommandType == commandType).ToList());
				}
			}

			return list;

		}

	}
}
