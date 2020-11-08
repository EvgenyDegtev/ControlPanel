using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using ControlPanel.Models;

namespace ControlPanel.Concrete.Config
{
    public class RouteConfiguration: EntityTypeConfiguration<Route>
    {
        public RouteConfiguration()
        {
            ToTable("Routes").HasIndex(route => route.Key).IsUnique();
            ToTable("Routes").HasIndex(route => route.Name);
        }
    }
}