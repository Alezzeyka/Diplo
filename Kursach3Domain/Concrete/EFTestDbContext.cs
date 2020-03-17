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
            //Database.SetInitializer<EFTestDbContext>(new CreateDatabaseIfNotExists<EFTestDbContext>());
        }
        /*public static EFTestDbContext Create()
        {
            return new EFTestDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<EFTestDbContext>(null);
            base.OnModelCreating(modelBuilder);
        }*/
        public DbSet<TestPreview> Tests { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Answers> Answers { get; set; }
    }
}