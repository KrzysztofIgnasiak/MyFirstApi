using AppWithUsers4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppWithUsers4.Controllers
{
    [Authorize]
    [RoutePrefix("api/Industry")]

    // POST/api/Industry
    public class IndustryController : ApiController
    {
        private ApplicationDbContext IndustryContext = new ApplicationDbContext();
        [HttpPost]
        public IHttpActionResult CreateIndustry([FromBody]Industry Industry)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("invalid data.");
            }
            if (Industry == null)
            {
                return BadRequest("any information about Industry was specified");
            }

            else
            {
                Industry NewIndustry = new Industry();
                NewIndustry.Name = Industry.Name;

                IndustryContext.Industries.Add(NewIndustry);
                IndustryContext.SaveChanges();

                return Ok();
            }
        }
    }
}
