using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlPanel.Models
{
    public class Report
    {
        public int Id { get; set; }

        public string Name { get; set; } 

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }
    }
}