using System;
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

        [Display(Name ="Навык")]
        [Required(ErrorMessage ="Требуется поле Навык")]
        [StringLength(100, ErrorMessage ="Максимальная длина - 100 символов" )]
        public string Name { get; set; }

        [Display(Name ="ID")]
        [Required(ErrorMessage ="Требутеся поле ID навыка")]
        [StringLength(100, ErrorMessage = "Максимальная длина - 100 символов")]
        [Remote(action:"CheckKeyUnique",controller:"Skills")]
        public string Key { get; set; }

        [Display(Name ="Алгоритм")]
        public int Algorithm { get; set; }

        [Display(Name ="Активен")]
        public bool IsActive { get; set; }

        public List<Route> Routes { get; set; }
    }
}