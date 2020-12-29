using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ControlPanel.Models;

namespace ControlPanel.ViewModels
{
    public class AgentAddSkillViewModel
    {
        public Agent Agent { get; set; }

        public List<Skill> Skills { get; set; }
    }
}