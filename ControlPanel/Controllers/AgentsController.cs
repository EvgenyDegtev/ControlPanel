using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ControlPanel.Models;
using System.Net;

namespace ControlPanel.Controllers
{
    public class AgentsController : Controller
    {
        DataBaseContext db = new DataBaseContext();
        // GET: Agent
        public ActionResult Index(string searchString)
        {
            if(String.IsNullOrEmpty(searchString))
            {
                return View(db.Agents.Include(agent=>agent.Group).ToList());
            }
            var agents = db.Agents.Where(ag => ag.Login.Contains(searchString)).Include(agent => agent.Group).ToList();
            return View(agents);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.GR = new SelectList(db.Groups, "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,Name,Login,Algorithm,IsAlgorithmAllowServiceLevel,WorkloadMaxContactsCount,IsActive,GroupId")] Agent agent)
        {
            if (ModelState.IsValid)
            {
                db.Agents.Add(agent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agent);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var agent = db.Agents.Include(ag => ag.Group).FirstOrDefault(ag => ag.Id == id);
            if (agent == null)
            {
                return HttpNotFound();
            }
            return View(agent);

        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Agent agent = db.Agents.Find(id);
            db.Agents.Remove(agent);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agent agent = db.Agents.Find(id);
            if (agent == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            ViewBag.Groups = new SelectList(db.Groups, "Id", "Name");
            return View(agent);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Name,Login,Algorithm,IsAlgorithmAllowServiceLevel,WorkloadMaxContactsCount,IsActive,GroupId")] Agent agent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(agent);
        }

        [HttpGet]
        public JsonResult CheckLoginUnique (string Login)
        {
            var agents = db.Agents.Where(ag => ag.Login == Login);
            if(agents.Count()>=1)
            {
                return Json($"Агент с логином {Login} уже существует", JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}