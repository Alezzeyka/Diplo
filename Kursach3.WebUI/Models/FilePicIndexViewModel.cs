﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kursach3Domain.Entities;

namespace Kursach3.WebUI.Models
{
    public class FilePicIndexViewModel
    {
        public int ChapterId { get; set; }
        public IEnumerable<Files> Files { get; set; }
    }
}