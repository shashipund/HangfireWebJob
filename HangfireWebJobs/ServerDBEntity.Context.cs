﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HangfireWebJobs
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class LT_SERVER_DBEntities : DbContext
    {
        public LT_SERVER_DBEntities()
            : base("name=LT_SERVER_DBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<Priority> Priorities { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TablePriority> TablePriorities { get; set; }
        public virtual DbSet<TestBenchDetail> TestBenchDetails { get; set; }
        public virtual DbSet<User> Users { get; set; }
    
        public virtual int CreateTable(string query, string tableName)
        {
            var queryParameter = query != null ?
                new ObjectParameter("query", query) :
                new ObjectParameter("query", typeof(string));
    
            var tableNameParameter = tableName != null ?
                new ObjectParameter("tableName", tableName) :
                new ObjectParameter("tableName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CreateTable", queryParameter, tableNameParameter);
        }
    }
}
