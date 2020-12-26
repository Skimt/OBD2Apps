using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Management.Models
{
    public partial class Car : IModel
    {

        public Car()
        {
            Sessions = new HashSet<Session>();
            CarBrand = new CarBrand();
            CarModel = new CarModel();
            Person = new Person();
            Marked = MarkedType.None;
        }

        public int CarId { get; set; }
        public string LicensePlate { get; set; }
        public double? Volume { get; set; }
        public DateTime? Year { get; set; }
        public byte? Fuel { get; set; }

        public int? CarBrandId { get; set; }
        public int? CarModelId { get; set; }
        public int? PersonId { get; set; }

        public virtual ICollection<Session> Sessions { get; set; }

        public virtual Person Person { get; set; }
        public virtual CarBrand CarBrand { get; set; }
        public virtual CarModel CarModel { get; set; }

        [NotMapped] public MarkedType Marked { get; set; }

    }
}
