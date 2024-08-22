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

        public virtual DbSet<Project>Project  { get; set; }
        public virtual DbSet<User>User  { get; set; }
        public virtual DbSet<ProjectFile>ProjectFile  { get; set; }
        public virtual DbSet<ProjectJson>ProjectJson  { get; set; }

        protected override void OnConfiguring
            (DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
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
}
