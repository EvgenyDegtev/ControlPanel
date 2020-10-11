using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ControlPanel.Models
{
    [Serializable]
    public class Agent
    {
        public int Id { get; set; }

        
        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Требуется поле Имя")]
        [StringLength(100, ErrorMessage = "Максимальная длина - 100 символов")]
        public string Name { get; set; }

        [Display(Name = "Логин")]
        [Required(ErrorMessage ="Требуется поле Логин")]
        [StringLength(100, ErrorMessage = "Максимальная длина - 100 символов")]
        [Remote(action:"CheckLoginUnique",controller:"Agents", AdditionalFields =nameof(Id))]
        public string Login { get; set; }

        [Display(Name = "Алгоритм")]
        public int Algorithm { get; set; }

        [Display(Name = "Желаемый уровень обслуживания")]
        public bool IsAlgorithmAllowServiceLevel { get; set; }

        [Display(Name = "Максимальная нагрузка")]
        [Range(0,10,ErrorMessage ="Поле Максимальная нагрузка должно иметь значение между 0 и 10")]
        public int WorkloadMaxContactsCount { get; set; }

        //Other properties

        //[Display(Name = "Активен")]
        //public bool IsActive { get; set; }

        [Display(Name = "Группа")]
        public int? GroupId { get; set; }

        public Group Group { get; set; }

        public List<AgentToSkill> AgentToSkills { get; set; }
    }
}