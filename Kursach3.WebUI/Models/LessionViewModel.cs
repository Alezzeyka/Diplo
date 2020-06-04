using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kursach3Domain.Entities;

namespace Kursach3.WebUI.Models
{
    public class LessionViewModel
    {
        public Lessions Lession { get; set; }
        public IEnumerable<Chapter> Chapters {get;set;}
        public IEnumerable<Picture> Pictures { get; set; }
        public IEnumerable<Files> Files { get; set; }
    }
}