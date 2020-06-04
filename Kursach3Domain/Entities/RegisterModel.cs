using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Kursach3Domain.Entities
{
    public class RegisterModel
    {
        [Required]
        [Display(Name = "Ваше ім'я")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Пароль")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}
