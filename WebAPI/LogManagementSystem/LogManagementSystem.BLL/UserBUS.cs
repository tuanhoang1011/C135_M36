using LogManagementSystem.DAO;
using System;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace LogManagementSystem.BLL
{
    public class UserBUS : IDisposable
    {
        private LogManagementSystemEntities db = new LogManagementSystemEntities();

        /// <summary>
        /// Check username and password when user login to system
        /// </summary>
        /// <param name="loginModel">Username and password of user who want to login to system</param>
        /// <returns>return 0 if username and password are valid, return 1 if password is invalid, return -1 if all are invalid</returns>
        public int CheckLogin(string userName, string password)
        {
            LogManagementSystemEntities dbContext = new LogManagementSystemEntities();
            var isExistUser = dbContext.Users.FirstOrDefault(x => x.UserName == userName && x.Password == password);
            if (isExistUser != null)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// The function used to get permission for roles.
        /// </summary>
        /// <param name="userName">userName</param>
        /// <returns>Return result in table roles.</returns>// xem ky
        public RoleModel GetPermission(string userName)
        {
            var r = db.Users.Join(
                db.Roles,
                tb1 => tb1.RoleId,
                tb2 => tb2.RoleId,
                (tb1, tb2) => new { tb1, tb2 })
                .FirstOrDefault(x => x.tb1.UserName == userName);
            var r1 = db.Logs.AsQueryable();
            RoleModel role = new RoleModel();
            {
                role.RoleName = r.tb2.RoleName;
                role.Permission = r.tb2.Permissions;
                role.UserID = r.tb1.UserId;
            }
            return role;
        }

        /// <summary>
        /// Get all the Users(userId, username, fullname, birthdate, gender,phone, address and RoleName)
        /// </summary>
        /// <returns>A List of all users in database who wasn't be deleted</returns>
        public IQueryable<XP_GetAllUsers_Result> GetAllUsers()
        {
            return db.XP_GetAllUsers().AsQueryable();
        }

        /// <summary>
        /// Get all info and user(uid) and all the logs
        /// </summary>
        /// <param name="uid">userId who want to get</param>
        /// <returns>A info of user who have this uid and all the logs of this user</returns>
        public ObjectResult<XP_GetUserDetails_Result> GetUserDetails(int id)
        {
            return db.XP_GetUserDetails(id);
        }

        /// <summary>
        /// Add a new user to database
        /// </summary>
        /// <param name="user">The one who you want to add to database</param>
        /// <returns>return false if system can't add this user to database, return true if can</returns>
        public bool AddUser(User user)
        {
            try
            {
                db.Users.Add(user);
                db.SaveChanges();//Apply changes to our database
                return true;
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

        /// <summary>
        /// Update information of a user
        /// </summary>
        /// <param name="user">the new user you want to change</param>
        /// <returns>return false if system can't update this user to database, return true if can</returns>
        public bool UpdateUser(User user)
        {
            try
            {
                User r = db.Users.FirstOrDefault(x => x.UserId == user.UserId);
                r.FullName = user.FullName;
                r.UserName = user.UserName;
                r.Birthdate = user.Birthdate;
                r.Gender = user.Gender;
                r.Phone = user.Phone;
                r.Address = user.Address;
                r.RoleId = user.RoleId;
                db.Entry(r).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return true;
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

        /// <summary>
        /// Remove that user if it doesn't appear in others table
        /// </summary>
        /// <param name="uid">UserId who you want to delete</param>
        /// <returns>return false if system can't update this user to database, return true if can</returns>
        public bool DeleteUser(int id)
        {
            try
            {
                db.XP_DeleteUser(id);
                return true;
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
        // ~UserBUS() {
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
