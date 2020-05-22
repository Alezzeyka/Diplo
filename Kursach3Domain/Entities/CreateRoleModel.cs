using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
namespace Kursach3Domain.Entities
{
    public class CreateRoleModel
    {
        [Display(Name ="Название")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
    }
}
