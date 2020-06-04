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
using Kursach3.WebUI.Infrastructure.Abstract;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Net.Mail;
namespace Kursach3.WebUI.Controllers
{
    [Authorize(Roles = "Користувач")]
    public class TestingController : Controller
    {
        public int pageSize = 1;
        private ITestRepository test;
        private IQuestionRepository question;
        private IAnswersRepository answers;
        private IPicturesRepository Pictures;
        private IStatsRepository Stats;
        private IMultiChoiceRepository MultiChoiceRepository;
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
        public TestingController(ITestRepository testRepository,IQuestionRepository questionRepository,IAnswersRepository answersRepository, IPicturesRepository picturesRepository, IStatsRepository stats, IMultiChoiceRepository multiChoice)
        {
            test = testRepository;
            question = questionRepository;
            answers = answersRepository;
            Pictures = picturesRepository;
            Stats = stats;
            MultiChoiceRepository = multiChoice;
        }
        public ViewResult Testing(int TestID)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<EFTestDbContext>());
            TestPreview Test = test.Tests.First(x => x.Id==TestID);
            List<Answers> list = new List<Answers>();
            List<Answers> list2 = new List<Answers>();
            List<MultiChoice> choices = new List<MultiChoice>();
            List<Picture> list1 = new List<Picture>();
            QuestionListviewModel model = new QuestionListviewModel
            {
                Test = Test,
                Questions = question.Question
                .Where(x => x.TestID == TestID)
            };
            foreach(var a in model.Questions)
            {
                if (a.MultiChoice == false)
                {
                    list.AddRange(answers.Answers.Where(x => x.QuestionID == a.Id));
                }
                else 
                {
                    choices.AddRange(MultiChoiceRepository.Lines.Where(x => x.QuestionID == a.Id));
                }
                if (a.ImgId != 0)
                {
                    list1.Add(Pictures.Pictures.First(x => x.Id == a.ImgId));
                }
            }
            foreach(var a in choices)
            {
            list2.AddRange(answers.Answers.Where(x => x.QuestionID == a.Id));
            }
            model.Answers = list;
            model.LineAnswers = list2;
            model.lines = choices;
            model.Questions=model.Questions.ToList().Shuffle();
            model.Answers = model.Answers.ToList().Shuffle();
            model.LineAnswers = model.LineAnswers.ToList().Shuffle();
            model.pictures = list1;
            string MyHash = model.Test.Id+":";
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
                 List<MultiChoice> multiChoices = new List<MultiChoice>();
                List<Answers> Line_Answers = new List<Answers>();
                 model.Test = test.Tests.First(x => x.Id == Id);
                 model.Questions = question.Question.Where(x => x.TestID == Id);
                 foreach (var a in model.Questions)
                 {
                    if (a.MultiChoice == false)
                    {
                        list.AddRange(answers.Answers.Where(x => x.QuestionID == a.Id && x.LineAnswer==false));
                    }
                    else
                    {
                        multiChoices.AddRange(MultiChoiceRepository.Lines.Where(x => x.QuestionID == a.Id));
                    }
                     if (a.ImgId != 0)
                     {
                         pictures.Add(Pictures.Pictures.First(x => x.Id == a.ImgId));
                     }
                 }
                 model.Answers = list;
                 model.pictures = pictures;
                 model.lines = multiChoices;
                foreach(var a in model.lines)
                {
                    Line_Answers.AddRange(answers.Answers.Where(x => x.QuestionID == a.Id && x.LineAnswer == true));
                }
                model.LineAnswers = Line_Answers;
                for (int i=1;i<=model.Test.NumOfQ;i++)
                {
                    var result = Request["answer" + i];
                    if(result!=null)
                    {
                        string[] answer = result.Split(':');
                        TestCountModel test = new TestCountModel
                        {
                            AnswerId = int.Parse(answer[0]),
                            QuestionId = int.Parse(answer[1]),
                            TestId = int.Parse(answer[2]), 
                            Line=false
                        };
                        test.IsCorrect = answers.Answers.First(x => x.Id == test.AnswerId).IsCorrect;
                        test.Score = answers.Answers.First(x => x.Id == test.AnswerId).AnswerScore;
                        model.ResultStrings.Add(test);
                    }
                }
                foreach(var a in model.lines)
                {
                    var result = Request[a.LineString];
                    if (result != null)
                    {
                        string[] answer = result.Split(':');
                        TestCountModel test = new TestCountModel
                        {
                            AnswerId = int.Parse(answer[0]),
                            QuestionId = int.Parse(answer[1]),
                            TestId = int.Parse(answer[2]),
                            Line=true
                        };
                        test.IsCorrect = answers.Answers.First(x => x.Id == test.AnswerId).IsCorrect;
                        test.Score = answers.Answers.First(x => x.Id == test.AnswerId).AnswerScore;
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
                model.LineAnswers = model.LineAnswers.ToList().Shuffle();
                ApplicationUser user = UserManager.FindByEmail(User.Identity.Name);
                if (user != null)
                {
                    UserStats stats = new UserStats
                    {
                        maxScore = model.Test.MaxScore,
                        score = model.score,
                        testId = model.Test.Id,
                        userId = user.Id,
                        ZNO = model.Test.ZNO,
                        Date = DateTime.Today
                    };
                    if(model.Test.ZNO==true)
                    {
                        stats.TestTheme = "ЗНО " + model.Test.Course + " " + model.Test.Category;
                    }
                    else
                    {
                        stats.TestTheme = model.Test.Category + " " + model.Test.Theme + " " + model.Test.Course + " клас";
                    }
                    Stats.SaveStat(stats);
                }
                return View(model);
            }
            else
            {
                TempData["error"] = string.Format("Виникла помилка");
                return RedirectToAction("Index","Default");
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