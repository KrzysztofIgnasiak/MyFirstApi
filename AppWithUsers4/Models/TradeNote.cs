using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWithUsers4.Models
{
    public class TradeNote
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public bool IsDeleted { get; set; }

        public Company CompanyId { get; set; }

        public ApplicationUser UserId { get; set; }

    }
}