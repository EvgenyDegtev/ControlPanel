using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControlPanel.Models;

namespace ControlPanel.ViewModels
{
    public class RouteCreateViewModel
    {
        public Route Route { get; set; }
        
        public SelectList Skills { get; set; }
    }
}