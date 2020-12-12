using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.Mvc;
using System.Data.Entity;
using ControlPanel.Models;
using System.Net;
using PagedList;
using PagedList.Mvc;
using NLog;
using ControlPanel.Filters;
using System.Reflection;
using ControlPanel.Abstract;
using System.Threading.Tasks;
using ControlPanel.ViewModels;

namespace ControlPanel.Controllers
{
    [Authorize]
    [SessionState(SessionStateBehavior.Disabled)]
    public class AgentsController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        IAgentRepository repository;

        public static void logWriter(Controller controller)
        {
            string controllerName = controller.RouteData.Values["controller"].ToString();
            string actionName= controller.RouteData.Values["action"].ToString();
            logger.Info($"Action Start | Controller name: {controllerName} | Action name: {actionName}| zzz");
        }

        public AgentsController (IAgentRepository agentRepository)
        {
            this.repository = agentRepository;
        }

        //Get and Post
        [ErrorLogger]
        public async Task<ActionResult> Index(string searchString, int? page, string selectedEntityName="Name", string sortOrder="asc", string prevSelectedEntityName=null)
        {
            logWriter(this);
            logger.Info($"Action Start | Controller name: {nameof(AgentsController)} | Action name: {nameof(Index)}| Input params: {nameof(searchString)}={searchString}, {nameof(page)}={page} ");

            int pageSize = 5;
            int pageNumber = page ?? 1;
            var agents = await repository.GetAgentsIncludeGroupAsync();

            AgentsIndexViewModel agentsIndexViewModel = new AgentsIndexViewModel
            {
                PagedAgents = agents.ToPagedList(pageNumber, pageSize),
                SearchString = searchString,
                SortOrder = sortOrder,
                SelectedEntityName=selectedEntityName,
                PrevSelectedEntityName=prevSelectedEntityName
            };

            if (String.IsNullOrEmpty(searchString))
            {
                logger.Info($"Action End | Controller name: {nameof(AgentsController)} | Action name: {nameof(Index)}");
                return View(agentsIndexViewModel);
            }
            agents = await repository.SearchAgentsAsync(searchString);
            agentsIndexViewModel.PagedAgents = agents.ToPagedList(pageNumber, pageSize);

            logger.Info($"Action End | Controller name: {nameof(AgentsController)} | Action name: {nameof(Index)}");

            return View(agentsIndexViewModel);
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> Create()
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");

            AgentCreateViewModel agentCreateViewModel = new AgentCreateViewModel
            {
                Groups= new SelectList(await repository.GetGroupsAsync(), "Id", "Name"),
                Algorithms= new SelectList(Agent.algorithmDictionary.Select(algo => new { Algorithm = algo.Key.ToString(), AlgorithmNAme = algo.Value }), "Algorithm", "AlgorithmName")

            };
            //ViewBag.Groups = new SelectList(await repository.GetGroupsAsync(), "Id", "Name");
            //ViewBag.Algorithms = new SelectList(Agent.algorithmDictionary.Select(algo=> new {Algorithm=algo.Key.ToString(),AlgorithmNAme=algo.Value }), "Algorithm", "AlgorithmName");

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(agentCreateViewModel);
        }

