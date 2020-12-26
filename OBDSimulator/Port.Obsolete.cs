using System;
using System.IO.Ports;

namespace OBDSimulator
{
	/// <summary>
	/// Is an extention of the <see cref="SerialPort"/> class, which represents a serial port resource. 
	/// To browser the .NET Framework source code for this type, see the Reference Source. 
	/// </summary>
	internal partial class Port
	{

		/// <summary>
		/// Use <see cref="WriteLine"/>
		/// </summary>
		[Obsolete]
		public new void Write(string text) { }

		/// <summary>
		/// Use <see cref="ReadLine"/>
		/// </summary>
		[Obsolete]
		public new void Read(byte[] buffer, int offset, int count) { }

		/// <summary>
		/// Use <see cref="ReadLine"/>
		/// </summary>
		[Obsolete]
		public new void ReadChar() { }

		/// <summary>
		/// Use <see cref="ReadLine"/>
		/// </summary>
		[Obsolete]
		public new void ReadByte() { }

		/// <summary>
		/// Use <see cref="ReadLine"/>
		/// </summary>
		[Obsolete]
		public new void ReadExisting() { }

		/// <summary>
		/// Use <see cref="ReadLine"/>
		/// </summary>
		[Obsolete]
		public new void ReadTo(string value) { }

	}
}
