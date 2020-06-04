using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kursach3Domain.Entities;

namespace Kursach3.WebUI.Models
{
    public class ChapterSummaryViewModel
    {
        public Chapter Chapter { get; set; }
        public Files Video { get; set; }
        public Files PDF { get; set; }
        public Picture Picture { get; set; }
    }
}