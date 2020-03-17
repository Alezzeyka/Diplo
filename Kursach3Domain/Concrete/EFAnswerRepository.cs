using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kursach3Domain.Entities;
using Kursach3Domain.Abstract;

namespace Kursach3Domain.Concrete
{
    public class EFAnswerRepository : IAnswersRepository
    {
        EFTestDbContext context = new EFTestDbContext();

        public IEnumerable<Answers> Answers
        {
            get { return context.Answers; }
        }
        public void SaveAnswer(Answers answers)
        {
            if (answers.Id == 0)
                context.Answers.Add(answers);
            else
            {
                Answers dbEntry = context.Answers.Find(answers.Id);
                if (dbEntry != null)
                {
                    dbEntry.AnswerForm = answers.AnswerForm;
                    dbEntry.IsCorrect = answers.IsCorrect;
                    dbEntry.QuestionID = answers.QuestionID;
                }
            }
            context.SaveChanges();
        }
        public Answers DeleteAnswer(int ID)
        {
            Answers dbEntry = context.Answers.Find(ID);
            if (dbEntry != null)
            {
                context.Answers.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
