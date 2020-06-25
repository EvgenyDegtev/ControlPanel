using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ControlPanel.Concrete
{
    public class DataBaseInitializer: CreateDatabaseIfNotExists<DataBaseContext>
    {
        protected override void Seed (DataBaseContext db)
        {
        }
    }
}