using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.ELM327
{
	/// <summary>
	/// Different types of requests such as searching for the OBD device and retrieving live data. 
	/// </summary>
	public enum RequestType
	{
		OBD,
		DataStream
	}
}
