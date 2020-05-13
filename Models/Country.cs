using System;
using System.Collections.Generic;

namespace Backend_Dis_App.Models
{
    public partial class Country
    {
        public Country()
        {
            Documents = new HashSet<Documents>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Documents> Documents { get; set; }
    }
}
