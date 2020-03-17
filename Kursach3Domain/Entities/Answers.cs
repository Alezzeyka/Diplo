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
        [Display(Name = "Форма ответа")]
        
        public string AnswerForm { get; set; }
        [Display(Name = "Правильность")]
        
        public string IsCorrect { get; set; }
    }
}
