﻿using ControlPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using PagedList;
using ControlPanel.Filters;
using NLog;
using System.Reflection;

namespace ControlPanel.Controllers
{
    public class RoutesController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        DataBaseContext db = new DataBaseContext();

        //Get and Post
        [ErrorLogger]
        public ActionResult Index(string searchString, int? page)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}| Input params: {nameof(searchString)}={searchString}, {nameof(page)}={page} ");


            int pageSize = 5;
            int pageNumber = page ?? 1;
            var routes = db.Routes.Include(route => route.Skill).ToList();
            if(String.IsNullOrEmpty(searchString))
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return View(routes.ToPagedList(pageNumber,pageSize));
            }
            routes = routes.Where(route => route.Key.Contains(searchString)).ToList();

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(routes.ToPagedList(pageNumber,pageSize));
        }

        [HttpGet]
        [ErrorLogger]
        public ActionResult Create ()
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            ViewBag.Skills = new SelectList(db.Skills, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ErrorLogger]
        public ActionResult Create( [Bind] Route route)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(route.Name)}={route.Name}, {nameof(route.Key)}={route.Key}, {nameof(route.SkillId)}={route.SkillId}");

            if (ModelState.IsValid)
            {
                db.Routes.Add(route);
                db.SaveChanges();

                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return RedirectToAction("Index");
            }

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(route);
        }
        
        [HttpGet]
        [ErrorLogger]
        public ActionResult Delete(int? id)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(id)}={id}");
            if (id==null)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Route route = db.Routes.Include(rt=>rt.Skill).FirstOrDefault(rt=>rt.Id==id);
            if(route==null)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(route);
        }

        [HttpPost]
        [ErrorLogger]
        public ActionResult Delete(int id)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(id)}={id}");
            Route route = db.Routes.Find(id);
            db.Routes.Remove(route);
            db.SaveChanges();

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ErrorLogger]
        public ActionResult Edit(int? id)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(id)}={id}");
            if (id==null)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Route route = db.Routes.Find(id);
            if(route==null)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            ViewBag.Skills = new SelectList(db.Skills, "Id", "Name");

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(route);
        }

        [HttpPost]
        [ErrorLogger]
        public ActionResult Edit([Bind] Route route)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {nameof(route.Id)}={route.Id}, {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(route.Name)}={route.Name}, {nameof(route.Key)}={route.Key}, {nameof(route.SkillId)}={route.SkillId}");

            if (ModelState.IsValid)
            {
                db.Entry(route).State = EntityState.Modified;
                db.SaveChanges();

                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return RedirectToAction("Index");
            }
            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
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