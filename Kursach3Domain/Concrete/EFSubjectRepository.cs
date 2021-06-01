using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kursach3Domain.Entities;
using Kursach3Domain.Abstract;

namespace Kursach3Domain.Concrete
{
    public class EFSubjectRepository : ISubjectRepository
    {
        EFTestDbContext context = new EFTestDbContext();

        public IEnumerable<Subject> _subjects
        {
            get { return context.Subjects; }
        }
        public void SaveSubject(Subject subject)
        {
            if (subject.Id == 0)
                context.Subjects.Add(subject);
            else
            {
                Subject dbEntry = context.Subjects.Find(subject.Id);
                if (dbEntry != null)
                {
                    dbEntry.SubjectName = subject.SubjectName;
                }
            }
            context.SaveChanges();
        }
        public Subject DeleteSubject(int ID)
        {
            Subject dbEntry = context.Subjects.Find(ID);
            if (dbEntry != null)
            {
                context.Subjects.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
        public void DeleteSubjects(IEnumerable<Subject> subjects)
        {

            context.Subjects.RemoveRange(subjects);
            context.SaveChanges();

            return;
        }
    }
}
