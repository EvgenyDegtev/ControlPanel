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
using NLog.LayoutRenderers;
using ControlPanel.Abstract;

namespace ControlPanel.Controllers
{
    public class AgentsController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        DataBaseContext db = new DataBaseContext();
        IAgentRepository repository;

        public AgentsController (IAgentRepository agentRepository)
        {
            this.repository = agentRepository;
        }

        //Get and Post
        [ErrorLogger]
        public ActionResult Index(string searchString, int? page)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}| Input params: {nameof(searchString)}={searchString}, {nameof(page)}={page} ");

            int pageSize = 5;
            int pageNumber = page ?? 1;
            var agents = repository.AgentsIncludeGroup.ToList();
            if(String.IsNullOrEmpty(searchString))
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return View(agents.ToPagedList(pageNumber, pageSize));
            }
            agents = repository.SearchAgent(searchString).ToList();

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(agents.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        [ErrorLogger]
        public ActionResult Create()
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            ViewBag.Groups = new SelectList(repository.Groups, "Id", "Name");

            var algorithm1 = new { Algorithm = 0, AlgorithmName = "Максимальная потребоность" };
            var algorithm2 = new { Algorithm = 1, AlgorithmName = "Уровень навыка" };
            var algorithm3 = new { Algorithm = 2, AlgorithmName = "Процентное распределение" };
            var algorithms = new[] { algorithm1, algorithm2, algorithm3 };
            ViewBag.Algorithms = new SelectList(algorithms, "Algorithm", "AlgorithmName");

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View();
        }

        [HttpPost]
        [ErrorLogger]
        public ActionResult Create([Bind] Agent agent)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(agent.Name)}={agent.Name}, {nameof(agent.Login)}={agent.Algorithm}, {nameof(agent.IsAlgorithmAllowServiceLevel)}={agent.IsAlgorithmAllowServiceLevel}, {nameof(agent.WorkloadMaxContactsCount)}={agent.WorkloadMaxContactsCount}, {nameof(agent.GroupId)}={agent.GroupId} ");

            if (ModelState.IsValid)
            {
                repository.Create(agent);
                repository.Save();

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
            var agent = repository.FindAgentByIdIncludeGroup((int)id);
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
            repository.Delete(id);
            repository.Save();

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
            Agent agent = repository.FindAgentById((int)id);
            if (agent == null)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            ViewBag.Groups = new SelectList(repository.Groups, "Id", "Name");

            var algorithm1 = new { Algorithm = 0, AlgorithmName = "Максимальная потребоность" };
            var algorithm2 = new { Algorithm = 1, AlgorithmName = "Уровень навыка" };
            var algorithm3 = new { Algorithm = 2, AlgorithmName = "Процентное распределение" };
            var algorithms = new[] { algorithm1, algorithm2, algorithm3 };
            ViewBag.Algorithms = new SelectList(algorithms, "Algorithm", "AlgorithmName", agent.Algorithm);

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(agent);
        }

        [HttpPost]
        [ErrorLogger]
        public ActionResult Edit([Bind] Agent agent)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(agent.Id)}={agent.Id}, {nameof(agent.Name)}={agent.Name}, {nameof(agent.Login)}={agent.Algorithm}, {nameof(agent.IsAlgorithmAllowServiceLevel)}={agent.IsAlgorithmAllowServiceLevel}, {nameof(agent.WorkloadMaxContactsCount)}={agent.WorkloadMaxContactsCount}, {nameof(agent.GroupId)}={agent.GroupId} ");
            if (ModelState.IsValid)
            {
                repository.Update(agent);
                repository.Save();

                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return RedirectToAction("Index");
            }

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(agent);
        }

        [HttpGet]
        [ErrorLogger]
        public JsonResult CheckLoginUnique (string login, int? id)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(login)}={login}, {nameof(id)}={id}");
            //var agentsAlreadyInDb2 = db.Agents.Where(ag => ag.Login == login);
            var agentsAlreadyInDb = repository.FindAgentsByLogin(login);

            if(agentsAlreadyInDb.Count() <= 0)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var modifiedAgent = agentsAlreadyInDb.First();
                //check login corresponds id
                if (modifiedAgent.Id == id)
                {
                    logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                    return Json($"Агент с логином {login} уже существует", JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet]
        [ErrorLogger]
        public ActionResult AgentSkills(int? id)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(id)}={id}");
            if (id==null)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agent agent = repository.FindAgentByIdIncludeSkill((int)id);
            if(agent==null)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            List<AgentToSkill> agentToSkills = repository.FindAgentToSkillForAgentById((int)id).ToList();
            ViewBag.AgentId = id;

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(agentToSkills);
        }

        [HttpGet]
        [ErrorLogger]
        public ActionResult AddSkill (int id)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(id)}={id}");
            List<Skill> skills = repository.Skills.ToList();
            //List<Skill> skills = db.Skills.ToList();
            ViewBag.AgentId = id;

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(skills);
        }

        [HttpGet]
        [ErrorLogger]
        public ActionResult SkillAddConfirmation(int id, int skillId)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(id)}={id}, {nameof(skillId)}={skillId}");
            AgentToSkill agentToSkill = new AgentToSkill { AgentId = id, SkillId = skillId };
            //string skillName = db.Skills.Find(skillId).Name;
            string skillName = repository.FindSkillById(skillId).Name;
            ViewBag.SkillName = skillName;

            var level1 = new { Level = 1, LevelName = "1" };
            var level2 = new { Level = 2, LevelName = "2" };
            var level3 = new { Level = 3, LevelName = "3" };
            var level4 = new { Level = 4, LevelName = "4" };
            var level5 = new { Level = 5, LevelName = "5" };
            var level6 = new { Level = 6, LevelName = "6" };
            var level7 = new { Level = 7, LevelName = "7" };
            var level8 = new { Level = 8, LevelName = "8" };
            var level9 = new { Level = 9, LevelName = "9" };
            var level10 = new { Level = 10, LevelName = "10" };
            var levelR1 = new { Level = -1, LevelName = "R1" };
            var levelR2 = new { Level = -2, LevelName = "R2" };
            var levels = new[] { level1, level2, level3, level4, level5, level6, level7, level8,level9,level10,levelR1,levelR2 };
            ViewBag.levels = new SelectList(levels, "Level", "LevelName",1); ;

            var mode1 = new { BreakingMode = 1, BreakingModeName = "Отключен" };
            var mode2 = new { BreakingMode = 2, BreakingModeName = "Автоматический" };
            var mode3 = new { BreakingMode = 3, BreakingModeName = "Ручной" };
            var modes = new[] { mode1, mode2, mode3 };
            ViewBag.modes = new SelectList(modes, "BreakingMode", "BreakingModename", 2);

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
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
                //db.AgentToSkills.Add(agentToSkill);
                //db.SaveChanges();

                repository.CreateAgentToSkill(agentToSkill);
                repository.Save();
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return RedirectToAction("AgentSkills", new { id = agentToSkill.AgentId });
            }

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(agentToSkill);
        }

        [HttpGet]
        [ErrorLogger]
        public ActionResult EditSkill (int id, int skillId)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(id)}={id}, {nameof(skillId)}={skillId}");
            AgentToSkill agentToSkill = db.AgentToSkills.Where(agToSkill => agToSkill.AgentId == id && agToSkill.SkillId == skillId).First();
            //AgentToSkill agentToSkill = repository.FindAgentToSkills(id, skillId).First();
            string skillName = repository.FindSkillById(skillId).Name;
            //string skillName = db.Skills.Find(skillId).Name;
            ViewBag.SkillName = skillName;

            var level1 = new { Level = 1, LevelName = "1" };
            var level2 = new { Level = 2, LevelName = "2" };
            var level3 = new { Level = 3, LevelName = "3" };
            var level4 = new { Level = 4, LevelName = "4" };
            var level5 = new { Level = 5, LevelName = "5" };
            var level6 = new { Level = 6, LevelName = "6" };
            var level7 = new { Level = 7, LevelName = "7" };
            var level8 = new { Level = 8, LevelName = "8" };
            var level9 = new { Level = 9, LevelName = "9" };
            var level10 = new { Level = 10, LevelName = "10" };
            var levelR1 = new { Level = -1, LevelName = "R1" };
            var levelR2 = new { Level = -2, LevelName = "R2" };
            var levels = new[] { level1, level2, level3, level4, level5, level6, level7, level8, level9, level10, levelR1, levelR2 };
            ViewBag.levels = new SelectList(levels, "Level", "LevelName", agentToSkill.Level); ;

            var mode1 = new { BreakingMode = 1, BreakingModeName = "Отключен" };
            var mode2 = new { BreakingMode = 2, BreakingModeName = "Автоматический" };
            var mode3 = new { BreakingMode = 3, BreakingModeName = "Ручной" };
            var modes = new[] { mode1, mode2, mode3 };
            ViewBag.modes = new SelectList(modes, "BreakingMode", "BreakingModename", agentToSkill.BreakingMode);

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(agentToSkill);
        }

        [HttpPost]
        [ErrorLogger]
        public ActionResult EditSkill ([Bind] AgentToSkill agentToSkill)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(agentToSkill.Id)}={agentToSkill.Id}, {nameof(agentToSkill.AgentId)}={agentToSkill.AgentId}, {nameof(agentToSkill.SkillId)}={agentToSkill.SkillId}, {nameof(agentToSkill.Level)}={agentToSkill.Level}, {nameof(agentToSkill.OrderIndex)}={agentToSkill.OrderIndex}, {nameof(agentToSkill.BreakingMode)}={agentToSkill.BreakingMode}, {nameof(agentToSkill.Percent)}={agentToSkill.Percent}");

            db.Entry(agentToSkill).State = EntityState.Modified;
            db.SaveChanges();
            //repository.Save();
            
            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ErrorLogger]
        public ActionResult RemoveSkill (int id, int skillId)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(id)}={id}, {nameof(skillId)}={skillId}");
            AgentToSkill agentToSkill = repository.FindAgentToSkills(id, skillId).FirstOrDefault();
            repository.DeleteAgentToSkill(agentToSkill.Id);
            repository.Save();

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return RedirectToAction("AgentSkills", new { id });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}