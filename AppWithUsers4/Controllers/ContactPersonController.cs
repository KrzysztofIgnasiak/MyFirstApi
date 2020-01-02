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
    [RoutePrefix("api/ContactPerson")]
    public class ContactPersonController : ApiController
    {
        private ApplicationDbContext ContactPersonContext = new ApplicationDbContext();

        //Get /api/ ContactPerson
        [HttpGet]
        public IHttpActionResult GetContactPeople()
        {
            List<ContactPerson> ContactPeople = ContactPersonContext.ContactPeople.
                Where(c => c.isDeleted ==false)
                .Include(c => c.CompanyId)
                .Include(c => c.UserId)
                .ToList();

            List<ContactPersonGetBindingModel> Models = new List<ContactPersonGetBindingModel>();

            foreach(ContactPerson Person in ContactPeople)
            {
                ContactPersonGetBindingModel Model = new ContactPersonGetBindingModel();
                Model.Id = Person.Id;
                Model.Name = Person.Name;
                Model.Surname = Person.Surname;
                Model.Phone = Person.Phone;
                Model.Mail = Person.Mail;
                Model.Position = Person.Position;

                Model.CompanyId = Person.CompanyId.Id;
                Model.UserId = Person.UserId.Id;
                
                Models.Add(Model);
            }
            return Ok(Models);
        }

        //Get /api/ ContactPerson/BySurname
        [HttpGet]
        [Route("BySurname")]
        public IHttpActionResult GetContactPeopleBySurname(string Surname)
        {
            List<ContactPerson> ContactPeople = ContactPersonContext.ContactPeople.
                Where(c => c.isDeleted == false && c.Surname == Surname)
                .Include(c => c.CompanyId)
                .Include(c => c.UserId)
                .ToList();

            List<ContactPersonGetBindingModel> Models = new List<ContactPersonGetBindingModel>();

            foreach (ContactPerson Person in ContactPeople)
            {
                ContactPersonGetBindingModel Model = new ContactPersonGetBindingModel();
                Model.Id = Person.Id;
                Model.Name = Person.Name;
                Model.Surname = Person.Surname;
                Model.Phone = Person.Phone;
                Model.Mail = Person.Mail;
                Model.Position = Person.Position;

                Model.CompanyId = Person.CompanyId.Id;
                Model.UserId = Person.UserId.Id;

                Models.Add(Model);
            }
            return Ok(Models);
        }

        // GET /api/ContactPerson/1
        [HttpGet]
        public IHttpActionResult GetContactPerson(int Id)
        {

            if (ModelState.IsValid == false)
            {
                return BadRequest("invalid data.");
            }

            ContactPerson Person = ContactPersonContext.ContactPeople.SingleOrDefault(c => c.Id == Id);
            if(Person == null || Person.isDeleted == true)
            {
                return NotFound();
            }
            else
            {
                ContactPersonContext.Entry(Person).Reference(c => c.CompanyId).Load();
                ContactPersonContext.Entry(Person).Reference(c => c.UserId).Load();
                ContactPersonGetBindingModel Model = new ContactPersonGetBindingModel();
                Model.Id = Person.Id;
                Model.Name = Person.Name;
                Model.Surname = Person.Surname;
                Model.Phone = Person.Phone;
                Model.Mail = Person.Mail;
                Model.Position = Person.Position;

                Model.CompanyId = Person.CompanyId.Id;
                Model.UserId = Person.UserId.Id;
                //Company CurrentCompany = Person.CompanyId;
                //Model.CompanyId = CurrentCompany.Id;

              //  ApplicationUser User = Person.UserId;

          

                return Ok(Model);
                
            }
        }
        // POST/api/ContactPerson
        [HttpPost]
        public IHttpActionResult CreateContactPerson([FromBody]ContactPersonBindingModel Model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("invalid data.");
            }
            if (Model == null)
            {
                return BadRequest("any information about ContactPerson was specified");
            }
            if (Model.CompanyId == null)
            {
                return BadRequest("You have to specivied the id of company");
            }
            else
            {
                var Company = ContactPersonContext.Companies.SingleOrDefault(c => c.Id == Model.CompanyId);

                if (Company == null)
                {
                    return BadRequest("The is no Company with given id");
                }
                else
                {
                    ContactPerson NewContactPerson = new ContactPerson();
                    NewContactPerson.Name = Model.Name;
                    NewContactPerson.Surname = Model.Surname;
                    NewContactPerson.Phone = Model.Phone;
                    NewContactPerson.Mail = Model.Mail;
                    NewContactPerson.Position = Model.Position;

                    NewContactPerson.CompanyId = Company;

                    string UserId = User.Identity.GetUserId();
                    ApplicationUser AppUser = ContactPersonContext.Users.Single(u => u.Id == UserId);
                    NewContactPerson.UserId = AppUser;

                    ContactPersonContext.ContactPeople.Add(NewContactPerson);
                    ContactPersonContext.SaveChanges();

                    return Ok();
                }
            }
        }

        // PUT /api/ContactPerson/1
        [HttpPut]
        public IHttpActionResult UpdateContactPerson(int Id, ContactPersonBindingModel Model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("invalid data.");
            }
            if (Model == null)
            {
                return BadRequest("any information about ContactPerson was specified");
            }
            else
            {
                ContactPerson UpdatePerson = ContactPersonContext.ContactPeople.SingleOrDefault(c => c.Id == Id);
                if(UpdatePerson == null)
                {
                    return BadRequest("The Person with this id does not exist");
                }
                if(Model.CompanyId != null)
                {
                    Company ChangedCompany = ContactPersonContext.Companies.SingleOrDefault(c => c.Id == Id);
                    if(ChangedCompany == null)
                    {
                        return BadRequest("The Company with given Id does not exist");
                    }
                    else
                    {
                        UpdatePerson.CompanyId = ChangedCompany;
                    }
                    UpdatePerson.Name = Model.Name ?? UpdatePerson.Name;
                    UpdatePerson.Surname = Model.Surname ?? UpdatePerson.Surname;
                    UpdatePerson.Phone = Model.Phone ?? UpdatePerson.Phone;
                    UpdatePerson.Mail = Model.Mail ?? UpdatePerson.Mail;
                    UpdatePerson.Position = Model.Position ?? UpdatePerson.Position;

                    ContactPersonContext.SaveChanges();

                }
                return Ok();
            }
        }

        //DELETE /api/ContactPerson/1
        [HttpDelete]

        public IHttpActionResult DeleteContactPerson(int Id)
        {
            ContactPerson DeletePerson = ContactPersonContext.ContactPeople.SingleOrDefault(c => c.Id == Id);
            if(DeletePerson == null || DeletePerson.isDeleted ==true)
            {
                return NotFound();

            }
            else
            {
                DeletePerson.isDeleted = true;
                ContactPersonContext.SaveChanges();
                return Ok();
            }
        }
    }
}
