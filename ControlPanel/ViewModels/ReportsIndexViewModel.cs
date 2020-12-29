using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControlPanel.Models;

namespace ControlPanel.ViewModels
{
    public class ReportsIndexViewModel
    {
        public Report Report { get; set; }
        public SelectList ReportNames { get; set; }
    }
}