using LogManagementSystem.DAO;
using System.Collections.Generic;
using System.Linq;

namespace LogManagementSystem.BLL
{
    public class LogBUS
    {
        private LogManagementSystemEntities db = new LogManagementSystemEntities();

        /// <summary>
        /// Get all logs in database
        /// </summary>
        /// <returns>Return all logs in database</returns>
        public IQueryable<XP_GetAllLogs_Result> GetAllLog()
        {
            return db.XP_GetAllLogs().AsQueryable();
        }

        /// <summary>
        /// Get the details of a log in database
        /// </summary>
        /// <param name="lid">Log's id you want to get details information</param>
        /// <returns>Return the details of a log which have the same log Id you had input</returns>
        public IQueryable<XP_GetLogDetails_Result> GetLogDetails(int lid)
        {
            return db.XP_GetLogDetails(lid).AsQueryable();
        }

        /// <summary>
        /// Get log overview
        /// </summary>
        /// <returns>Return 10 logs and with the name who logged it</returns>
        public IQueryable<XP_GetLogOverview_Result> GetLogOverView()
        {
            return db.XP_GetLogOverview().AsQueryable();
        }

        /// <summary>
        /// Get count daily logs in week
        /// </summary>
        /// <returns></returns>
        public IQueryable<XP_CountWeek_Result> GetCountWeek(){
            return db.XP_CountWeek().AsQueryable();
        }

        /// <summary>
        /// Get the result of logs which have similar with datasearch
        /// </summary>
        /// <param name="dataSearch">The string you want to find in your logs</param>
        /// <returns>Return a list all of logs which have similar with datasearch</returns>
        public IEnumerable<Log> SearchLogs(string dataSearch)
        {
            string dataSearchUnsign = CommonFunction.ConvertToUnSign(dataSearch);//Convert dataSearch to unsigned
            var dataLogs = db.Logs;//Get all logs in database
            List<Log> logs = new List<Log>();//Create a list of logs to contain the result

            foreach(var item in dataLogs)
            {
                //Check if log name contains dataSearch
                if (CommonFunction.ConvertToUnSign(item.LogName).Contains(dataSearchUnsign))
                {
                    logs.Add(item);
                }
            }

            return logs;
        }

        /// <summary>
        /// Add a log to database
        /// </summary>
        /// <param name="userID">The user id who created this log</param>
        /// <param name="log">A log you want to add to database</param>
        /// <returns>Return true if success, false if failed</returns>
        public bool AddLog(int userID, Log log)
        {
            try
            {
                UserLog ul = new UserLog();
                ul.LogId = log.LogId;
                ul.UserId = userID;
                log.UserLogs.Add(ul);

                db.Logs.Add(log);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Update a log in database
        /// </summary>
        /// <param name="userID">The id of user who updated this log</param>
        /// <param name="log">The log you want to update (warning: The LogId must be same)</param>
        /// <returns>Return true if success, false if failed</returns>
        public bool UpdateLog(int userID, Log log)
        {
            try
            {
                Log l = db.Logs.FirstOrDefault(x => x.LogId == log.LogId);
                if (l != null)
                {
                    l.LogName = log.LogName;
                    l.LogType = log.LogType;
                    l.Status = log.Status;
                    l.Description = log.Description;

                    db.Entry(l).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Delete a log in database
        /// </summary>
        /// <param name="lid">Id of the log you want to delete</param>
        /// <returns>Return true if success, false if failed</returns>
        public bool DeleteLog(int lid)
        {
            try
            {
                db.XP_DeleteLog(lid);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get the total of users, logs and roles
        /// </summary>
        /// <returns>Return the total of users, logs, roles</returns>
        public IQueryable<XP_CountTotal_Result> CountTotal()
        {
            return db.XP_CountTotal().AsQueryable();
        }
    }
}
