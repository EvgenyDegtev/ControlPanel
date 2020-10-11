using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using ControlPanel.Models;
using ControlPanel.Abstract;
using NLog;

namespace ControlPanel.Controllers
{
    public class ReportsController : Controller
    {
        IAgentRepository agentRpository;
        ISkillRepository skillRepository;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // GET: Reports
        public ActionResult Index()
        {
            string[] reports = new string[] { "AgentReport", "Skillreport" };
            ViewBag.Reports = new SelectList(reports);
            return View();
        }

        [HttpPost]
        public FileResult GetReport([Bind] Report report)
        {

            string reportString = report.Name.ToString()+";"+report.DateFrom.ToString() + ";" + report.DateTo.ToString();
            Encoding encoding = Encoding.ASCII;
            byte[] reportStream = encoding.GetBytes(reportString);
            //FileStream fs = new FileStream();
            string file_type = "application/csv";
            string file_name = "1.csv";
            return File(reportStream, file_type, file_name);
        }
    }
}