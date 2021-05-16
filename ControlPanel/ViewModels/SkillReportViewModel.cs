using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlPanel.ViewModels
{
    public class SkillReportViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }

        public int Algorithm { get; set; }

        public static Dictionary<int, string> algorithmDictionary = new Dictionary<int, string>
        {
            [0] = "ucd-mia",
            [1] = "ucd-loa",
            [2] = "ead-mia",
            [3] = "ead-loa",
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