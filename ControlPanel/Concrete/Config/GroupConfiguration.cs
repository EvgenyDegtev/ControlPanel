using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using ControlPanel.Models;

namespace ControlPanel.Concrete.Config
{
    public class GroupConfiguration : EntityTypeConfiguration<Group>
    {
        public GroupConfiguration()
        {
            ToTable("Groups").HasIndex(group => group.Name).IsUnique();
            ToTable("Groups").Property(group => group.Description).HasMaxLength(8000);
        }
    }
}