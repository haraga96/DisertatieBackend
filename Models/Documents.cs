using System;
using System.Collections.Generic;

namespace Backend_Dis_App.Models
{
    public partial class Documents
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ValueDue { get; set; }
        public int CountryId { get; set; }
        public int UserId { get; set; }

        public virtual Country Country { get; set; }
        public virtual User User { get; set; }
    }
}
