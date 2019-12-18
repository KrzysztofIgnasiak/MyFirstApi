using AppWithUsers4.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Security.Cryptography;
namespace AppWithUsers4.Controllers
{

    [Authorize]
    [RoutePrefix("api/Company")]
    public class CompanyController : ApiController
    {
        private ApplicationDbContext CompanyContext = new ApplicationDbContext();
        
        // GET/api/Company
        [HttpGet]
        public IHttpActionResult GetCompanies([FromUri]PagingParameterModel paging)
        {
            if (paging.PageNumber <= 0)
            {
                return BadRequest();
            }
            else
            {
                var Companies = CompanyContext.Companies.Where(c => c.IsDeleted == false)
                    .OrderBy(u => u.Name)
                    .AsQueryable()
                    .Skip((paging.PageNumber - 1) * paging.PageSize)
                    .Take(paging.PageSize)
                    .Include(c => c.userID)
                    .Include(c => c.IndustryType)
                    .ToList();
                     //.Include(c => c.IndustryType)


                List<CompanyViewPublicModel> Models = new List<CompanyViewPublicModel>();
                foreach(Company Company in Companies)
                { 
                        CompanyViewPublicModel Model = new CompanyViewPublicModel();
                        Model.Name = Company.Name;
                        Model.Nip = Company.Nip;
                        Model.IndustryType = Company.IndustryType;
                        Model.Address = Company.Address;
                        Model.City = Company.City;
                        ApplicationUser userID = Company.userID;
                        Model.userID = userID.Id.ToString();

                        Models.Add(Model);
                }

                return Ok(Models);
            }
        }

        // GET /api/Company/1
        [HttpGet]
        public IHttpActionResult GetCompany(int Id)
        {
            var Company = CompanyContext.Companies.Include(c=>c.userID)
                .Include(c => c.IndustryType)
                .SingleOrDefault(c => c.Id == Id);
            

            if (Company == null || Company.IsDeleted == true)
            {
                return NotFound();
            }

            else
            { 
                CompanyViewPublicModel Model = new CompanyViewPublicModel();
                Model.Name = Company.Name;
                Model.Nip = Company.Nip;
                Model.IndustryType = Company.IndustryType;
                Model.Address = Company.Address;
                Model.City = Company.City;
                ApplicationUser userID = Company.userID;
                Model.userID = userID.Id.ToString();
 
                return Ok(Model);
            }
            
        }

        // POST/api/Company
        [HttpPost]
        public IHttpActionResult CreateCompany([FromBody]CompanyAddBindingModel Model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("invalid data.");
            }
            if (Model == null)
            {
                return BadRequest("any information about company was specified");
            }
            else
            {
                Company NewCompany = new Company();
                NewCompany.Name = Model.Name;
                NewCompany.Address = Model.Address;
                NewCompany.City = Model.City;
                NewCompany.Nip = Model.Nip;
                
                if(Model.IndustryId == null)
                {
                    NewCompany.IndustryType = null;
                }
                else
                {
                    Industry CurrentIndustry = CompanyContext.Industries.SingleOrDefault(i => i.Id == Model.IndustryId);

                    NewCompany.IndustryType = CurrentIndustry;
                }

                string UserId = User.Identity.GetUserId();
                ApplicationUser AppUser = CompanyContext.Users.Single(u => u.Id == UserId); 
                NewCompany.userID = AppUser;

                CompanyContext.Companies.Add(NewCompany);
                CompanyContext.SaveChanges();
                return Ok();
            }
        }

        // PUT /api/Company/1
        [HttpPut]
        public IHttpActionResult UpdateCompany(int Id, CompanyUpdateBindingModel Model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("invalid data.");
            }

            var Company = CompanyContext.Companies.SingleOrDefault(c => c.Id == Id);

            if (Company == null || Company.IsDeleted == true)
            {
                return NotFound();
            }

            else
            {
                Company.Name = Model.Name ?? Company.Name;
                Company.Nip = Model.Nip ?? Company.Nip;
                //Company.IndustryType = Model.IndustryType ?? Company.IndustryType;
                Company.Address = Model.Address?? Company.Address;
                Company.City = Model.City ?? Company.City;

                 if(Model.IndustryId != null)
                {
                    Company.IndustryType = CompanyContext.Industries.Single(i => i.Id == Model.IndustryId);
                }
                CompanyContext.SaveChanges();
                return Ok();
            }

        }

        //DELETE /api/Company/1
        [HttpDelete]

        public IHttpActionResult DeleteCompany(int Id)
        {
            Company CompanyToDelete = CompanyContext.Companies.SingleOrDefault(c => c.Id == Id);

            if (CompanyToDelete == null || CompanyToDelete.IsDeleted == true)
            {
                return NotFound();
            }
            CompanyContext.Companies.Single(c => c.Id == Id).IsDeleted = true;
            CompanyContext.SaveChanges();
            return Ok();
        }
    }
}
