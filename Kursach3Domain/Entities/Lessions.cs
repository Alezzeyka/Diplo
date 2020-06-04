using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Kursach3Domain.Entities
{
    public class Lessions
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Display(Name = "Тема")]
        [Required(ErrorMessage = "Пожалуйста, введите тему лекции")]
        public string Theme { get; set; }
        [Display(Name = "Класс")]
        [Required]
        [Range(1, 11, ErrorMessage = "Пожалуйста, введите подходящее значение для класса (1-11)")]
        public int Course { get; set; }
        public int ImgId { get; set; }
    }
}
