using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Management.Models
{
    public partial class LogEntry : IModel
    {

        public LogEntry()
        {
            Marked = MarkedType.None;
        }

        public int LogEntryId { get; set; }
        public DateTime? Date { get; set; }
        public int? SessionId { get; set; }
        public string Pid { get; set; }
        public float? Value { get; set; }

        public virtual Session Session { get; set; }

        [NotMapped] public int PageCount { get; set; }
        [NotMapped] public MarkedType Marked { get; set; }

    }
}
