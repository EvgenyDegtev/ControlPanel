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
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using CsvHelper;
using System.Globalization;

namespace ControlPanel.Controllers
{
    public class ReportsController : Controller
    {
        IAgentRepository agentRpository;
        ISkillRepository skillRepository;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ReportsController(IAgentRepository agentRepository, ISkillRepository skillRepository)
        {
            this.agentRpository = agentRepository;
            this.skillRepository = skillRepository;
        }

        // GET: Reports
        public ActionResult Index()
        {
            string[] reports = new string[] { "AgentReport", "SkillReport" };
            ViewBag.Reports = new SelectList(reports);
            return View();
        }

        [HttpPost]
        public async Task<FileResult> GetReport([Bind] Report report)
        {
            string pathCsvFile=null;
            switch (report.Name)
            {
                case "AgentReport":
                    var agentsForReport = await agentRpository.GetAgentsAsync();
                    pathCsvFile=CreateCsvReport<Agent>(report, agentsForReport);
                    break;

                case "SkillReport":
                    var skillsForReport = await skillRepository.GetSkillsAsync();
                    pathCsvFile = CreateCsvReport<Skill>(report, skillsForReport);
                    break;
                default:
                    break;
            }

            string fileType = "application/csv";
            string fileName = $"{report.Name}_{report.DateFrom}_{report.DateTo}.csv";
            return File(pathCsvFile, fileType, fileName);
        }

        private string CreateCsvReport<T>(Report report, List<T> reportData)
        {
            string pathCsvFile = $@"C:\Users\EDegtev\AppData\Roaming\ControlPanel\{report.Name}.csv";

            using (StreamWriter streamWriter = new StreamWriter(pathCsvFile))
            {
                using (CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                {
                    csvWriter.Configuration.Delimiter = ";";
                    csvWriter.WriteRecords(reportData);
                }
            }
            return pathCsvFile;
        }
    }
}