using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Management.Models
{
    public partial class Configuration
    {

        public int ConfigurationId { get; set; }
        public int CarId { get; set; }
        public bool IsLoggingToDb { get; set; }

    }
}
