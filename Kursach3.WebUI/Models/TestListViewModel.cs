using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kursach3Domain.Entities;

namespace Kursach3.WebUI.Models
{
    public class TestListViewModel
    {
        public IEnumerable<TestPreview> Tests { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
        public IEnumerable<Picture> pictures { get; set; }
    }
}