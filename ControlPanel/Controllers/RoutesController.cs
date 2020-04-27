using ControlPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace ControlPanel.Controllers
{
    public class RoutesController : Controller
    {
        DataBaseContext db = new DataBaseContext();
        // GET: Routes
        [HttpGet]
        public ActionResult Index()
        {
            var routes = db.Routes.Include(route => route.Skill).ToList();
            return View(routes);
        }


        [HttpGet]
        public ActionResult Create ()
        {
            ViewBag.Skills = new SelectList(db.Skills, "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create( [Bind] Route route)
        {
            if(ModelState.IsValid)
            {
                db.Routes.Add(route);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(route);
        }
    }
}