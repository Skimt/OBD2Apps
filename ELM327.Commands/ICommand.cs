using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELM327.Commands
{
	public interface ICommand
	{

		string CommandType { get; }
		string PID { get; }
		int BytesCount { get; }
		UnitType Unit { get; }
		double Max { get; }
		double Min { get; }
		double Value { get; }
		//void ConvertToBytes(string response);

	}
}
