﻿using System;
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

namespace ControlPanel.Controllers
{
    public class GroupsController : Controller
    {
        private DataBaseContext db = new DataBaseContext();

        [ErrorLogger]
        public ActionResult Index(string searchString, int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;
            var groups = db.Groups.ToList();
            if(String.IsNullOrEmpty(searchString))
            {
                return View(groups.ToPagedList(pageNumber,pageSize));
            }
            groups = groups.Where(group => group.Name.Contains(searchString)).ToList();
            //var groups = db.Groups.Where(gr => gr.Name.Contains(searchString)).ToList();
            return View(groups.ToPagedList(pageNumber,pageSize));
        }

        // GET: Group/Create
        [ErrorLogger]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Group/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ErrorLogger]
        public ActionResult Create([Bind(Include = "Id,Name,Description,IsActive")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(group);
        }

        // GET: Group/Edit/5
        [ErrorLogger]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: Group/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ErrorLogger]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,IsActive")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        // GET: Group/Delete/5
        [ErrorLogger]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: Group/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [ErrorLogger]
        public ActionResult DeleteConfirmed(int id)
        {
            Group group = db.Groups.Find(id);
            db.Groups.Remove(group);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Get
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
        public JsonResult CheckNameUnique (string Name)
        {
            var groups = db.Groups.Where(gr => gr.Name == Name);
            if(groups.Count()>=1)
            {
                return Json($"Группа с названием {Name} уже существует", JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
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
