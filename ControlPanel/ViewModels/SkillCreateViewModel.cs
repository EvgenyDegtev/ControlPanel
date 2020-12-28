using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControlPanel.Models;

namespace ControlPanel.ViewModels
{
    public class SkillCreateViewModel
    {
        public Skill Skill { get; set; }

        public SelectList Algorithms { get; set; }
    }
}