using LogManagementSystem.DAO;
using System;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace LogManagementSystem.BLL
{
    public class RoleBUS : IDisposable
    {
        private LogManagementSystemEntities db = new LogManagementSystemEntities();
        //private LogManagementSystemEntities db = null;
        //public RoleBUS() => db = new LogManagementSystemEntities();

        /// <summary>
        /// Get all info and role(rid)
        /// </summary>
        /// <returns>A List of all roles in database who wasn't be deleted</returns>
        public IQueryable<XP_GetAllRoles_Result> GetAllRoles()
        {
            return db.XP_GetAllRoles().AsQueryable();
        }

        /// <summary>
        /// Get info of select role(rid)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List all info of role detail</returns>
        public ObjectResult<XP_GetRoleDetails_Result> GetRoleDetails(int id)
        {
            return db.XP_GetRoleDetails(id);
        }

        /// <summary>
        /// Add a new user to database
        /// </summary>
        /// <param name="role">The one who you want to add to database</param>
        /// <returns>return false if system can't add this role to database, return true if can</returns>
        public bool AddRole(Role role)
        {
            try
            {
                db.Roles.Add(role);
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
        /// Update info of select role(rid)
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool UpdateRole(Role role)
        {
            try
            {
                var r = db.Roles.FirstOrDefault(x => x.RoleId == role.RoleId);
                r.Permissions = role.Permissions;
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
        /// Remove that role if it doesn't appear in others table
        /// </summary>
        /// <param name="rid">RoleId who you want to delete</param>
        /// <returns>return false if system can't update this role to database, return true if can</returns>
        public bool DeleteRole(int rid)
        {
            try
            {
                db.XP_DeleteRole(rid);
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
        // ~RoleBUS() {
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
