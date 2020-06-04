using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ControlPanel.Models
{
    public class AgentToSkill
    {
        [Required]
        public int Id { get; set; }

        [Display(Name ="Уровень")]
        [Required]
        public string Level { get; set; }

        [Display(Name ="Порядок")]
        [Required]
        public int OrderIndex { get; set; }

        [Display(Name ="Режим прерываний")]
        [Required]
        public string BreakingMode { get; set; }

        [Display(Name ="Процент")]
        [Required]
        public int Percent { get; set; }

        public int AgentId { get; set; }

        public Agent Agent { get; set; }

        public int SkillId {get;set;}

        public Skill Skill { get; set; }

        public bool IsActive { get; set; }
    }
}