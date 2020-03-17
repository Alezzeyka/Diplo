using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kursach3Domain.Entities;
using Kursach3Domain.Abstract;

namespace Kursach3Domain.Concrete
{
    public class EFQuestionRepository : IQuestionRepository
    {
        EFTestDbContext context = new EFTestDbContext();

        public IEnumerable<Question> Question
        {
            get { return context.Question; }
        }
        public void SaveQuestion(Question quest)
        {
            if (quest.Id == 0)
                context.Question.Add(quest);
            else
            {
                Question dbEntry = context.Question.Find(quest.Id);
                if (dbEntry != null)
                {
                    dbEntry.QuestionForm = quest.QuestionForm;
                    dbEntry.TestID = quest.TestID;
                }
            }
            context.SaveChanges();
        }
        public Question DeleteQuest(int ID)
        {
            Question dbEntry = context.Question.Find(ID);
            if (dbEntry != null)
            {
                context.Question.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
