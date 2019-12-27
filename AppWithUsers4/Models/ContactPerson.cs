using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWithUsers4.Models
{
    public class ContactPerson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int? Phone { get; set; }

        public string Mail { get; set; }

        public string Position { get; set; }

        public Company CompanyId { get; set; }
        public ApplicationUser UserId { get; set; }

        public bool isDeleted { get; set; }
    }
}