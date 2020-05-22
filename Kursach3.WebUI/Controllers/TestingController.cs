using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kursach3Domain.Abstract;
using Kursach3Domain.Entities;
using Kursach3Domain.Concrete;
using Kursach3.WebUI.Models;
using System.Data.Entity;
using System.Threading;
namespace Kursach3.WebUI.Controllers
{
    [Authorize(Roles = "Пользователь")]
    public class TestingController : Controller
    {
        public int pageSize = 1;
        private ITestRepository test;
        private IQuestionRepository question;
        private IAnswersRepository answers;
        private IPicturesRepository Pictures;
        public TestingController(ITestRepository testRepository,IQuestionRepository questionRepository,IAnswersRepository answersRepository, IPicturesRepository picturesRepository)
        {
            test = testRepository;
            question = questionRepository;
            answers = answersRepository;
            Pictures = picturesRepository;
        }
        public ViewResult Testing(int TestID,int page=1)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<EFTestDbContext>());
            TestPreview Test = test.Tests.First(x => x.Id==TestID);
            List<Answers> list = new List<Answers>();
            List<Picture> list1 = new List<Picture>();
            QuestionListviewModel model = new QuestionListviewModel
            {
                Test = Test,
                Questions = question.Question
                .Where(x => x.TestID == TestID)
                .OrderBy(x=>x.Id),
            };
            foreach(var a in model.Questions)
            {
                list.AddRange(answers.Answers.Where(x => x.QuestionID == a.Id));
                if (a.ImgId != 0)
                {
                    list1.Add(Pictures.Pictures.First(x => x.Id == a.ImgId));
                }
            }
            model.Answers = list;
            model.Questions=model.Questions.ToList().Shuffle();
            model.Answers = model.Answers.ToList().Shuffle();
            model.pictures = list1;
            return View(model);
        }
        public ViewResult ZNOTesting(int TestID)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<EFTestDbContext>());
            TestPreview TempTest = test.Tests.First(x => x.Id == TestID);
            IEnumerable<Question> questions = question.Question.Where(x => x.TestID == TestID);
            TestComplitingViewModel model = new TestComplitingViewModel();
            model.Tests = TempTest;
            model.Questions = questions;
            model.Answers = answers.Answers;
            return View(model);
        }
        public ActionResult TestResult(int Id)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<EFTestDbContext>());
            TestResultViewModel model = new TestResultViewModel
            {
                score = 0,
                ResultStrings = new List<TestCountModel>()
            };
            if (Id!=0)
            {
                 List<Answers> list = new List<Answers>();
                 List<Picture> pictures = new List<Picture>();
                 model.Test = test.Tests.First(x => x.Id == Id);
                 model.Questions = question.Question.Where(x => x.TestID == Id);
                 foreach (var a in model.Questions)
                 {
                     list.AddRange(answers.Answers.Where(x => x.QuestionID == a.Id));
                     if (a.ImgId != 0)
                     {
                         pictures.Add(Pictures.Pictures.First(x => x.Id == a.ImgId));
                     }
                 }
                 model.Answers = list;
                 model.Pictures = pictures;
                for (int i=1;i<=model.Test.NumOfQ;i++)
                {
                    var result = Request["answer" + i];
                    if(result!=null)
                    {
                        string[] answer = result.Split(':');
                        TestCountModel test = new TestCountModel
                        {
                            IsCorrect = bool.Parse(answer[0]),
                            AnswerId = int.Parse(answer[1]),
                            QuestionId = int.Parse(answer[2]),
                            TestId = int.Parse(answer[3]),
                            Score = int.Parse(answer[4])
                        };
                        model.ResultStrings.Add(test);
                    }
                }
                foreach(var a in model.ResultStrings)
                {
                    if(a.IsCorrect==true)
                    {
                        model.score += a.Score;
                    }
                }
                model.Questions = model.Questions.ToList().Shuffle();
                model.Answers = model.Answers.ToList().Shuffle();
                return View(model);
            }
            else
            {
                TempData["error"] = string.Format("Произошла ошибка с тестом, вы возвращены к списку вам доступных");
                return RedirectToAction("List","Test");
            }
        }
    }
    public static class ThreadSafeRandom
    {
        [ThreadStatic] private static Random Local;

        public static Random ThisThreadsRandom
        {
            get { return Local ?? (Local = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
        }
    }
    public static class MyExtensions
    {
        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return (list);
        }
    }
}