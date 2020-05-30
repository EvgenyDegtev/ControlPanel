using ControlPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using PagedList;
using ControlPanel.Filters;

namespace ControlPanel.Controllers
{
    public class RoutesController : Controller
    {
        DataBaseContext db = new DataBaseContext();

        [ErrorLogger]
        public ActionResult Index(string searchString, int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;
            var routes = db.Routes.Include(route => route.Skill).ToList();
            if(String.IsNullOrEmpty(searchString))
            {
                return View(routes.ToPagedList(pageNumber,pageSize));
            }
            routes = routes.Where(route => route.Key.Contains(searchString)).ToList();
            return View(routes.ToPagedList(pageNumber,pageSize));
        }

        [HttpGet]
        [ErrorLogger]
        public ActionResult Create ()
        {
            ViewBag.Skills = new SelectList(db.Skills, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ErrorLogger]
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
        [ErrorLogger]
        public ActionResult Delete(int? id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Route route = db.Routes.Include(rt=>rt.Skill).FirstOrDefault(rt=>rt.Id==id);
            if(route==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(route);
        }

        [HttpPost]
        [ErrorLogger]
        public ActionResult Delete(int id)
        {
            Route route = db.Routes.Find(id);
            db.Routes.Remove(route);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ErrorLogger]
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
        [ErrorLogger]
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

        [HttpGet]
        [ErrorLogger]
        public JsonResult CheckKeyUnique (string Key)
        {
            var routes = db.Routes.Where(route => route.Key == Key);
            if(routes.Count()>=1)
            {
                return Json($"Маршрут с ID {Key} уже существует", JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }


        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}