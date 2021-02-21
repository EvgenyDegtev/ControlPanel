using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ControlPanel.Models;

namespace ControlPanel.ViewModels
{
    public class AgentSkillsViewModel
    {
        public int? AgentId { get; set; }

        public List<AgentToSkill> AgentToSkills { get; set; }
    }
}