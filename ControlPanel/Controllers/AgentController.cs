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
            return View(db.Agents.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create ([Bind(Include ="Id,Name,Login,Algorithm,IsAlgorithmAllowServiceLevel,WorkloadMaxContactsCount,IsActive")] Agent agent)
        {
            if(ModelState.IsValid)
            {
                db.Agents.Add(agent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agent);
        }
    }
}