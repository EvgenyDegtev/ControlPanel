using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControlPanel.Models;
using System.ComponentModel.DataAnnotations;

namespace ControlPanel.ViewModels
{
    public class AgentCreateViewModel
    {
        public Agent Agent { get; set; }

        public SelectList Groups { get; set; }

        public SelectList Algorithms { get; set; }
    }
}