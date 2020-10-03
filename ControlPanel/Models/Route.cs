using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ControlPanel.Models
{
    public class Route
    {
        public int Id { get; set; }

        [Display(Name="Название")]
        [Required(ErrorMessage = "Требуется поле Название")]
        [MaxLength(100,ErrorMessage = "Максимальная длина - 100 символов")]
        public string Name { get; set; }

        [Display(Name ="Key")]
        [Required(ErrorMessage = "Требуется поле ID")]
        [MaxLength(100,ErrorMessage = "Максимальная длина - 100 символов")]
        [Remote(action: "CheckKeyUnique", controller: "Routes", AdditionalFields = nameof(Id))]
        public string Key { get; set; }

        //[Display(Name = "Активен")]
        //public bool IsActive { get; set; }

        [Display(Name="Навык")]
        public int? SkillId { get; set; }

        public Skill Skill { get; set; }
    }
}