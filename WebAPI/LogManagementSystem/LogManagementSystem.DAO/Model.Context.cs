﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LogManagementSystem.DAO
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class LogManagementSystemEntities : DbContext
    {
        public LogManagementSystemEntities()
            : base("name=LogManagementSystemEntities")
        {
            base.Configuration.ProxyCreationEnabled = false;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserLog> UserLogs { get; set; }
    
        public virtual int XP_DeleteLog(Nullable<int> logID)
        {
            var logIDParameter = logID.HasValue ?
                new ObjectParameter("LogID", logID) :
                new ObjectParameter("LogID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("XP_DeleteLog", logIDParameter);
        }
    
        public virtual int XP_DeleteRole(Nullable<int> roleID)
        {
            var roleIDParameter = roleID.HasValue ?
                new ObjectParameter("RoleID", roleID) :
                new ObjectParameter("RoleID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("XP_DeleteRole", roleIDParameter);
        }
    
        public virtual int XP_DeleteUser(Nullable<int> userID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("XP_DeleteUser", userIDParameter);
        }
    
        public virtual ObjectResult<XP_GetAllLogs_Result> XP_GetAllLogs()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<XP_GetAllLogs_Result>("XP_GetAllLogs");
        }
    
        public virtual ObjectResult<XP_GetAllUsers_Result> XP_GetAllUsers()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<XP_GetAllUsers_Result>("XP_GetAllUsers");
        }
    
        public virtual ObjectResult<XP_GetLogDetails_Result> XP_GetLogDetails(Nullable<int> lid)
        {
            var lidParameter = lid.HasValue ?
                new ObjectParameter("lid", lid) :
                new ObjectParameter("lid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<XP_GetLogDetails_Result>("XP_GetLogDetails", lidParameter);
        }
    
        public virtual ObjectResult<XP_GetLogOverview_Result> XP_GetLogOverview()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<XP_GetLogOverview_Result>("XP_GetLogOverview");
        }
    
        public virtual ObjectResult<XP_GetRoleDetails_Result> XP_GetRoleDetails(Nullable<int> rid)
        {
            var ridParameter = rid.HasValue ?
                new ObjectParameter("rid", rid) :
                new ObjectParameter("rid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<XP_GetRoleDetails_Result>("XP_GetRoleDetails", ridParameter);
        }
    
        public virtual ObjectResult<XP_GetUserDetails_Result> XP_GetUserDetails(Nullable<int> uid)
        {
            var uidParameter = uid.HasValue ?
                new ObjectParameter("uid", uid) :
                new ObjectParameter("uid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<XP_GetUserDetails_Result>("XP_GetUserDetails", uidParameter);
        }
    
        public virtual ObjectResult<XP_GetUserLogs_Result> XP_GetUserLogs(Nullable<int> lid)
        {
            var lidParameter = lid.HasValue ?
                new ObjectParameter("lid", lid) :
                new ObjectParameter("lid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<XP_GetUserLogs_Result>("XP_GetUserLogs", lidParameter);
        }
    
        public virtual ObjectResult<XP_GetAllRoles_Result> XP_GetAllRoles()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<XP_GetAllRoles_Result>("XP_GetAllRoles");
        }
    
        public virtual ObjectResult<Nullable<int>> XP_CountDay(Nullable<System.DateTime> logDay)
        {
            var logDayParameter = logDay.HasValue ?
                new ObjectParameter("LogDay", logDay) :
                new ObjectParameter("LogDay", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("XP_CountDay", logDayParameter);
        }
    
        public virtual ObjectResult<XP_CountWeek_Result> XP_CountWeek()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<XP_CountWeek_Result>("XP_CountWeek");
        }
    
        public virtual ObjectResult<XP_CountTotal_Result> XP_CountTotal()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<XP_CountTotal_Result>("XP_CountTotal");
        }
    }
}
