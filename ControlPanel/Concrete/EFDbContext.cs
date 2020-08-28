﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ControlPanel.Models;

namespace ControlPanel.Concrete
{
    public class EFDbContext: DbContext
    {
        public EFDbContext()
            :base("MainContext")
        { }

        public DbSet<Skill> Skills { get; set; }

        public DbSet<Route> Routes { get; set; }
    }
}