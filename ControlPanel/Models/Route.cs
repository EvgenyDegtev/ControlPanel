using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ControlPanel.Models
{
    public class Route
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Key { get; set; }

        public bool IsActive { get; set; }

        public int? SkillId { get; set; }

        public Skill Skill { get; set; }
    }
}