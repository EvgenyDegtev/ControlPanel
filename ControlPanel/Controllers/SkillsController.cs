using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControlPanel.Models;
using System.Data.Entity;
using System.Net;
using PagedList;
using ControlPanel.Filters;

namespace ControlPanel.Controllers
{
    
    public class SkillsController : Controller
    {
        DataBaseContext db = new DataBaseContext();

        [ErrorLogger]
        public ActionResult Index(string searchString, int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;
            var skills = db.Skills.ToList();
            if(String.IsNullOrEmpty(searchString))
            {
                return View(skills.ToPagedList(pageNumber,pageSize));
            }
            skills = db.Skills.Where(skill => skill.Key.Contains(searchString)).ToList();

            return View(skills.ToPagedList(pageNumber,pageSize));
        }

        [HttpGet]
        [ErrorLogger]
        public ActionResult Create(int? id)
        {
            return View();
        }

        [HttpPost]
        [ErrorLogger]
        public ActionResult Create([Bind] Skill skill)
        {
            if(ModelState.IsValid)
            {
                db.Skills.Add(skill);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(skill);
        }

        [HttpGet]
        [ErrorLogger]
        public ActionResult Delete (int? id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skill skill = db.Skills.Find(id);
            if(skill==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(skill);
        }

        [HttpPost]
        [ErrorLogger]
        public ActionResult Delete(int id)
        {
            Skill skill = db.Skills.Find(id);
            db.Skills.Remove(skill);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ErrorLogger]
        public ActionResult Edit (int? id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skill skill = db.Skills.Find(id);
            if(skill==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(skill);
        }

        [HttpPost]
        [ErrorLogger]
        public ActionResult Edit ([Bind] Skill skill)
        {
            if(ModelState.IsValid)
            {
                db.Entry(skill).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(skill);
        }

        [HttpGet]
        [ErrorLogger]
        public JsonResult CheckKeyUnique(string Key)
        {
            var skills = db.Skills.Where(sk => sk.Key == Key);
            if (skills.Count()>=1)
            {
                return Json($"Навык с ID {Key} уже существует", JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [ErrorLogger]
        public ActionResult SkillRoutes(int? id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skill skill = db.Skills.Include(sk => sk.Routes).FirstOrDefault(sk => sk.Id == id);
            if(skill==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(skill);
        }

        [ErrorLogger]
        public ActionResult RemoveRoute(int id, int routeId)
        {
            Route route = db.Routes.Find(routeId);
            Skill skill = db.Skills.Find(id);
            route.SkillId = null;
            db.Entry(route).State = EntityState.Modified;
            db.SaveChanges();           
            return RedirectToAction("SkillRoutes", skill);
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