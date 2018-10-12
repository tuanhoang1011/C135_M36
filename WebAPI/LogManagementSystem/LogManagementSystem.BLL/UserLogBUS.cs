using LogManagementSystem.DAO;
using System;
using System.Linq;

namespace LogManagementSystem.BLL
{
    public class UserLogBUS : IDisposable
    {
        private LogManagementSystemEntities db = new LogManagementSystemEntities();
        /// <summary>
        /// The function get all user logs.
        /// </summary>
        /// <param name="logID">logID</param>
        /// <returns>List of user logs</returns>
        public IQueryable<XP_GetUserLogs_Result> GetAllUserLogs(int logID)
        {
            return db.XP_GetUserLogs(logID).AsQueryable();
        }
        /// <summary>
        /// The function update user logs into table UserLog.
        /// </summary>
        /// <param name="userID">userID</param>
        /// <param name="logID">logID</param>
        /// <returns>Return result update into table userlog.</returns>
        public bool UpdateUserLog(int userID, int logID)
        {
            try
            {
                UserLog ul = db.UserLogs.FirstOrDefault(x => x.LogId == logID && x.UserId == userID);
                if (ul != null)
                {
                    ul.ModifyDate = DateTime.Now;
                    db.Entry(ul).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    UserLog newUserLog = new UserLog();
                    {
                        newUserLog.LogId = logID;
                        newUserLog.UserId = userID;
                        db.UserLogs.Add(newUserLog);
                        db.SaveChanges();
                    }
                    return true;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                GC.Collect();
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~UserLogBUS() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            db.Dispose();
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
