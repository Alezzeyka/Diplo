using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kursach3Domain.Entities;

namespace Kursach3.WebUI.Models
{
    public class SetPicIndexModel
    {
        public int TestId { get; set; }
        public int QuestId { get; set; }
        public int AnswerId { get; set; }
        public int LessId { get; set; }
        public IEnumerable<Picture> Pictures {get;set;}
    }
}