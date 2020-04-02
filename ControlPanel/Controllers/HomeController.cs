using ControlPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControlPanel.Models;

namespace ControlPanel.Controllers
{
    public class HomeController : Controller
    {
        DataBaseContext db = new DataBaseContext();
        public ActionResult Index()
        {
            IEnumerable<Book> books = db.Books;
            ViewBag.Books = books;
            return View();
        }
    }
}