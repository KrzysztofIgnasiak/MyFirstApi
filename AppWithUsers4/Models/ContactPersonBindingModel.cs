using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWithUsers4.Models
{
    public class ContactPersonBindingModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int? Phone { get; set; }

        public string Mail { get; set; }

        public string Position { get; set; }
        
        public int? CompanyId { get; set; }
    }
}