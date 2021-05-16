using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ControlPanel.Models;

namespace ControlPanel.ViewModels
{
    public class AgentReportViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public bool IsAlgorithmAllowServiceLevel { get; set; }

        public int WorkloadMaxContactsCount { get; set; }

        public int Algorithm { get; set; }

        public static Dictionary<int, string> algorithmDictionary = new Dictionary<int, string>
        {
            [0] = "Max Demand",
            [1] = "Skill level",
            [2] = "Max ercent"
        };

        public string AlgorithmName
        {
            get
            {
                return algorithmDictionary[this.Algorithm];
            }
        }
    }
}