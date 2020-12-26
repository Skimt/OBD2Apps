using System;

namespace Logging.ELM327.Commands
{
	public class Command
	{

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

	}
}
