using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ControlPanel.Models;
using ControlPanel.Concrete.Config;

namespace ControlPanel.Concrete
{
    public class EFDbContext: DbContext
    {
        public EFDbContext()
            :base("MainContext")
        { }

        public DbSet<Skill> Skills { get; set; }

        public DbSet<Route> Routes { get; set; }

        public DbSet<Agent> Agents { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<AgentToSkill> AgentToSkills { get; set; }

        public System.Data.Entity.DbSet<ControlPanel.Models.Report> Reports { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AgentConfiguration());
            modelBuilder.Configurations.Add(new GroupConfiguration());
            modelBuilder.Configurations.Add(new SkillConfiguration());
            modelBuilder.Configurations.Add(new RouteConfiguration());
        }
    }
}