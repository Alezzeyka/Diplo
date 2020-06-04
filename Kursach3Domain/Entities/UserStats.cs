using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Kursach3Domain.Entities
{
    public class UserStats
    {
        [Key]
        public int Id { get; set; }
        public string userId { get; set; }
        public int testId { get; set; }
        public string TestTheme { get; set; }
        public int score { get; set; }
        public int maxScore { get; set; }
        public bool ZNO { get; set; }
        public DateTime Date { get; set; }
    }
}
