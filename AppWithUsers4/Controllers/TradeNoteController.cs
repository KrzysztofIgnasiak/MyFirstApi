using AppWithUsers4.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppWithUsers4.Controllers
{
    [Authorize]
    [RoutePrefix("api/TradeNote")]
    public class TradeNoteController : ApiController
    {
        private ApplicationDbContext TradeNoteContext = new ApplicationDbContext();

        // GET /api/TradeNote/1
        [HttpGet]
        public IHttpActionResult GetTradeNotes(int Id)
        {
            var Company = TradeNoteContext.Companies.SingleOrDefault(c => c.Id == Id);

            if (Company == null || Company.IsDeleted == true)
            {
                return NotFound();
            }
            else
            {
                List<TradeNote> TradeNotes = TradeNoteContext.TradeNotes.Where(t => t.CompanyId.Id == Company.Id).Include(t => t.UserId).ToList();
                List<TradeNoteViewModel> Models = new List<TradeNoteViewModel>();
                foreach(TradeNote Note in TradeNotes)
                {
                    TradeNoteViewModel Model = new TradeNoteViewModel();
                    Model.Id = Note.id;
                    Model.Text = Note.Text;
                    ApplicationUser userID = Note.UserId;
                    Model.UserId = userID.Id.ToString();

                    Models.Add(Model);
                }
                return Ok(Models);
            }
        }

        // POST/api/TradeNote
        [HttpPost]
        public IHttpActionResult CreateNote([FromBody]TradeNoteAddBindingModel Model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("invalid data.");
            }
            if (Model == null)
            {
                return BadRequest("any information about Note was specified");
            }
            else
            {
                Company Company = TradeNoteContext.Companies.SingleOrDefault(c => c.Id == Model.CompanyId);
                if(Company == null || Company.IsDeleted == true)
                {
                    return BadRequest("The company with this request doesn't exist");
                }

                TradeNote NewNote = new TradeNote();
                NewNote.Text = Model.Text;
                NewNote.CompanyId = Company;

                string UserId = User.Identity.GetUserId();
                ApplicationUser AppUser = TradeNoteContext.Users.Single(u => u.Id == UserId);
                NewNote.UserId = AppUser;
                TradeNoteContext.TradeNotes.Add(NewNote);
                TradeNoteContext.SaveChanges();
                return Ok(NewNote);
            }
        }
    }
}
