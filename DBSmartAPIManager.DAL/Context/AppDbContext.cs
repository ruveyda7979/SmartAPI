using DBSmartAPIManager.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBSmartAPIManager.DAL.Context
{
    public class DBSmartAPIManagerContext:DbContext
    {

        public DBSmartAPIManagerContext()
        {          
        }

        public DBSmartAPIManagerContext(DbContextOptions<
            DBSmartAPIManagerContext> options)
            : base(options) 
        {
        }

        public virtual DbSet<Project>Projects  { get; set; }
        public virtual DbSet<User>Users  { get; set; }
        public virtual DbSet<ProjectFile>ProjectFiles  { get; set; }
        public virtual DbSet<ProjectJson>ProjectJsons  { get; set; }

        protected override void OnConfiguring
            (DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot config = new
                        ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
                optionsBuilder.UseSqlServer
                    (config.GetConnectionString("CS"));
                optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }
        }
    }
}
