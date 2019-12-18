using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWithUsers4.Models
{
    public class TradeNoteAddUpdateBindingModel
    {
        public string Text { get; set; }

        public int CompanyId { get; set; }
    }

    public class TradeNoteViewModel
    {
        public int Id { get; set; }
        public string Text { get; set;}
        
        public string UserId { get; set; }
    }

}