using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Kursach3Domain.Entities
{
    public class Chapter
    {
        [Key]
        public int Id { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Текст абзаца")]
        [Required(ErrorMessage = "Пожалуйста, введите текст вашего урока")]
        public string Text { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int LessionId { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int ImgId { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int VideoId { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int PdfId { get; set; }
    }
}
