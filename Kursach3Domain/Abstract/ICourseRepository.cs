using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kursach3Domain.Entities;

namespace Kursach3Domain.Abstract
{
    public interface ICourseRepository
    {
        IEnumerable<Course> _Courses { get; }
        void SaveCourse(Course course);
        Course DeleteCourse(int ID);
        void DeleteCourses(IEnumerable<Course> courses);
    }
}