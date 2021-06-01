using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kursach3Domain.Entities
{
    public class Course
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Display(Name = "Класс")]
        [Required(ErrorMessage = "Пожалуйста, введите класс (целое число)")]
        public int CourseNum { get; set; }
    }
}
