using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kursach3Domain.Entities;
namespace Kursach3.WebUI.Models
{
    public class QuestionSummaryViewModel
    {
        public Question Question { get; set; }
        public IEnumerable<Answers> Answers { get; set; }
        public Picture Picture { get; set; }
    }
}