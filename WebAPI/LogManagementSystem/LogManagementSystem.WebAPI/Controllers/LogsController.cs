using LogManagementSystem.BLL;
using LogManagementSystem.DAO;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Linq;

namespace LogManagementSystem.WebAPI.Controllers
{
    //[EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class LogsController : ApiController
    {
        private LogBUS logBUS = new LogBUS();
        private UserLogBUS userLogBUS = new UserLogBUS();

        [HttpGet]
        [Route("api/Logs")]
        [Authorize(Roles = "Admin")]
        [AuthorizePermission(Permission = "1,3,5,7")]
        public IEnumerable<XP_GetAllLogs_Result> GetAllLog()
        {
            return logBUS.GetAllLog().ToList();
        }

        /// <summary>
        /// Get All Logs For Search
        /// </summary>
        /// <returns>Return all logs name for search in database</returns>
        [HttpGet]
        [Route("api/Logs/GetAllLogForSearch")]
        [Authorize(Roles = "Admin, User")]
        [AuthorizePermission(Permission = "1,3,5,7")]
        public IEnumerable<string> GetAllLogForSearch()
        {
            return logBUS.GetAllLog().Select(x => x.LogName).ToList();
        }

        /// <summary>
        /// Get all logs
        /// </summary>
        /// <param name="s">How many item you want to skip?</param>
        /// <param name="t">How many item you want to take?</param>
        /// <returns>Return all logs within skip and take</returns>
        [HttpGet]
        [Route("api/Logs/GetAllDataForPagination/{s}/{t}")]
        [Authorize(Roles = "Admin, User")]
        [AuthorizePermission(Permission = "1,3,5,7")]
        public IEnumerable<XP_GetAllLogs_Result> GetAllLogForPagination(int s, int t)
        {
            return logBUS.GetAllLog().OrderByDescending(x => x.CreateDate).Skip(s).Take(t).ToList();
        }

        /// <summary>
        /// Get logs details
        /// </summary>
        /// <param name="id">The id of a log you want to get information</param>
        /// <returns>Return details of a log</returns>
        [HttpGet]
        [Route("api/Logs/{id}")]
        [Authorize(Roles = "Admin, User")]
        [AuthorizePermission(Permission = "1,3,5,7")]
        public IEnumerable<XP_GetLogDetails_Result> GetLogDetails(int id)
        {
            return logBUS.GetLogDetails(id).ToList();
        }

        /// <summary>
        /// Get logs when you search 
        /// </summary>
        /// <param name="dataSearch">The search data you want to find in logs</param>
        /// <returns>Return logs which have the smiliar name</returns>
        [HttpGet]
        [Route("api/Logs/Search/{skipItems}/{takeItems}")]
        [Authorize(Roles = "Admin, User")]
        [AuthorizePermission(Permission = "1,3,5,7")]
        public IEnumerable<Log> SearchLogs(string dataSearch, int skipItems, int takeItems)
        {
            return logBUS.SearchLogs(dataSearch).OrderByDescending(x => x.CreateDate).Skip(skipItems).Take(takeItems).ToList();
        }

        /// <summary>
        /// Get logs when you search 
        /// </summary>
        /// <param name="dataSearch">The search data you want to find in logs</param>
        /// <returns>Return logs which have the smiliar name</returns>
        [HttpGet]
        [Route("api/Logs/CountResultSearch")]
        [Authorize(Roles = "Admin, User")]
        [AuthorizePermission(Permission = "1,3,5,7")]
        public int GetCountResultSearchLog(string dataSearch)
        {
            return logBUS.SearchLogs(dataSearch).Count();
        }

        /// <summary>
        /// Add a log to database
        /// </summary>
        /// <param name="userID">The id of user who add this log to database</param>
        /// <param name="log">The log you want to add to database</param>
        [HttpPost]
        [Route("api/Logs/{userID}")]
        [Authorize(Roles = "Admin, User")]
        [AuthorizePermission(Permission = "2,3,6,7")]
        public void AddLog(int userID, Log log)
        {
            logBUS.AddLog(userID, log);
        }
        
        /// <summary>
        /// Edit a log
        /// </summary>
        /// <param name="userID">The id of user who update this log</param>
        /// <param name="log">The log you want to edit (warning: logId must be same)</param>
        [HttpPut]
        [Route("api/Logs/{userID}")]
        [Authorize(Roles = "Admin, User")]
        [AuthorizePermission(Permission = "4,5,6,7")]
        public void UpdateLog(int userID, Log log)
        {
            logBUS.UpdateLog(userID, log);
            userLogBUS.UpdateUserLog(userID, log.LogId);
        }

        /// <summary>
        /// Delete a log
        /// </summary>
        /// <param name="id">The id of log which one you want to delete</param>
        [HttpDelete]
        [Route("api/Logs/{id}")]
        [Authorize(Roles = "Admin, User")]
        [AuthorizePermission(Permission = "4,5,6,7")]
        public void DeleteLog(int id)
        {
            logBUS.DeleteLog(id);
        }

        /// <summary>
        /// Get total of users, logs and roles
        /// </summary>
        /// <returns>Return total of users, logs and roles</returns>
        [HttpGet]
        [Route("api/Logs/GetCountTotal")]
        [Authorize(Roles = "Admin, User")]
        [AuthorizePermission(Permission = "1,3,5,7")]
        public IEnumerable<XP_CountTotal_Result> GetCountTotal()
        {
            return logBUS.CountTotal().ToList();
        }

        /// <summary>
        /// Get the overview of logs (except Description and all users who logged the log)
        /// </summary>
        /// <returns>Top 10 logs in database</returns>
        [HttpGet]
        [Route("api/Logs/GetLogOverview")]
        [Authorize(Roles = "Admin")]
        public IEnumerable<XP_GetLogOverview_Result> GetLogOverview()
        {
            return logBUS.GetLogOverView().ToList();
        }

        /// <summary>
        /// Get the overview of logs (except Description and all users who logged the log)
        /// </summary>
        /// <returns>Top 10 logs in database</returns>
        [HttpGet]
        [Route("api/Logs/GetCountWeek")]
        [Authorize(Roles = "Admin")]
        public IEnumerable<XP_CountWeek_Result> GetCountWeek()
        {
            return logBUS.GetCountWeek().ToList();
        }
    }
}