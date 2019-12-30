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
                List<TradeNote> TradeNotes = TradeNoteContext.TradeNotes
                    .Where(t => t.CompanyId.Id == Company.Id && t.IsDeleted==false)
                    .Include(t => t.UserId)
                    .ToList();
                List<TradeNoteViewBindingModelModel> Models = new List<TradeNoteViewBindingModelModel>();
                foreach (TradeNote Note in TradeNotes)
                {
                    TradeNoteViewBindingModelModel Model = new TradeNoteViewBindingModelModel();
                    Model.Id = Note.Id;
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
                if (Company == null || Company.IsDeleted == true)
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

        [HttpPut]
        // PUT/api/TradeNote/1
        public IHttpActionResult UpdateNote(int Id, [FromBody]TradeNoteUpdateBindingModel Model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("invalid data.");
            }
            if (Model == null)
            {
                return BadRequest("no information about note was specified");
            }

            TradeNote Note = TradeNoteContext.TradeNotes.SingleOrDefault(c => c.Id == Id);
            if (Note == null)
            {
                return BadRequest("note with this id does not exist");
            }
            Company Company = TradeNoteContext.Companies.SingleOrDefault(c => c.Id == Model.CompanyId);
            if (Company.IsDeleted == true)
            {
                Company = null;
            }
           Note.Text = Model.Text?? Note.Text;
           Note.CompanyId = Company ?? Note.CompanyId;
           TradeNoteContext.SaveChanges();
            return Ok();
            
        }

        //DELETE /api/TradeNote/1
        [HttpDelete]

        public IHttpActionResult DeleteTradeNote(int Id)
        {
            TradeNote NoteToDelete = TradeNoteContext.TradeNotes.SingleOrDefault(t => t.Id == Id);

            if (NoteToDelete == null || NoteToDelete.IsDeleted == true)
            {
                return NotFound();
            }
            TradeNoteContext.TradeNotes.Single(t => t.Id == Id).IsDeleted = true;
            TradeNoteContext.SaveChanges();
            return Ok();
        }


    }
}
