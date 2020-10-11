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

        [Display(Name ="C:")]
        [DataType(DataType.DateTime, ErrorMessage ="Требуется начальный период")]
        public DateTime DateFrom { get; set; }

        [Display(Name = "По:")]
        [DataType(DataType.DateTime, ErrorMessage = "Требуется начальный период")]
        public DateTime DateTo { get; set; }
    }
}