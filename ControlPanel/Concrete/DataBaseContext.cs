using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ControlPanel.Models;

namespace ControlPanel.Concrete
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

        public DbSet<AgentToSkill> AgentToSkills { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}