using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kursach3.WebUI.Models
{
    public class TestCountModel
    {
        public bool IsCorrect { get; set; }
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public int TestId { get; set; }
        public int Score { get; set; }
        public bool Line { get; set; }
    }
}