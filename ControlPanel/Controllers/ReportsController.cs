﻿using System;
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
using ControlPanel.ViewModels;

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
        public async Task<FileResult> GetReport([Bind] Report report)
        {
            string csvFilePath=null;
            switch (report.Name)
            {
                case "AgentReport":
                    var agentsForReport = await agentRpository.GetAgentsAsync();
                    csvFilePath=CreateCsvReport<Agent>(report, agentsForReport);
                    break;

                case "SkillReport":
                    //var skillsForReport = await skillRepository.GetSkillsAsync();
                    var skillsForReport = skillRepository.GetSkillsFromSqlQuery();
                    csvFilePath = CreateCsvReport<Skill>(report, skillsForReport);
                    break;
                default:
                    break;
            }

            string fileType = "application/csv";
            string fileName = $"{report.Name}_{report.DateFrom}_{report.DateTo}.csv";
            return File(csvFilePath, fileType, fileName);
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