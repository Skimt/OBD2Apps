using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ELM327.Commands
{
	public abstract class Command : ICommand
	{

		private readonly int[] bytes;

		/// <summary>
		/// Initializes a new instance of the Command class. 
		/// </summary>
		public Command(string commandType, string pid, int bytesCount, UnitType unit = UnitType.None)
		{
			CommandType = commandType;
			PID = pid;
			BytesCount = bytesCount;
			Unit = unit;
			bytes = new int[BytesCount];
		}
		
		/// <summary>
		/// Determines whether this command is of type Attention ('AT') or Service ('01 - '09'). 
		/// </summary>
		public string CommandType { get; }

		/// <summary>
		/// The parameter identity that this command responds to. 
		/// </summary>
		public string PID { get; }

		/// <summary>
		/// The amount of bytes that a PID should contain in a response from the OBD. 
		/// </summary>
		public int BytesCount { get; }

		/// <summary>
		/// The value's unit type.  
		/// </summary>
		public UnitType Unit { get; }

		public virtual double Max { get; }
		public virtual double Min { get; }

		/// <summary>
		/// Returns the value of this command. Is typically updated when hexadecimals are converted to bytes. 
		/// </summary>
		public virtual double Value { get; }

		protected int A => bytes.Length > 0 ? bytes[0] : 0;
		protected int B => bytes.Length > 1 ? bytes[1] : 0;
		protected int C => bytes.Length > 2 ? bytes[2] : 0;
		protected int D => bytes.Length > 3 ? bytes[3] : 0;

		/// <summary>
		/// Returns the given string as an array of strings containing hexadeximals.
		/// </summary>
		public void ConvertToBytes(string response)
		{

			// Insert hexadecimals (bytes).
			for (int i = 0; i < BytesCount; i++)
			{
				bytes[i] = Convert.ToInt32(response.Substring(i * 2, 2), 16);
			}

		}

		/// <summary>
		/// Returns all child objects of <see cref="Command"/> in this assembly using reflection. 
		/// </summary>
		public static List<Command> GetCommandsInAssembly()
		{

			List<Command> commands = new List<Command>();

			// Where the Command class is defined. 
			Assembly assembly = Assembly.GetAssembly(typeof(Command));

			// The list of types in the assembly is narrowed down to the type known as Command. 
			TypeInfo[] types = assembly.DefinedTypes.Where(type => type != null && type.BaseType == typeof(Command)).ToArray();

			// We do not want to try/catch here because if something goes wrong, 
			// we are failing at reflection from the ground-up, which means that everything SHOULD be replaced. 
			foreach (TypeInfo type in types)
			{
				commands.Add((Command)assembly.CreateInstance(type.FullName));
			}

			return commands;

		}

	}
}
