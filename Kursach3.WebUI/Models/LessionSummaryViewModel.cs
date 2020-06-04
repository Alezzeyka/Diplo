using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kursach3Domain.Entities;

namespace Kursach3.WebUI.Models
{
    public class LessionSummaryViewModel
    {
        public Lessions Lession { get; set; }
        public Picture Picture { get; set; }
    }
}