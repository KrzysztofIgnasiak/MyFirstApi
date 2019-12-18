using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWithUsers4.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Nip { get; set; }
        public Industry IndustryType { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public ApplicationUser userID { get; set; }

        public bool IsDeleted { get; set; }

        public List<TradeNote> TradeNotes{ get;set;}
        public List<ContactPerson> ContactPeople{ get;set;}

    }
}