using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ControlPanel.Models
{
    public class Group
    {
        public int Id { get; set; }

        [Display(Name ="Группа")]
        public string Name { get; set; }

        [Display(Name="Комментарий")]
        public string Description { get; set; }

        [Display(Name="Активен")]
        public bool IsActive { get; set; }

        public List<Agent> Agents { get; set; }
    }
}