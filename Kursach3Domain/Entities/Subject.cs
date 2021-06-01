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
    public class Subject
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Display(Name = "Название предмета")]
        [Required(ErrorMessage = "Пожалуйста, введите название предмета")]
        public int SubjectName { get; set; }
    }
}
