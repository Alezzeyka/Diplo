﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kursach3Domain.Entities;

namespace Kursach3.WebUI.Models
{
    public class TestSummaryViewModel
    {
        public TestPreview Test { get; set;}
        public List<Question> questions { get; set; }
    }
}