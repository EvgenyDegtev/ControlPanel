using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ControlPanel.Concrete
{
    public class EFDbInitializer: CreateDatabaseIfNotExists<EFDbContext>
    {
        protected override void Seed(EFDbContext context)
        {
        }
    }
}