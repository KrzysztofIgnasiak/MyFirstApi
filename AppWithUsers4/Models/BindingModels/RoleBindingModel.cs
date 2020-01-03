using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWithUsers4.Models.BindingModels
{
    public class IsAdminBindingModel
    {
        public bool IsAdmin { get; set; }
    }
   public class CreateRoleBindingModel
    {
        public string Name { get; set; }

    }
}