        [HttpPost]
        [ErrorLogger]
        public async Task<ActionResult> Create([Bind] Agent agent)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(agent.Name)}={agent.Name}, {nameof(agent.Login)}={agent.Algorithm}, {nameof(agent.IsAlgorithmAllowServiceLevel)}={agent.IsAlgorithmAllowServiceLevel}, {nameof(agent.WorkloadMaxContactsCount)}={agent.WorkloadMaxContactsCount}, {nameof(agent.GroupId)}={agent.GroupId} ");

            if (ModelState.IsValid)
            {
                repository.Create(agent);
                await repository.SaveAsync();

                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return RedirectToAction("Index");
            }

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(agent);
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> Delete(int? id)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(id)}={id}");
            if (id == null)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var agent = await repository.FindAgentByIdIncludeGroupAsync(id);
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
        public async Task<ActionResult> Delete(int id)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(id)}={id}");
            await repository.DeleteAsync(id);
            await repository.SaveAsync();

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> Edit(int? id)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(id)}={id}");
            if (id == null)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agent agent = await repository.FindAgentByIdAsync(id);
            if (agent == null)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            AgentCreateViewModel agentCreateViewModel = new AgentCreateViewModel
            {
                Agent = agent,
                Groups = new SelectList(await repository.GetGroupsAsync(), "Id", "Name"),
                Algorithms= new SelectList(Agent.algorithmDictionary.Select(algo => new { Algorithm = algo.Key.ToString(), AlgorithmNAme = algo.Value }), "Algorithm", "AlgorithmName", agent.Algorithm)
            };

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(agentCreateViewModel);
        }

        [HttpPost]
        [ErrorLogger]
        public async Task<ActionResult> Edit([Bind] Agent agent)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(agent.Id)}={agent.Id}, {nameof(agent.Name)}={agent.Name}, {nameof(agent.Login)}={agent.Algorithm}, {nameof(agent.IsAlgorithmAllowServiceLevel)}={agent.IsAlgorithmAllowServiceLevel}, {nameof(agent.WorkloadMaxContactsCount)}={agent.WorkloadMaxContactsCount}, {nameof(agent.GroupId)}={agent.GroupId} ");
            if (ModelState.IsValid)
            {
                repository.Update(agent);
                await repository.SaveAsync();

                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return RedirectToAction("Index");
            }

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(agent);
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<JsonResult> CheckLoginUnique (string login, int? id)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(login)}={login}, {nameof(id)}={id}");
            var agentsAlreadyInDb = await repository.FindAgentsByLoginAsync(login);

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
        public async Task<ActionResult> AgentSkills(int? id)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(id)}={id}");
            if (id==null)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agent agent = await repository.FindAgentByIdIncludeSkillsAsync(id);
            if(agent==null)
            {
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            List<AgentToSkill> agentToSkills = await repository.FindAgentToSkillForAgentByIdAsync(id);
            ViewBag.AgentId = id;

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(agentToSkills);
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> AddSkill (int id)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(id)}={id}");
            List<Skill> skills = await repository.GetSkillsAsync();
            ViewBag.AgentId = id;

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(skills);
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> SkillAddConfirmation(int agentId, int skillId)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(agentId)}={agentId}, {nameof(skillId)}={skillId}");
            AgentToSkill agentToSkill = new AgentToSkill { AgentId = agentId, SkillId = skillId };
            Skill skill = await repository.FindSkillByIdAsync(skillId);
            ViewBag.SkillName = skill.Name;

            ViewBag.levels = new SelectList(AgentToSkill.levelDictionary.Select(level=>new {Level=level.Key.ToString(), LevelName=level.Value}),"Level","LevelName",1);
            ViewBag.modes = new SelectList(AgentToSkill.breakingModeDictionary.Select(mode => new { BreakingMode = mode.Key.ToString(), BreakingModeName = mode.Value }),"BreakingMode","BreakingModeName", 2);

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(agentToSkill);
        }

        [HttpPost]
        [ErrorLogger]
        public async Task<ActionResult> SkillAddConfirmation([Bind] AgentToSkill agentToSkill)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(agentToSkill.AgentId)}={agentToSkill.AgentId}, {nameof(agentToSkill.SkillId)}={agentToSkill.SkillId}, {nameof(agentToSkill.Level)}={agentToSkill.Level}, {nameof(agentToSkill.OrderIndex)}={agentToSkill.OrderIndex}, {nameof(agentToSkill.BreakingMode)}={agentToSkill.BreakingMode}, {nameof(agentToSkill.Percent)}={agentToSkill.Percent}");
            
            if(ModelState.IsValid)
            {
                repository.CreateAgentToSkill(agentToSkill);
                await repository.SaveAsync();
                logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
                return RedirectToAction("AgentSkills", new { id = agentToSkill.AgentId });
            }

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(agentToSkill);
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ViewResult> EditSkill (int agentId, int skillId)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(agentId)}={agentId}, {nameof(skillId)}={skillId}");
            AgentToSkill agentToSkill = await repository.FindAgentToSkillAsync(agentId, skillId);
            Skill skill = await repository.FindSkillByIdAsync(skillId);
            ViewBag.SkillName = skill.Name;

            ViewBag.levels = new SelectList(AgentToSkill.levelDictionary.Select(level => new { Level = level.Key.ToString(), LevelName = level.Value }), "Level", "LevelName", agentToSkill.Level);
            ViewBag.modes = new SelectList(AgentToSkill.breakingModeDictionary.Select(mode => new {BreakingMode=mode.Key.ToString(),BreakingModeName=mode.Value }),"BreakingMode","BreakingModeName",agentToSkill.BreakingMode);

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(agentToSkill);
        }

        [HttpPost]
        [ErrorLogger]
        public async Task<ActionResult> EditSkill ([Bind] AgentToSkill agentToSkill)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(agentToSkill.Id)}={agentToSkill.Id}, {nameof(agentToSkill.AgentId)}={agentToSkill.AgentId}, {nameof(agentToSkill.SkillId)}={agentToSkill.SkillId}, {nameof(agentToSkill.Level)}={agentToSkill.Level}, {nameof(agentToSkill.OrderIndex)}={agentToSkill.OrderIndex}, {nameof(agentToSkill.BreakingMode)}={agentToSkill.BreakingMode}, {nameof(agentToSkill.Percent)}={agentToSkill.Percent}");

            if(ModelState.IsValid)
            {
                repository.UpdateAgentToSkill(agentToSkill);
                await repository.SaveAsync();
                return RedirectToAction("AgentSkills", new { id = agentToSkill.AgentId });
            }

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return View(agentToSkill);
        } 

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> RemoveSkill (int agentId, int skillId)
        {
            logger.Info($"Action Start | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name} | Input params: {nameof(agentId)}={agentId}, {nameof(skillId)}={skillId}");
            AgentToSkill agentToSkill = await repository.FindAgentToSkillAsync(agentId, skillId);
            await repository.DeleteAgentToSkillAsync(agentToSkill.Id);
            await repository.SaveAsync();

            logger.Info($"Action End | Controller name: {MethodBase.GetCurrentMethod().ReflectedType.Name} | Action name: {MethodBase.GetCurrentMethod().Name}");
            return RedirectToAction("AgentSkills", new { id=agentId });
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