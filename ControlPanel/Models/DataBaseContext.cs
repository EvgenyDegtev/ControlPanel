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

        public DbSet<Book> Books { get; set; }

        public DbSet<Purchase> Purchases { get; set; }

        public DbSet<Agent> Agents { get; set; }

        public DbSet<Group> Groups { get; set; }
    }
}