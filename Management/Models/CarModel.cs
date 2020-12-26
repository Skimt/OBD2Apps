using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Models
{
	public partial class CarModel
	{
		public CarModel()
		{
			Cars = new HashSet<Car>();
		}
		public int CarModelId { get; set; }
		public string Name { get; set; }
		public virtual ICollection<Car> Cars { get; set; }
	}
}
