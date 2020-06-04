using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kursach3Domain.Entities;

namespace Kursach3.WebUI.Models
{
    public class TestComplitingViewModel
    {
        public TestPreview Tests { get; set; }
        public IEnumerable<Answers> Answers { get; set; }
        public IEnumerable<Question> Questions { get; set; }
        public IEnumerable<MultiChoice> Lines { get; set; }
        public IEnumerable<Answers> LineAnswers { get; set; }
    }
}