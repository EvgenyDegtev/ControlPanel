using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ControlPanel.Models
{
    public class Skill
    {
        public int Id { get; set; }

        [Display(Name ="Название")]
        public string Name { get; set; }

        [Display(Name ="ID")]
        public string Key { get; set; }

        [Display(Name ="Алгоритм")]
        public int Algorithm { get; set; }

        [Display(Name ="Активен")]
        public bool IsActive { get; set; }

        public List<Route> Routes { get; set; }
    }
}