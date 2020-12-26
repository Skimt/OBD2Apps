using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Models
{
	public interface IModel
	{

		[NotMapped]
		public MarkedType Marked { get; set; }

	}
}
