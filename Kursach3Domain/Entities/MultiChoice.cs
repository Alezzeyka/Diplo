using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Kursach3Domain.Entities
{
    public class MultiChoice
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="Идентификатор линии")]
        [Required(ErrorMessage ="Неподходящее значение")]
        public string LineString { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int QuestionID { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int LineScore { get; set; }
        [HiddenInput(DisplayValue =false)]
        public int NumOfCorrectAnswers { get; set; }
    }
}
