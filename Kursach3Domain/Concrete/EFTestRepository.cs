using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kursach3Domain.Entities;
using Kursach3Domain.Abstract;

namespace Kursach3Domain.Concrete
{
    public class EFTestRepository : ITestRepository
    {
        EFTestDbContext context = new EFTestDbContext();

        public IEnumerable<TestPreview> Tests
        {
            get { return context.Tests; }
        }
        public void SaveTest(TestPreview test)
        {
            if (test.Id == 0)
                context.Tests.Add(test);
            else
            {
                TestPreview dbEntry = context.Tests.Find(test.Id);
                if (dbEntry != null)
                {
                    dbEntry.Theme = test.Theme;
                    dbEntry.Description = test.Description;
                    dbEntry.Course = test.Course;
                    dbEntry.Category = test.Category;
                    dbEntry.ImageData = test.ImageData;
                    dbEntry.ImageMimeType = test.ImageMimeType;
                }
            }
            context.SaveChanges();
        }
        public TestPreview DeleteTest(int TestID)
        {
            TestPreview dbEntry = context.Tests.Find(TestID);
            if(dbEntry!=null)
            {
                context.Tests.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}