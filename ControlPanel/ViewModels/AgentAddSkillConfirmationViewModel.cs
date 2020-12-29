using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControlPanel.Models;

namespace ControlPanel.ViewModels
{
    public class AgentAddSkillConfirmationViewModel
    {
        public AgentToSkill AgentToSkill { get; set; }
        public string SkillName { get; set; }

        public SelectList Levels { get; set; }

        public SelectList Modes { get; set; }
    }
}