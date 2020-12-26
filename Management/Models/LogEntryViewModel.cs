using System.Collections.Generic;

namespace Management.Models
{
	public class LogEntryViewModel
	{
		public IEnumerable<LogEntry> LogEntries { get; set; }
		public int RowCount { get; set; }
	}
}
