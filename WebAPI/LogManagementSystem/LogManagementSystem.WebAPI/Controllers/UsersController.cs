using LogManagementSystem.BLL;
using LogManagementSystem.DAO;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web.Http;

namespace LogManagementSystem.WebAPI.Controllers
{
    public class UsersController : ApiController
    {
        UserBUS bus = new UserBUS();

        /// <summary>
        /// The function get all info of Users.
        /// </summary>
        /// <returns>List of Users</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [AuthorizePermission(Permission = "1,3,5,7")]
        public IEnumerable<XP_GetAllUsers_Result> GetAllUsers()
        {
            return bus.GetAllUsers();
        }

        /// <summary>
        /// The function get detail select User.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Detail of select User</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [AuthorizePermission(Permission = "1,3,5,7")]
        public ObjectResult<XP_GetUserDetails_Result> GetUserDetails(int id)
        {
            return bus.GetUserDetails(id);
        }

        /// <summary>
        /// Add user info table User.
        /// </summary>
        /// <param name="user">user</param>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [AuthorizePermission(Permission = "2,3,6,7")]
        public void AddUser(User user)
        {
            bus.AddUser(user);
        }

        /// <summary>
        /// Edit select info User.
        /// </summary>
        /// <param name="user">user</param>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [AuthorizePermission(Permission = "4,5,6,7")]
        public void UpdateUser(User user)
        {
            bus.UpdateUser(user);
        }

        /// <summary>
        /// Remove info User out of table User.
        /// </summary>
        /// <param name="id">id</param>
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [AuthorizePermission(Permission = "4,5,6,7")]
        public void DeleteUser(int id)
        {
            bus.DeleteUser(id);
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <param name="s">How many item you want to skip?</param>
        /// <param name="t">How many item you want to take?</param>
        /// <returns>Return all users within skip and take</returns>
        [HttpGet]
        [Route("api/Users/GetAllUsers/{s}/{t}")]
        public IEnumerable<XP_GetAllUsers_Result> GetAllUsers(int s, int t)
        {
            return bus.GetAllUsers().Skip(s).Take(t).ToList();
        }
    }
}
