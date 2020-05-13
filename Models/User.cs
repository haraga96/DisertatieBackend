using System;
using System.Collections.Generic;

namespace Backend_Dis_App.Models
{
    public partial class User
    {
        public User()
        {
            Documents = new HashSet<Documents>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

        public virtual ICollection<Documents> Documents { get; set; }
    }
}
