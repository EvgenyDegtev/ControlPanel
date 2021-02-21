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
using System.Runtime.InteropServices;

namespace ControlPanel.Controllers
{
    [Authorize]
    [SessionState(SessionStateBehavior.Disabled)]
    [ActionLogger]
    public class AgentsController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        IAgentRepository repository;

        public AgentsController (IAgentRepository agentRepository)
        {
            this.repository = agentRepository;
        }

        //Get and Post
        [ErrorLogger]
        public async Task<ActionResult> Index(AgentsIndexViewModel agentsIndexModel)
        {
            logger.Info($"Action Start | Controller name: {nameof(AgentsController)} | Action name: {nameof(Index)}| Input params: {nameof(agentsIndexModel.SearchString)}={agentsIndexModel.SearchString}, " +
                $"{nameof(agentsIndexModel.Page)}={agentsIndexModel.Page}, {nameof(agentsIndexModel.SelectedGroupId)}={agentsIndexModel.SelectedGroupId}, " +
                $"{nameof(agentsIndexModel.SelectedAlgorithmId)}={agentsIndexModel.SelectedAlgorithmId}, {nameof(agentsIndexModel.IsServiceLevel)}={agentsIndexModel.IsServiceLevel}, " +
                $"{nameof(agentsIndexModel.SelectedSortProperty)}={agentsIndexModel.SelectedSortProperty} ,{nameof(agentsIndexModel.SortOrder)}={agentsIndexModel.SortOrder}");

            int pageSize = 5;
            int pageNumber = agentsIndexModel.Page ?? 1;
            var agents = await repository.GetAgentsIncludeGroupAsync();

            agents = SortAgents(agents, agentsIndexModel.SortOrder, agentsIndexModel.SelectedSortProperty);
                
            AgentsIndexViewModel agentsIndexViewModel = new AgentsIndexViewModel
            {
                SearchString = agentsIndexModel.SearchString,
                SortOrder = agentsIndexModel.SortOrder,
                SelectedSortProperty = agentsIndexModel.SelectedSortProperty,
                Groups = new SelectList(await repository.GetGroupsAsync(), "Id", "Name", agentsIndexModel.SelectedGroupId),
                SelectedGroupId = agentsIndexModel.SelectedGroupId,
                Algorithms = new SelectList(Agent.algorithmDictionary.Select(algo => new { Algorithm = algo.Key.ToString(), AlgorithmNAme = algo.Value }), "Algorithm", "AlgorithmName"),
                SelectedAlgorithmId = agentsIndexModel.SelectedAlgorithmId,
                IsServiceLevelList = new SelectList(AgentsIndexViewModel.IsServiceLevelListDictionary.Select(pair=>new {Flag=pair.Key.ToString(), Name=pair.Value }), "Flag", "Name"),
                IsServiceLevel= agentsIndexModel.IsServiceLevel
            };

            agents = FilterAgents(agents, agentsIndexModel.SelectedGroupId, agentsIndexModel.SelectedAlgorithmId, agentsIndexModel.IsServiceLevel, agentsIndexModel.SearchString);
            
            agentsIndexViewModel.PagedAgents = agents.ToPagedList(pageNumber, pageSize);
            return View(agentsIndexViewModel);
        }

