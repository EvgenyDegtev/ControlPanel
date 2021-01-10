using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using NLog;
using ControlPanel.Abstract;

namespace ControlPanel.Controllers
{
    [Authorize]
    public class MonitoringController: Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        IAgentRepository repository;

        public MonitoringController(IAgentRepository agentRepository)
        {
            this.repository = agentRepository;
        }

        [HttpGet]
        public ActionResult TopAgents ()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> TopSkills()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetTopAgents()
        {
            var agents = await repository.GetAgentsIncludeGroupAsync();
            var monitoringAgents = agents.Select(agent => new { name = agent.Name, login = agent.Login, algorithm = agent.Algorithm }).Take(10).ToList();
            return Json(monitoringAgents, JsonRequestBehavior.AllowGet);
        }

    }
}