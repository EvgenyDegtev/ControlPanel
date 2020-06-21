using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ControlPanel.Models;
using PagedList;
using ControlPanel.Filters;
using NLog;
using System.Reflection;

namespace ControlPanel.Controllers
{
    public class GroupsController : Controller
    {
        private DataBaseContext db = new DataBaseContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        //Get and Post
        [ErrorLogger]
        public ActionResult Index(string searchString, int? page)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}| Input params: {nameof(searchString)}={searchString}, {nameof(page)}={page} ");

            int pageSize = 5;
            int pageNumber = page ?? 1;
            var groups = db.Groups.ToList();
            if(String.IsNullOrEmpty(searchString))
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return View(groups.ToPagedList(pageNumber,pageSize));
            }
            groups = groups.Where(group => group.Name.Contains(searchString)).ToList();

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(groups.ToPagedList(pageNumber,pageSize));
        }

        [HttpGet]
        [ErrorLogger]
        public ActionResult Create()
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View();
        }

        [HttpPost]
        [ErrorLogger]
        public ActionResult Create([Bind] Group group)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(group.Name)}={group.Name}, {nameof(group.Description)}={group.Description}");

            if (ModelState.IsValid)
            {
                db.Groups.Add(group);
                db.SaveChanges();

                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return RedirectToAction("Index");
            }

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(group);
        }

        [HttpGet]
        [ErrorLogger]
        public ActionResult Delete(int? id)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(id)}={id}");
            if (id == null)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return HttpNotFound();
            }
            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(group);
        }

        [HttpPost]
        [ErrorLogger]
        public ActionResult Delete(int id)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(id)}={id}");
            Group group = db.Groups.Find(id);
            db.Groups.Remove(group);
            db.SaveChanges();

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ErrorLogger]
        public ActionResult Edit(int? id)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(id)}={id}");
            if (id == null)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return HttpNotFound();
            }

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(group);
        }

        [HttpPost]
        [ErrorLogger]
        public ActionResult Edit([Bind] Group group)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(group.Id)}={group.Id}, {nameof(group.Name)}={group.Name}, {nameof(group.Description)}={group.Description}");

            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();

                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return RedirectToAction("Index");
            }
            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(group);
        }

        [HttpGet]
        [ErrorLogger]
        public ActionResult GroupAgents(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Include(gr => gr.Agents).FirstOrDefault(gr => gr.Id == id);
            if (group == null)
            {
                return HttpNotFound();
            }

            return View(group);
        }

        [HttpGet]
        [ErrorLogger]
        public JsonResult CheckNameUnique (string name, int? id)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(name)}={name}, {nameof(id)}={id}");
            var groupsAlreadyInDb = db.Groups.Where(group => group.Name == name);

            if (groupsAlreadyInDb.Count() <= 0)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var modifiedGroup = groupsAlreadyInDb.First();
                //check name corresponds id
                if (modifiedGroup.Id == id)
                {
                    logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                    return Json($"Группа с названием {name} уже существует", JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet]
        [ErrorLogger]
        public ActionResult RemoveAgent (int id,int agentId)
        {
            var group = db.Groups.Find(id);
            var agent = db.Agents.Find(agentId);
            agent.GroupId = null;
            db.Entry(agent).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("GroupAgents",group);
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