        public async Task<ActionResult> AutocompleteSearch(string term)
        {
            logger.Info($"Action Start | Controller name: {nameof(AgentsController)} | Action name: {nameof(AutocompleteSearch)}| Input params: {nameof(term)}={term}");
            var agents = await repository.SearchAgentsAsync(term);
            var logins = agents.Select(agent => new { value = agent.Login }).Take(5).OrderByDescending(row=>row.value);
            return Json(logins, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> Create()
        {
            logger.Info($"Action Start | Controller name: {nameof(AgentsController)} | Action name: {nameof(Create)}");

            AgentCreateViewModel agentCreateViewModel = new AgentCreateViewModel
            {
                Groups= new SelectList(await repository.GetGroupsAsync(), "Id", "Name"),
                Algorithms= new SelectList(Agent.algorithmDictionary.Select(algo => new { Algorithm = algo.Key.ToString(), AlgorithmNAme = algo.Value }), "Algorithm", "AlgorithmName")

            };

            return View(agentCreateViewModel);
        }

        [HttpPost]
        [ErrorLogger]
        public async Task<ActionResult> Create([Bind] Agent agent)
        {
            logger.Info($"Action Start | Controller name: {nameof(AgentsController)} | Action name: {nameof(Create)} | Input params: {nameof(agent.Name)}={agent.Name}, {nameof(agent.Login)}={agent.Login}, {nameof(agent.IsAlgorithmAllowServiceLevel)}={agent.IsAlgorithmAllowServiceLevel}, {nameof(agent.WorkloadMaxContactsCount)}={agent.WorkloadMaxContactsCount}, {nameof(agent.GroupId)}={agent.GroupId} ");

            if (ModelState.IsValid)
            {
                repository.Create(agent);
                await repository.SaveAsync();
                return RedirectToAction("Index");
            }

            return View(agent);
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> Delete(int? id)
        {
            logger.Info($"Action Start | Controller name: {nameof(AgentsController)} | Action name: {nameof(Delete)} | Input params: {nameof(id)}={id}");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var agent = await repository.FindAgentByIdIncludeGroupAsync(id);
            if (agent == null)
            {
                return HttpNotFound();
            }
            return PartialView(agent);

        }

        [HttpPost]
        [ErrorLogger]
        public async Task<ActionResult> Delete(int id)
        {
            logger.Info($"Action Start | Controller name: {nameof(AgentsController)} | Action name: {nameof(Delete)} | Input params: {nameof(id)}={id}");
            await repository.DeleteAsync(id);
            await repository.SaveAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> Edit(int? id)
        {
            logger.Info($"Action Start | Controller name: {nameof(AgentsController)} | Action name: {nameof(Edit)} | Input params: {nameof(id)}={id}");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agent agent = await repository.FindAgentByIdAsync(id);
            if (agent == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            AgentCreateViewModel agentCreateViewModel = new AgentCreateViewModel
            {
                Agent = agent,
                Groups = new SelectList(await repository.GetGroupsAsync(), "Id", "Name"),
                Algorithms= new SelectList(Agent.algorithmDictionary.Select(algo => new { Algorithm = algo.Key.ToString(), AlgorithmNAme = algo.Value }), "Algorithm", "AlgorithmName", agent.Algorithm)
            };

            return View(agentCreateViewModel);
        }

        [HttpPost]
        [ErrorLogger]
        public async Task<ActionResult> Edit([Bind] Agent agent)
        {
            logger.Info($"Action Start | Controller name: {nameof(AgentsController)} | Action name: {nameof(Edit)} | Input params: {nameof(agent.Id)}={agent.Id}, {nameof(agent.Name)}={agent.Name}, {nameof(agent.Login)}={agent.Algorithm}, {nameof(agent.IsAlgorithmAllowServiceLevel)}={agent.IsAlgorithmAllowServiceLevel}, {nameof(agent.WorkloadMaxContactsCount)}={agent.WorkloadMaxContactsCount}, {nameof(agent.GroupId)}={agent.GroupId} ");
            if (ModelState.IsValid)
            {
                repository.Update(agent);
                await repository.SaveAsync();
                return RedirectToAction("Index");
            }
            return View(agent);
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<JsonResult> CheckLoginUnique (string login, int? id)
        {
            logger.Info($"Action Start | Controller name: {nameof(AgentsController)} | Action name: {nameof(CheckLoginUnique)} | Input params: {nameof(login)}={login}, {nameof(id)}={id}");
            var agentsAlreadyInDb = await repository.FindAgentsByLoginAsync(login);

            if(agentsAlreadyInDb.Count() <= 0)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var modifiedAgent = agentsAlreadyInDb.First();
                //check login corresponds id
                if (modifiedAgent.Id == id)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json($"Агент с логином {login} уже существует", JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> AgentSkills(int? id)
        {
            logger.Info($"Action Start | Controller name: {nameof(AgentsController)} | Action name: {nameof(AgentSkills)} | Input params: {nameof(id)}={id}");
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agent agent = await repository.FindAgentByIdIncludeSkillsAsync(id);
            if(agent==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            List<AgentToSkill> agentToSkills = await repository.FindAgentToSkillForAgentByIdAsync(id);
            AgentSkillsViewModel agentSkillsViewModel = new AgentSkillsViewModel
            {
                AgentId = id,
                AgentToSkills = agentToSkills
            };
            return View(agentSkillsViewModel);
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> AddSkill (int id)
        {
            logger.Info($"Action Start | Controller name: {nameof(AgentsController)} | Action name: {nameof(AddSkill)} | Input params: {nameof(id)}={id}");
            List<Skill> skills = await repository.GetSkillsAsync();

            AgentAddSkillViewModel agentAddSkillViewModel = new AgentAddSkillViewModel { Agent = await repository.FindAgentByIdAsync(id), Skills = skills };
            return View(agentAddSkillViewModel);
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> SkillAddConfirmation(int agentId, int skillId)
        {
            logger.Info($"Action Start | Controller name: {nameof(AgentsController)} | Action name: {nameof(SkillAddConfirmation)} | Input params: {nameof(agentId)}={agentId}, {nameof(skillId)}={skillId}");
            AgentToSkill agentToSkill = new AgentToSkill { AgentId = agentId, SkillId = skillId };
            Skill skill = await repository.FindSkillByIdAsync(skillId);

            AgentAddSkillConfirmationViewModel agentAddSkillConfirmationViewModel = new AgentAddSkillConfirmationViewModel
            {
                AgentToSkill = agentToSkill,
                SkillName = skill.Name,
                Levels = new SelectList(AgentToSkill.levelDictionary.Select(level => new { Level = level.Key.ToString(), LevelName = level.Value }), "Level", "LevelName", 1),
                Modes = new SelectList(AgentToSkill.breakingModeDictionary.Select(mode => new { BreakingMode = mode.Key.ToString(), BreakingModeName = mode.Value }), "BreakingMode", "BreakingModeName", 2)
            };
            return PartialView(agentAddSkillConfirmationViewModel);
        }

        [HttpPost]
        [ErrorLogger]
        public async Task<ActionResult> SkillAddConfirmation([Bind] AgentToSkill agentToSkill)
        {
            logger.Info($"Action Start | Controller name: {nameof(AgentsController)} | Action name: {nameof(SkillAddConfirmation)} | Input params: {nameof(agentToSkill.AgentId)}={agentToSkill.AgentId}, {nameof(agentToSkill.SkillId)}={agentToSkill.SkillId}, {nameof(agentToSkill.Level)}={agentToSkill.Level}, {nameof(agentToSkill.OrderIndex)}={agentToSkill.OrderIndex}, {nameof(agentToSkill.BreakingMode)}={agentToSkill.BreakingMode}, {nameof(agentToSkill.Percent)}={agentToSkill.Percent}");

            if (ModelState.IsValid)
            {
                repository.CreateAgentToSkill(agentToSkill);
                await repository.SaveAsync();
                return RedirectToAction("AgentSkills", new { id = agentToSkill.AgentId });
            }
            return View(agentToSkill);
        }

        [HttpGet]
        [ErrorLogger]
        public async Task<ViewResult> EditSkill (int agentId, int skillId)
        {
            logger.Info($"Action Start | Controller name: {nameof(AgentsController)} | Action name: {nameof(EditSkill)} | Input params: {nameof(agentId)}={agentId}, {nameof(skillId)}={skillId}");
            AgentToSkill agentToSkill = await repository.FindAgentToSkillAsync(agentId, skillId);

            Skill skill = await repository.FindSkillByIdAsync(skillId);

            AgentAddSkillConfirmationViewModel agentAddSkillConfirmationViewModel = new AgentAddSkillConfirmationViewModel
            {
                AgentToSkill = agentToSkill,
                SkillName = skill.Name,
                Levels = new SelectList(AgentToSkill.levelDictionary.Select(level => new { Level = level.Key.ToString(), LevelName = level.Value }), "Level", "LevelName", agentToSkill.Level),
                Modes = new SelectList(AgentToSkill.breakingModeDictionary.Select(mode => new { BreakingMode = mode.Key.ToString(), BreakingModeName = mode.Value }), "BreakingMode", "BreakingModeName", agentToSkill.BreakingMode)
            };
            return View(agentAddSkillConfirmationViewModel);
        }

        [HttpPost]
        [ErrorLogger]
        public async Task<ActionResult> EditSkill ([Bind] AgentToSkill agentToSkill)
        {
            logger.Info($"Action Start | Controller name: {nameof(EditSkill)} | Action name: {nameof(EditSkill)} | Input params: {nameof(agentToSkill.Id)}={agentToSkill.Id}, {nameof(agentToSkill.AgentId)}={agentToSkill.AgentId}, {nameof(agentToSkill.SkillId)}={agentToSkill.SkillId}, {nameof(agentToSkill.Level)}={agentToSkill.Level}, {nameof(agentToSkill.OrderIndex)}={agentToSkill.OrderIndex}, {nameof(agentToSkill.BreakingMode)}={agentToSkill.BreakingMode}, {nameof(agentToSkill.Percent)}={agentToSkill.Percent}");

            if(ModelState.IsValid)
            {
                repository.UpdateAgentToSkill(agentToSkill);
                await repository.SaveAsync();
                return RedirectToAction("AgentSkills", new { id = agentToSkill.AgentId });
            }
            return View(agentToSkill);
        } 

        [HttpGet]
        [ErrorLogger]
        public async Task<ActionResult> RemoveSkill (int agentId, int skillId)
        {
            logger.Info($"Action Start | Controller name: {nameof(AgentsController)} | Action name: {nameof(RemoveSkill)} | Input params: {nameof(agentId)}={agentId}, {nameof(skillId)}={skillId}");
            AgentToSkill agentToSkill = await repository.FindAgentToSkillAsync(agentId, skillId);
            await repository.DeleteAgentToSkillAsync(agentToSkill.Id);
            await repository.SaveAsync();

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

        private static List<Agent> SortAgents(List<Agent> agents, string sortOrder, string selectedSortProperty)
        {
            List<Agent> sortedAgents = agents;
            if (sortOrder == "desc" && selectedSortProperty == nameof(Agent.Name))
                sortedAgents = sortedAgents.OrderByDescending(agent => agent.Name).ToList();
            else if (sortOrder == "asc" && selectedSortProperty == nameof(Agent.Login))
                sortedAgents = sortedAgents.OrderBy(agent => agent.Login).ToList();
            else if (sortOrder == "desc" && selectedSortProperty == nameof(Agent.Login))
                sortedAgents = sortedAgents.OrderByDescending(agent => agent.Login).ToList();
            else if (sortOrder == "asc" && selectedSortProperty == nameof(Agent.Algorithm))
                sortedAgents = sortedAgents.OrderBy(agent => agent.Algorithm).ToList();
            else if (sortOrder == "desc" && selectedSortProperty == nameof(Agent.Algorithm))
                sortedAgents = sortedAgents.OrderByDescending(agent => agent.Algorithm).ToList();
            else if (sortOrder == "asc" && selectedSortProperty == nameof(Agent.WorkloadMaxContactsCount))
                sortedAgents = sortedAgents.OrderBy(agent => agent.WorkloadMaxContactsCount).ToList();
            else if (sortOrder == "desc" && selectedSortProperty == nameof(Agent.WorkloadMaxContactsCount))
                sortedAgents = sortedAgents.OrderByDescending(agent => agent.WorkloadMaxContactsCount).ToList();
            else if (sortOrder == "asc" && selectedSortProperty == nameof(Agent.IsAlgorithmAllowServiceLevel))
                sortedAgents = sortedAgents.OrderBy(agents => agents.IsAlgorithmAllowServiceLevel).ToList();
            else if (sortOrder == "desc" && selectedSortProperty == nameof(Agent.IsAlgorithmAllowServiceLevel))
                sortedAgents = sortedAgents.OrderByDescending(agent => agent.IsAlgorithmAllowServiceLevel).ToList();
            else if (sortOrder == "asc" && selectedSortProperty == nameof(Agent.Group))
                sortedAgents = sortedAgents.OrderBy(agent => agent.Group?.Name).ToList();
            else if (sortOrder == "desc" && selectedSortProperty == nameof(Agent.Group))
                sortedAgents = sortedAgents.OrderByDescending(agent => agent.Group?.Name).ToList();
            else
                sortedAgents = sortedAgents.OrderBy(agent => agent.Name).ToList();
            return sortedAgents;
        }

        private static List<Agent> FilterAgents(List<Agent> agents, int? selectedGroupId, int? selectedAlgorithmId, bool? isServiceLevelBool, string searchString)
        {
            List<Agent> filtredAgents = agents;
            if (selectedGroupId != null)
            {
                filtredAgents = filtredAgents.Where(agent => agent.GroupId == selectedGroupId).ToList();
            }

            if (selectedAlgorithmId != null)
            {
                filtredAgents = filtredAgents.Where(agent => agent.Algorithm == selectedAlgorithmId).ToList();
            }

            if (isServiceLevelBool != null)
            {
                filtredAgents = filtredAgents.Where(agent => agent.IsAlgorithmAllowServiceLevel == isServiceLevelBool).ToList();
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                filtredAgents = filtredAgents.Where(ag => ag.Login.ToLower().Contains(searchString.ToLower())).ToList();
            }
            return filtredAgents;
        }
    }
}