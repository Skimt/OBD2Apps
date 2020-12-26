using Logging.ELM327.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.ELM327.Commands
{
	public class CommandTools
	{

		/// <summary>
		/// Turns a list of lists into an enumerator (state machine). 
		/// This method is normally used to distribute load (to improve performance)
		/// but in this case it is used to distribute commands over time. 
		/// </summary>
		public IEnumerator<List<Command>> GetCommandSets(List<List<Command>> assortedCommands)
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
		public T GetCommandOfType<T>(List<Command> commands) where T : Command
		{

			// Warning! Visual Studio wants to complicate the readability of this code. 
			T command = commands.FirstOrDefault(c => c.GetType() == typeof(T)) as T;

			if (command == null)
			{
				throw new Exception(string.Format("Could not find command of type '{0}'.", typeof(T)));
			}

			return command;

		}

		/// <summary>
		/// Returns the commands that are being used to retrieve data from the OBD. 
		/// </summary>
		public List<Command> GetActiveCommands(List<List<Command>> assortedCommands)
		{
			return assortedCommands.SelectMany(c => c).ToList();
		}

		/// <summary>
		/// Splits the given list into chunks of smaller lists. 
		/// Requires command type, e.g. "AT" or service "01". 
		/// Method rewritten from user Serj-Tm's example at https://stackoverflow.com/a/11463800.
		/// </summary>
		public List<List<Command>> GetCommands(List<Command> commands, string[] commandTypes, int byteLength = 6)
		{

			List<List<Command>> list = new List<List<Command>>();

			foreach (string commandType in commandTypes)
			{
				for (int i = 0; i < commands.Count; i += byteLength)
				{
					list.Add(commands.GetRange(i, Math.Min(byteLength, commands.Count - i)).Where(c => c.CommandType == commandType).ToList());
				}
			}

			return list;

		}

		/// <summary>
		/// Instantiates given command and adds it to the commands list. 
		/// </summary>
		public void RegisterCommandOfType<T>(List<Command> commands) where T : Command, new()
		{
			commands.Add(new T());
		}

	}
}
