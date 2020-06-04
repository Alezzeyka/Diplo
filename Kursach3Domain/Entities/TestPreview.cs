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
        [Display(Name = "Тема, Будет учитываться только для обычных тестов")]
        [Required(ErrorMessage = "Пожалуйста, введите тему теста")]
        public string Theme { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание, Будет учитываться только для обычных тестов")]
        [Required(ErrorMessage = "Пожалуйста, введите описание теста")]
        public string Description { get; set; }
        [Display(Name = "Сессия ЗНО (Только для тестов в формате ЗНО, для обычных тестов будет выбрано автоматически \"История украины\")")]
        [Required(ErrorMessage = "Пожалуйста, введите категорию теста")]
        public string Category { get; set; }
        [Display(Name = "Класс или Год")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Пожалуйста, введите положительное значение для класса или года")]
        public int Course { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int ImgId { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int NumOfQ { get; set; }
        [Display(Name = "Тест в ЗНО формате?")]
        public bool ZNO { get; set; }
        public int MaxScore { get; set; }
    }
}
