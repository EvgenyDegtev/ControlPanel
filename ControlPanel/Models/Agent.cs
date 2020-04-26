using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ControlPanel.Models
{
    public class Agent
    {
        public int Id { get; set; }

        [Display(Name="Имя")]
        public string Name { get; set; }

        [Display(Name="Логин")]
        public string Login { get; set; }

        [Display(Name="Алгоритм")]
        public int Algorithm { get; set; }

        [Display(Name="Желаемый уровень обслуживания")]
        public bool IsAlgorithmAllowServiceLevel { get; set; }

        [Display(Name ="Максимальная нагрузка")]
        public int WorkloadMaxContactsCount { get; set; }

        //Other properties

        [Display(Name ="Активен")]
        public bool IsActive { get; set; }

        public int? GroupId { get; set; }

        public Group Group { get; set; }
    }
}