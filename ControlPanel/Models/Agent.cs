using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlPanel.Models
{
    public class Agent
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public int Algorithm { get; set; }

        public bool IsAlgorithmAllowServiceLevel { get; set; }

        public int WorkloadMaxContactsCount { get; set; }

        //Other properties

        public bool IsActive { get; set; }
    }
}