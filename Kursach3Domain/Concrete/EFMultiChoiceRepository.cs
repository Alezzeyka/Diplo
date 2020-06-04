using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kursach3Domain.Entities;
using Kursach3Domain.Abstract;

namespace Kursach3Domain.Concrete
{
    public class EFMultiChoiceRepository : IMultiChoiceRepository
    {
        EFTestDbContext context = new EFTestDbContext();

        public IEnumerable<MultiChoice> Lines
        {
            get { return context.Lines; }
        }
        public void SaveLine(MultiChoice Line)
        {
            if (Line.Id == 0)
                context.Lines.Add(Line);
            else
            {
                MultiChoice dbEntry = context.Lines.Find(Line.Id);
                if (dbEntry != null)
                {
                    dbEntry.LineString = Line.LineString;
                    dbEntry.LineScore = Line.LineScore;
                    dbEntry.QuestionID = Line.QuestionID;
                    dbEntry.NumOfCorrectAnswers = Line.NumOfCorrectAnswers;
                }
            }
            context.SaveChanges();
        }
        public MultiChoice DeleteLine(int ID)
        { 
            MultiChoice dbEntry = context.Lines.Find(ID);
            if (dbEntry != null)
            {
                context.Lines.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
        public void DeleteLines(IEnumerable<MultiChoice> Lines)
        {

            context.Lines.RemoveRange(Lines);
            context.SaveChanges();

            return;
        }
    }
}
