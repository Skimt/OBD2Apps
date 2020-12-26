using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Models
{
	public partial class CarBrand
	{
		public CarBrand()
		{
			Cars = new HashSet<Car>();
		}
		public int CarBrandId { get; set; }
		public string Name { get; set; }
		public virtual ICollection<Car> Cars { get; set; }
	}
}
