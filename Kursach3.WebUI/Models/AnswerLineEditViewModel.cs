﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kursach3Domain.Entities;
namespace Kursach3.WebUI.Models
{
    public class AnswerLineEditViewModel
    {
        public MultiChoice Lines { get; set; }
        public int QuestionID { get; set; }
    }
}