using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Kursach3Domain.Entities
{
    public class Files
    {
        [Key]
        public int Id { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        [Required(ErrorMessage = "Пожалуйста, введите название файла")]
        [Display(Name = "Название")]
        public string Name { get; set; }
    }
}
