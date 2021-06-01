using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kursach3Domain.Entities;
using Kursach3Domain.Abstract;

namespace Kursach3Domain.Concrete
{
    public class EFCourseRepository : ICourseRepository
    {
        EFTestDbContext context = new EFTestDbContext();

        public IEnumerable<Course> _Courses
        {
            get { return context.Courses; }
        }
        public void SaveCourse(Course course)
        {
            if (course.Id == 0)
                context.Courses.Add(course);
            else
            {
                Course dbEntry = context.Courses.Find(course.Id);
                if (dbEntry != null)
                {
                    dbEntry.CourseNum = course.CourseNum;
                }
            }
            context.SaveChanges();
        }
        public Course DeleteCourse(int ID)
        {
            Course dbEntry = context.Courses.Find(ID);
            if (dbEntry != null)
            {
                context.Courses.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
        public void DeleteCourses(IEnumerable<Course> courses)
        {

            context.Courses.RemoveRange(courses);
            context.SaveChanges();

            return;
        }
    }
}
