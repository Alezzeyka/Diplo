using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Kursach3Domain.Entities
{
    public class Question
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int TestID { get; set; }
        [Display(Name = "Форма вопроса")]
        [Required(ErrorMessage = "Пожалуйста, введите форму вопроса")]
        public string QuestionForm { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int ImgId { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int NumOfCorrectAnswers { get; set; }
        [Display(Name = "Вопрос на сопоставление?")]
        public bool MultiChoice { get; set; }
        public int Score { get; set; }
    }
}
