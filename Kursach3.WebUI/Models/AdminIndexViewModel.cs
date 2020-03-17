using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kursach3Domain.Entities;
using Kursach3Domain.Abstract;

namespace Kursach3.WebUI.Models
{
    public class AdminIndexViewModel
    {
        public IEnumerable<TestPreview> Tests { get; set; }
    }
}