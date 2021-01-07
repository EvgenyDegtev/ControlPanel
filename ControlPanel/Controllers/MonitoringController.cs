using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using NLog;

namespace ControlPanel.Controllers
{
    [Authorize]
    public class MonitoringController: Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

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

    }
}