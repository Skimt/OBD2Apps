using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Management.Models
{
    public partial class Person : IModel
    {

        public Person()
        {
            Cars = new HashSet<Car>();
            Marked = MarkedType.None;
        }

        public int PersonId { get; set; }
        public string GivenName { get; set; }
        public string SurName { get; set; }

        public virtual ICollection<Car> Cars { get; set; }

        [NotMapped] public Guid? IdentityId { get; set; }

        [NotMapped] public string FullName => GivenName + " " + SurName;
        [NotMapped] public string PersonUrl => "person/" + PersonId.ToString();
        [NotMapped] public MarkedType Marked { get; set; }

    }
}
