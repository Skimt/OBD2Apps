using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Management.Models
{
    public partial class Session : IModel
    {

        public Session()
        {
            LogEntries = new HashSet<LogEntry>();
            Marked = MarkedType.None;
        }

        public int SessionId { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateStop { get; set; }
        public int CarId { get; set; }

        public virtual Car Car { get; set; }
        public virtual ICollection<LogEntry> LogEntries { get; set; }

        [ForeignKey("SessionId")]
        public LogEntry LogEntry { get; set; }

        [NotMapped] public virtual int LogEntryCount { get; set; }
        [NotMapped] public MarkedType Marked { get; set; }

    }
}
