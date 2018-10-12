using LogManagementSystem.BLL;
using LogManagementSystem.DAO;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web.Http;

namespace LogManagementSystem.WebAPI.Controllers
{
    public class RolesController : ApiController
    {
        RoleBUS bus = new RoleBUS();
        /// <summary>
        /// The function get all roles.
        /// </summary>
        /// <returns>List of roles.</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [AuthorizePermission(Permission = "1,3,5,7")]
        public IEnumerable<XP_GetAllRoles_Result> GetAllRoles()
        {
            return bus.GetAllRoles().ToList();
        }

        /// <summary>
        /// The function get detail of select role.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Detail of select role.</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [AuthorizePermission(Permission = "1,3,5,7")]
        public ObjectResult<XP_GetRoleDetails_Result> GetRoleDetails(int id)
        {
            return bus.GetRoleDetails(id);
        }

        /// <summary>
        /// Add new role into table role.
        /// </summary>
        /// <param name="role">role</param>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [AuthorizePermission(Permission = "2,3,6,7")]
        public void AddRole(Role role)
        {
            bus.AddRole(role);
        }

        /// <summary>
        /// Update role into table role.
        /// </summary>
        /// <param name="role">role</param>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [AuthorizePermission(Permission = "4,5,6,7")]
        public void UpdateRole(Role role)
        {
            bus.UpdateRole(role);
        }

        /// <summary>
        /// Remove info role out of table role.
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [AuthorizePermission(Permission = "4,5,6,7")]
        public void DeleteRole(int id)
        {
            bus.DeleteRole(id);
        }

        /// <summary>
        /// Get all roles
        /// </summary>
        /// <param name="s">How many item you want to skip?</param>
        /// <param name="t">How many item you want to take?</param>
        /// <returns>Return all roles within skip and take</returns>
        [HttpGet]
        [Route("api/Roles/GetAllRoles/{s}/{t}")]
        public IEnumerable<XP_GetAllRoles_Result> GetAllRoles(int s, int t)
        {
            return bus.GetAllRoles().Skip(s).Take(t).ToList();
        }
    }
}
