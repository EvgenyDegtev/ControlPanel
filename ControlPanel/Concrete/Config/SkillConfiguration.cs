using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using ControlPanel.Models;

namespace ControlPanel.Concrete.Config
{
    public class SkillConfiguration: EntityTypeConfiguration<Skill>
    {
        public SkillConfiguration()
        {
            ToTable("Skills").HasIndex(skill => skill.Key).IsUnique();
            ToTable("Skills").HasIndex(skill => skill.Name);
        }
    }
}