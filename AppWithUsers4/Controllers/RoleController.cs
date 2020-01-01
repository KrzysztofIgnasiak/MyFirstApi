using AppWithUsers4.Models;
using AppWithUsers4.Models.BindingModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AppWithUsers4.Controllers
{
    [Authorize(Roles = "admin")]
    [RoutePrefix("api/Role")]
    public class RoleController : ApiController
    {
        private ApplicationDbContext RoleContext = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public RoleController()
        {
        }

        public RoleController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
           // AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [Route("GetAll")]
        public IHttpActionResult GetAllRoles()
        {
            var Rolestore = new RoleStore<IdentityRole>(new ApplicationDbContext());
            var roleManager = new RoleManager<IdentityRole>(Rolestore);
            var roles = roleManager.Roles;

            return Ok(roles);
        }

        [Route("create")]
        public async Task<IHttpActionResult> Create(CreateRoleBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = new IdentityRole { Name = model.Name };

            var Rolestore = new RoleStore<IdentityRole>(new ApplicationDbContext());
            var roleManager = new RoleManager<IdentityRole>(Rolestore);

            var result = await roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                return BadRequest();
                //return GetErrorResult(result);
            }
            else
            {
                return Ok();
            }

        }
        [Route("AddUserToRole")]
        [HttpPost]
        public async Task<IHttpActionResult> AddUserToRole(string Id, string RoleName)
        {
            var User = RoleContext.Users.SingleOrDefault(u => u.Id == Id);
            if (User == null || User.IsDeleted == true)
            {
                return NotFound();
            }
            var Rolestore = new RoleStore<IdentityRole>(new ApplicationDbContext());
            var roleManager = new RoleManager<IdentityRole>(Rolestore);
            var Role = await roleManager.FindByNameAsync(RoleName);
            if (Role == null)
            {
                return NotFound();
            }
            await UserManager.AddToRoleAsync(User.Id, RoleName);
            RoleContext.SaveChanges();
            return Ok();
        }
        [Route("RemoveUserFromRole")]
        [HttpPost]
        public async Task<IHttpActionResult> RemoveUserFromRole(string Id, string RoleName)
        {
            var User = RoleContext.Users.SingleOrDefault(u => u.Id == Id);
            if (User == null || User.IsDeleted == true)
            {
                return NotFound();
            }
            var Rolestore = new RoleStore<IdentityRole>(new ApplicationDbContext());
            var roleManager = new RoleManager<IdentityRole>(Rolestore);
            var Role = await roleManager.FindByNameAsync(RoleName);
            if (Role == null)
            {
                return NotFound();
            }
            await UserManager.RemoveFromRoleAsync(User.Id, RoleName);
            RoleContext.SaveChanges();
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }
    }

}

