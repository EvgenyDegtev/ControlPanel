using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ControlPanel.Models
{
    public class DataBaseInitializer: CreateDatabaseIfNotExists<DataBaseContext>
    {
        protected override void Seed (DataBaseContext db)
        {
            db.Books.Add(new Book { Name = "Война и мир", Author = "Толстой", Price = 10 });
            db.Books.Add(new Book { Name = "Отцы и дети", Author = "И. Тургенев", Price = 20 });
            db.Books.Add(new Book { Name = "Чайка", Author = "А. Чехов", Price = 30 });
            db.SaveChanges();
        }
    }
}