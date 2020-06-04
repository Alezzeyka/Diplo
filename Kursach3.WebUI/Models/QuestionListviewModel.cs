using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kursach3Domain.Entities;
namespace Kursach3.WebUI.Models
{
    public class QuestionListviewModel
    {
        public TestPreview Test { get; set; }
        public IEnumerable<Question> Questions { get; set; }
        public IEnumerable<Answers> Answers { get; set; }
        public IEnumerable<Picture> pictures { get; set; }
        public IEnumerable<MultiChoice> lines { get; set; }
        public IEnumerable<Answers> LineAnswers { get; set; }
    }
}