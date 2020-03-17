using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kursach3Domain.Entities;

namespace Kursach3.WebUI.Models
{
    public class AnswersIndexViewModel
    {
        public IEnumerable<Answers> Answers{get;set;}
        public int QuestionID { get; set; }
    }
}