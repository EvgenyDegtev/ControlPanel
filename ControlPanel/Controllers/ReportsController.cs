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
using CsvHelper.Configuration;
using System.Globalization;
using ControlPanel.ViewModels;
using System.Net;
using ControlPanel.Filters;

namespace ControlPanel.Controllers
{
    [Authorize]
    [ErrorLogger]
    [ActionLogger]
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
            ReportsIndexViewModel reportsIndexViewModel = new ReportsIndexViewModel
            {
                ReportNames = new SelectList(reports),
                Report = new Report 
                { 
                    Name = "SkillReport", 
                    DateFrom= DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "T" + DateTime.Now.AddDays(-1).ToString("HH:mm"),
                    DateTo = DateTime.Now.ToString("yyyy-MM-dd")+"T"+ DateTime.Now.ToString("HH:mm") 
                }
            };
            return View(reportsIndexViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> GetReport(string actionName, [Bind] Report report)
        {
            if(actionName.ToLower()=="preview"&& report.Name == "AgentReport")
            {
                var agentsForReport = await agentRpository.GetAgentsIncludeGroupAsync();
                agentsForReport = agentsForReport.Take(10).OrderBy(agent => agent.Login).ToList();
                return View("GetAgentReport", agentsForReport);
            }
            if(actionName.ToLower() == "preview" && report.Name =="SkillReport")
            {
                var skillsForReport = skillRepository.GetSkillsFromSqlQuery();
                skillsForReport = skillsForReport.Take(10).OrderBy(skill => skill.Key).ToList();
                return View("GetSkillReport", skillsForReport);
            }

            var csvFilePath=await GetCsvFilePath(report);
            string fileType = "application/csv";
            string fileName = $"{report.Name}_{report.DateFrom}_{report.DateTo}.csv";
            return File(csvFilePath, fileType, fileName);
        }

        private async Task<string> GetCsvFilePath(Report report)
        {
            string csvFilePath = null;
            switch (report.Name)
            {
                case "AgentReport":
                    var agents = await agentRpository.GetAgentsAsync();
                    List<AgentReportViewModel> agentsForReport = agents
                        .Select(agent => new AgentReportViewModel
                        {
                            Id = agent.Id,
                            Name = agent.Name,
                            Login = agent.Login,
                            WorkloadMaxContactsCount = agent.WorkloadMaxContactsCount,
                            IsAlgorithmAllowServiceLevel = agent.IsAlgorithmAllowServiceLevel,
                            Algorithm = agent.Algorithm
                        }
                        ).ToList();

                    csvFilePath = CreateCsvReport<AgentReportViewModel>(report, agentsForReport);
                    break;

                case "SkillReport":
                    //var skillsForReport = await skillRepository.GetSkillsAsync();
                    var skills = skillRepository.GetSkillsFromSqlQuery();
                    List<SkillReportViewModel> skillsForReport = skills
                        .Select(skill => new SkillReportViewModel
                        {
                            Id = skill.Id,
                            Key = skill.Key,
                            Name = skill.Name,
                            Algorithm = skill.Algorithm
                        }
                        ).ToList();
                    csvFilePath = CreateCsvReport<SkillReportViewModel>(report, skillsForReport);
                    break;
                default:
                    break;                    
            }
            return csvFilePath;
        }

        private string CreateCsvReport<T>(Report report, List<T> reportData)
        {
            string pathCsvFile = $@"C:\Users\Edegt\AppData\Roaming\ControlPanel\{report.Name}.csv";
            CsvConfiguration configuration = new CsvConfiguration(CultureInfo.CurrentCulture);
            configuration.Encoding = Encoding.GetEncoding(1251);
            using (StreamWriter streamWriter = new StreamWriter(pathCsvFile))
            {
                using (CsvWriter csvWriter = new CsvWriter(streamWriter, configuration))
                {
                    csvWriter.Configuration.Delimiter = ";";
                    csvWriter.WriteRecords(reportData);
                }
            }
            return pathCsvFile;
        }
    }
}