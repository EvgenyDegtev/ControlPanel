﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ControlPanel.Models
{
    public class Skill
    {
        public int Id { get; set; }

        [Display(Name = "Навык")]
        [Required(ErrorMessage = "Требуется поле Навык")]
        [StringLength(100, ErrorMessage = "Максимальная длина - 100 символов")]
        public string Name { get; set; }

        [Display(Name = "ID")]
        [Required(ErrorMessage = "Требутеся поле ID навыка")]
        [StringLength(100, ErrorMessage = "Максимальная длина - 100 символов")]
        [Remote(action: "CheckKeyUnique", controller: "Skills", AdditionalFields = nameof(Id))]
        public string Key { get; set; }

        [Display(Name = "Алгоритм")]
        public int Algorithm { get; set; }

        public static Dictionary<int, string> algorithmDictionary = new Dictionary<int, string>
        {
            [0] = "Наиболее свободный без учета навыка",
            [1] = "Наиболее свободный с учетом навыка",
            [2] = "Наименее занятый без учета навыка",
            [3] = "Наименее занятый с учетом навыка",
        };

        public string AlgorithmName
        {
            get
            {
                return algorithmDictionary[this.Algorithm];
            }
        }


        public List<Route> Routes { get; set; }

        public List<AgentToSkill> AgentToSkills { get; set; }
    }
}