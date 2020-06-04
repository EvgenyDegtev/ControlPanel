using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ControlPanel.Models;
using System.Net;
using PagedList;
using PagedList.Mvc;
using NLog;
using ControlPanel.Filters;
using System.Reflection;


namespace ControlPanel.Controllers
{
    public class AgentsController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        DataBaseContext db = new DataBaseContext();

        [ErrorLogger]
        public ActionResult Index(string searchString, int? page)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}| Input params: {nameof(searchString)}={searchString}, {nameof(page)}={page} ");

            int pageSize = 5;
            int pageNumber = page ?? 1;
            var agents = db.Agents.Include(agent => agent.Group).ToList();
            if(String.IsNullOrEmpty(searchString))
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return View(agents.ToPagedList(pageNumber, pageSize));
            }
            agents = agents.Where(agent => agent.Login.Contains(searchString)).ToList();

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(agents.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        [ErrorLogger]
        public ActionResult Create()
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            ViewBag.GR = new SelectList(db.Groups, "Id", "Name");
            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View();
        }

        [HttpPost]
        [ErrorLogger]
        public ActionResult Create([Bind(Include = "Id,Name,Login,Algorithm,IsAlgorithmAllowServiceLevel,WorkloadMaxContactsCount,IsActive,GroupId")] Agent agent)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(agent.Name)}={agent.Name}, {nameof(agent.Login)}={agent.Algorithm}, {nameof(agent.IsAlgorithmAllowServiceLevel)}={agent.IsAlgorithmAllowServiceLevel}, {nameof(agent.WorkloadMaxContactsCount)}={agent.WorkloadMaxContactsCount}, {nameof(agent.GroupId)}={agent.GroupId} ");

            if (ModelState.IsValid)
            {
                db.Agents.Add(agent);
                db.SaveChanges();

                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return RedirectToAction("Index");
            }

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(agent);
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
            var agent = db.Agents.Include(ag => ag.Group).FirstOrDefault(ag => ag.Id == id);
            if (agent == null)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return HttpNotFound();
            }
            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(agent);

        }

        [HttpPost]
        [ErrorLogger]
        public ActionResult Delete(int id)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(id)}={id}");
            Agent agent = db.Agents.Find(id);
            db.Agents.Remove(agent);
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
            Agent agent = db.Agents.Find(id);
            if (agent == null)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            ViewBag.Groups = new SelectList(db.Groups, "Id", "Name");

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(agent);
        }

        [HttpPost]
        [ErrorLogger]
        public ActionResult Edit([Bind(Include = "Id,Name,Login,Algorithm,IsAlgorithmAllowServiceLevel,WorkloadMaxContactsCount,IsActive,GroupId")] Agent agent)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(agent.Id)}={agent.Id}, {nameof(agent.Name)}={agent.Name}, {nameof(agent.Login)}={agent.Algorithm}, {nameof(agent.IsAlgorithmAllowServiceLevel)}={agent.IsAlgorithmAllowServiceLevel}, {nameof(agent.WorkloadMaxContactsCount)}={agent.WorkloadMaxContactsCount}, {nameof(agent.GroupId)}={agent.GroupId} ");
            if (ModelState.IsValid)
            {
                db.Entry(agent).State = EntityState.Modified;
                db.SaveChanges();

                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return RedirectToAction("Index");
            }

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(agent);
        }

        [HttpGet]
        [ErrorLogger]
        public JsonResult CheckLoginUnique (string Login, int? Id)
        {
            var agents = db.Agents.Where(ag => ag.Login == Login);
            //create agent
            if (Id==null)
            {
                if (agents.Count() > 0)
                {
                    return Json($"Агент с логином {Login} уже существует", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            //edit agent
            if(agents.Count()>0)
            {
                var agent = agents.First();
                //login corresponds id
                if(agent.Id==Id)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                return Json($"Агент с логином {Login} уже существует", JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AgentSkills(int? id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agent agent = db.Agents.Include(ag => ag.AgentToSkills).FirstOrDefault(ag => ag.Id == id);
            if(agent==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            List<AgentToSkill> agentToSkills = db.AgentToSkills.Include(agentToSkill => agentToSkill.Skill).Where(agentToSkill => agentToSkill.AgentId == id).ToList();
            ViewBag.AgentId = id;
            return View(agentToSkills);
        }

        [HttpGet]
        [ErrorLogger]
        public ActionResult AddSkill (int id)
        {
            List<Skill> skills = db.Skills.ToList();
            ViewBag.AgentId = id;
            return View(skills);
        }

        [HttpGet]
        [ErrorLogger]
        public ActionResult SkillAddConfirmation(int id, int skillId)
        {
            AgentToSkill agentToSkill = new AgentToSkill { AgentId = id, SkillId = skillId };
            string skillName = db.Skills.Find(skillId).Name;
            ViewBag.SkillName = skillName;
            return View(agentToSkill);
        }

        [HttpPost]
        [ErrorLogger]
        public ActionResult SkillAddConfirmation([Bind] AgentToSkill agentToSkill)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(agentToSkill.AgentId)}={agentToSkill.AgentId}, {nameof(agentToSkill.SkillId)}={agentToSkill.SkillId}, {nameof(agentToSkill.Level)}={agentToSkill.Level}, {nameof(agentToSkill.OrderIndex)}={agentToSkill.OrderIndex}, {nameof(agentToSkill.BreakingMode)}={agentToSkill.BreakingMode}, {nameof(agentToSkill.Percent)}={agentToSkill.Percent}");
            
            if(ModelState.IsValid)
            {
                agentToSkill.IsActive = true;
                db.AgentToSkills.Add(agentToSkill);
                db.SaveChanges();
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return RedirectToAction("AgentSkills", new { id = agentToSkill.AgentId });
            }

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(agentToSkill);
        }

        [HttpGet]
        [ErrorLogger]
        public ActionResult RemoveSkill (int id, int skillId)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(id)}={id}, {nameof(skillId)}={skillId}");
            AgentToSkill agentToSkill = db.AgentToSkills.Where(agToSkill =>( agToSkill.AgentId == id && agToSkill.SkillId == skillId)).FirstOrDefault();
            db.Entry(agentToSkill).State = EntityState.Deleted;
            db.SaveChanges();
            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return RedirectToAction("AgentSkills", new { id });
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