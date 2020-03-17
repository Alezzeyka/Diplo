using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kursach3Domain.Entities;
namespace Kursach3.WebUI.Models
{
    public class TestResultViewModel
    { 
        public IEnumerable<Question> Questions { get; set; }
        public IEnumerable<Answers> Answers { get; set; }
        public string[] ResultStrings { get; set; }
    }
}