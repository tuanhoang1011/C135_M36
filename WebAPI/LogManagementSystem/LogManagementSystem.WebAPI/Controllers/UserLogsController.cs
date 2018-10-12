using LogManagementSystem.BLL;
using LogManagementSystem.DAO;
using System.Collections.Generic;
using System.Web.Http;


namespace LogManagementSystem.WebAPI.Controllers
{
    public class UserLogsController : ApiController
    {
        UserLogBUS bus = new UserLogBUS();

        /// <summary>
        /// The function get all info of user logs.
        /// </summary>
        /// <param name="logID">logID</param>
        /// <returns>List of user logs.</returns>
        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        [AuthorizePermission(Permission = "1,3,5,7")]
        [Route("api/UserLogs/{logID}")]
        public IEnumerable<XP_GetUserLogs_Result> GetAllUserLogs(int logID)
        {
            return bus.GetAllUserLogs(logID);
        }
    }
}
