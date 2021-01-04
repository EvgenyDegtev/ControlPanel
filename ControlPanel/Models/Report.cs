using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ControlPanel.Models
{
    public class Report
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Требуется поле Логин")]
        [Display(Name = "Отчет")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Требуется начальный период")]
        [Display(Name ="C:")]        
        public string DateFrom { get; set; }

        [Required(ErrorMessage = "Требуется конечный период")]
        [Display(Name = "По:")]        
        public string DateTo { get; set; }
    }
}