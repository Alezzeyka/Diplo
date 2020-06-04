using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kursach3Domain.Entities;

namespace Kursach3.WebUI.Models
{
    public class ChapterIndexViewModel
    {
        public int LessionId { get; set; }
        public IEnumerable<Chapter> Chapters { get; set; }
    }
}