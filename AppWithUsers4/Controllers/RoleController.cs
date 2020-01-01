using AppWithUsers4.Models;
using AppWithUsers4.Models.BindingModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
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
        private ApplicationUserManager _userManager;
        private ApplicationDbContext RoleContext = new ApplicationDbContext();

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

        [Route("user/{id:guid}/roles")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] string id, [FromBody] string[] rolesToAssign)
        {

            var appUser = await UserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }
            var Rolestore = new RoleStore<IdentityRole>(new ApplicationDbContext());
            var roleManager = new RoleManager<IdentityRole>(Rolestore);

            var currentRoles = await UserManager.GetRolesAsync(appUser.Id);

            var rolesNotExists = rolesToAssign.Except(roleManager.Roles.Select(x => x.Name)).ToArray();

            if (rolesNotExists.Count() > 0)
            {

                ModelState.AddModelError("", string.Format("Roles '{0}' does not exixts in the system", string.Join(",", rolesNotExists)));
                return BadRequest(ModelState);
            }

            IdentityResult removeResult = await UserManager.RemoveFromRolesAsync(appUser.Id, currentRoles.ToArray());

            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return BadRequest(ModelState);
            }

            IdentityResult addResult = await UserManager.AddToRolesAsync(appUser.Id, rolesToAssign);

            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user roles");
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}

