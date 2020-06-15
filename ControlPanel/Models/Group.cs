using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ControlPanel.Models
{
    public class Group
    {
        public int Id { get; set; }

        [Display(Name ="Группа")]
        [Required(ErrorMessage = "Требуется поле Группа")]
        [MaxLength(100,ErrorMessage = "Максимальная длина - 100 символов")]
        [Remote(action: "CheckNameUnique", controller: "Groups", AdditionalFields = nameof(Id))]
        public string Name { get; set; }

        [Display(Name="Комментарий")]
        [MaxLength(5000,ErrorMessage = "Максимальная длина - 5000 символов")]
        public string Description { get; set; }

        [Display(Name="Активен")]
        public bool IsActive { get; set; }

        public List<Agent> Agents { get; set; }
    }
}