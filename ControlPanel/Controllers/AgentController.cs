using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ControlPanel.Models;

namespace ControlPanel.Controllers
{
    public class AgentController : Controller
    {
        DataBaseContext db = new DataBaseContext();
        // GET: Agent
        public ActionResult Index()
        {
            IEnumerable<Book> books = db.Books;
            ViewBag.Books = books;
            return View();
        }
    }
}