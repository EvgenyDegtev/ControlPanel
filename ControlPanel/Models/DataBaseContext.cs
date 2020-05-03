using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ControlPanel.Models
{
    public class DataBaseContext: DbContext
    {
        public DataBaseContext()
            :base("MainContext")
        { }

        public DbSet<Agent> Agents { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Skill> Skills { get; set; }

        public DbSet<Route> Routes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}