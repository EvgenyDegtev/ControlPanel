using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControlPanel.Models;
using System.Data.Entity;
using System.Net;

namespace ControlPanel.Controllers
{
    
    public class SkillsController : Controller
    {
        DataBaseContext db = new DataBaseContext();
        // GET: Skills
        [HttpGet]
        public ActionResult Index()
        {
            var skills = db.Skills.ToList();
            return View(skills);
        }


        [HttpGet]
        public ActionResult Create(int? id)
        {
            return View();
        }

        [HttpPost]
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
        public ActionResult Delete(int id)
        {
            Skill skill = db.Skills.Find(id);
            db.Skills.Remove(skill);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
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
        public ActionResult Edit ([Bind] Skill skill)
        {
            if(ModelState.IsValid)
            {
                db.Entry(skill).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
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