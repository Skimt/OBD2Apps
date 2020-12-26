using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading.Tasks;

namespace Logging.ELM327
{
	public class Config
	{

		public int BaudRate { get; } = 38400; // Live: 38400, Sleep: 9600
		public Parity Parity { get; } = Parity.None;
		public int DataBits { get; } = 8;
		public StopBits StopBits { get; } = StopBits.One;
		public Handshake Handshake { get; } = Handshake.None;
		public int ReadTimeout { get; } = 5000;
		public int WriteTimeout { get; } = 5000;

	}
}
