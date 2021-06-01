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
        public EFTestDbContext() :base("CybertestDB")
        {
           
        }
        public DbSet<TestPreview> Tests { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Answers> Answers { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<UserStats> Stats { get; set; }
        public DbSet<Lessions> Lessions { get; set; }
        public DbSet<MultiChoice> Lines { get; set; }
        public DbSet<Files> Files { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Course> Courses { get; set; }
    }
}