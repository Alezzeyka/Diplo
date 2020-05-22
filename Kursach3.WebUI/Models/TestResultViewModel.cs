using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kursach3Domain.Entities;
namespace Kursach3.WebUI.Models
{
    public class TestResultViewModel
    { 
        public TestPreview Test { get; set; }
        public IEnumerable<Question> Questions { get; set; }
        public IEnumerable<Answers> Answers { get; set; }
        public List<TestCountModel> ResultStrings { get; set; }
        public IEnumerable<Picture> Pictures { get; set; }
        public int score { get; set; }
    }
}