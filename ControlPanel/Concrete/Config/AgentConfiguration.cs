﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using ControlPanel.Models;

namespace ControlPanel.Concrete.Config
{
    public class AgentConfiguration: EntityTypeConfiguration<Agent>
    {
        public AgentConfiguration()
        {
            ToTable("Agents").HasIndex(agent => agent.Login).IsUnique();
            ToTable("Agents").HasIndex(agent => agent.Name);
        }
    }
}