using ControlPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;

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
        
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Route route = db.Routes.Find(id);
            if(route==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(route);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Route route = db.Routes.Find(id);
            db.Routes.Remove(route);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Route route = db.Routes.Find(id);
            if(route==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            ViewBag.Skills = new SelectList(db.Skills, "Id", "Name");
            return View(route);
        }

        [HttpPost]
        public ActionResult Edit([Bind] Route route)
        {
            if(ModelState.IsValid)
            {
                db.Entry(route).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(route);
        }
    }
}