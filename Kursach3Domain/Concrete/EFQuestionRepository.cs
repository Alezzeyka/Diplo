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
                    dbEntry.NumOfCorrectAnswers = quest.NumOfCorrectAnswers;
                    dbEntry.ImgId = quest.ImgId;
                    dbEntry.MultiChoice = quest.MultiChoice;
                    dbEntry.Score = quest.Score;
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
        public void DeleteQuests(IEnumerable<Question> questions)
        {
                context.Question.RemoveRange(questions);
                context.SaveChanges();
        }
        public void SaveQuestions(IEnumerable<Question> questions)
        {
            foreach (var a in questions)
            {
                Question dbEntry = context.Question.Find(a.Id);
                {
                    dbEntry.QuestionForm = a.QuestionForm;
                    dbEntry.TestID = a.TestID;
                    dbEntry.NumOfCorrectAnswers = a.NumOfCorrectAnswers;
                    dbEntry.ImgId = a.ImgId;
                    dbEntry.MultiChoice = a.MultiChoice;
                    dbEntry.Score = a.Score;
                }
            }
            context.SaveChanges();
        }
    }
}
