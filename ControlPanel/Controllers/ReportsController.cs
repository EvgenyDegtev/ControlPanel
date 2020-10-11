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

namespace ControlPanel.Controllers
{
    public class ReportsController : Controller
    {
        CsvHelper.Configuration.Configuration configuration = new Configuration();
        IAgentRepository agentRpository;
        ISkillRepository skillRepository;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ReportsController(IAgentRepository agentRepository)
        {
            this.agentRpository = agentRepository;
        }

        // GET: Reports
        public ActionResult Index()
        {
            string[] reports = new string[] { "AgentReport", "Skillreport" };
            ViewBag.Reports = new SelectList(reports);
            return View();
        }

        [HttpPost]
        public async Task<FileResult> GetReport([Bind] Report report)
        {
            var agentsForReport = await agentRpository.GetAgentsAsync();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            byte[] reportByteStream;
            using(var memoryStream=new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, agentsForReport);
                reportByteStream = memoryStream.ToArray();
            }

            string reportString = report.Name.ToString()+";"+report.DateFrom.ToString() + ";" + report.DateTo.ToString();
            Encoding encoding = Encoding.ASCII;
            byte[] reportStream = encoding.GetBytes(reportString);
            string file_type = "application/txt";
            string file_name = "1.txt";
            return File(reportByteStream, file_type, file_name);
        }
    }
}