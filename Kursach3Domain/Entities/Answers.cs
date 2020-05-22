using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Kursach3Domain.Entities
{
    public class Answers
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int QuestionID { get; set; }
        [Required]
        [Display(Name = "Форма ответа")]
        public string AnswerForm { get; set; }
        [Required]
        [Display(Name = "Правильность")]
        public bool IsCorrect { get; set; }
        [Display(Name ="Код изображения")]
        [HiddenInput(DisplayValue = false)]
        public int ImgId { get; set; }
        [Display(Name ="Баллы за ответ (только для правильных)")]
        public int AnswerScore { get; set; }
    }
}
