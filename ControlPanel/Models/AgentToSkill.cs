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

        public static Dictionary<int, string> levelDictionary = new Dictionary<int, string>
        {
            [1] = "1",
            [2] = "2",
            [3] = "3",
            [4] = "4",
            [5] = "5",
            [6] = "6",
            [7] = "7",
            [8] = "8",
            [9] = "9",
            [10]="10",
            [-1]="R1",
            [-2]="R2"
        };

        public string LevelName
        {
            get
            {
                return levelDictionary[Convert.ToInt32(this.Level)];
            }
        }

        [Display(Name ="Порядок")]
        [Required]
        public int OrderIndex { get; set; }

        [Display(Name ="Режим прерываний")]
        [Required]
        public string BreakingMode { get; set; }

        public static Dictionary<int, string> breakingModeDictionary = new Dictionary<int, string>
        {
            [1] = "Отключен",
            [2] = "Автоматический",
            [3] = "Ручной",
        };

        public string BreakingModeName
        {
            get
            {
                return breakingModeDictionary[Convert.ToInt32(this.BreakingMode)];
            }
        }

        [Display(Name ="Процент")]
        [Required]
        public int Percent { get; set; }

        public int AgentId { get; set; }

        public Agent Agent { get; set; }

        public int SkillId {get;set;}

        public Skill Skill { get; set; }
    }
}