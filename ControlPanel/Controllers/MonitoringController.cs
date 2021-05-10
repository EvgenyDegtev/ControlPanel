using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Threading;
using NLog;
using ControlPanel.Abstract;
using ControlPanel.Filters;

namespace ControlPanel.Controllers
{
    [Authorize]
    [ErrorLogger]
    [ActionLogger]
    public class MonitoringController: Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        IAgentRepository agentRepository;
        ISkillRepository skillRepository;

        public MonitoringController(IAgentRepository agentRepository, ISkillRepository skillRepository)
        {
            this.agentRepository = agentRepository;
            this.skillRepository = skillRepository;
        }

        [HttpGet]
        public ActionResult TopAgents ()
        {
            return View();
        }

        [HttpGet]
        public ActionResult TopSkills()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetTopAgents()
        {
            var agents = await agentRepository.GetAgentsIncludeGroupAsync();
            var monitoringAgents = agents.Select(agent => new { name = agent.Name, login = agent.Login, algorithm = agent.AlgorithmName }).Take(10).ToList();
            //Thread.Sleep(5000);
            return Json(monitoringAgents, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetTopSkills()
        {
            var skills = await skillRepository.GetSkillsAsync();
            var monitoringSkills = skills.Select(skill=>new {name=skill.Name,key=skill.Key,algorithm=skill.AlgorithmName }).Take(10).ToList();
            return Json(monitoringSkills, JsonRequestBehavior.AllowGet);
        }

    }
}