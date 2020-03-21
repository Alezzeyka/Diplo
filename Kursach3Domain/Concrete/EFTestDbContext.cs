using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kursach3Domain.Entities;
using System.Data.Entity;

namespace Kursach3Domain.Concrete
{
    public class EFTestDbContext : DbContext
    {
        public EFTestDbContext() :base("MyTest")
        {
           
        }
        public DbSet<TestPreview> Tests { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Answers> Answers { get; set; }
    }
}