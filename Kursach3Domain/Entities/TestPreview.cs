using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Kursach3Domain.Entities
{
    public class TestPreview
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Display(Name = "Тема")]
        [Required(ErrorMessage = "Пожалуйста, введите тему теста")]
        public string Theme { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Пожалуйста, введите описание теста")]
        public string Description { get; set; }
        [Display(Name = "Категория")]
        [Required(ErrorMessage = "Пожалуйста, введите категорию теста")]
        public string Category { get; set; }
        [Display(Name = "Класс")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Пожалуйста, введите положительное значение для класса")]
        public int Course { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
    }
}
