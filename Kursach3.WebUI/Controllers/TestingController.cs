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

namespace Kursach3.WebUI.Controllers
{
    [Authorize]
    public class TestingController : Controller
    {
        // GET: Testing
        private ITestRepository test;
        private IQuestionRepository question;
        private IAnswersRepository answers;
        public TestingController(ITestRepository testRepository,IQuestionRepository questionRepository,IAnswersRepository answersRepository)
        {
            test = testRepository;
            question = questionRepository;
            answers = answersRepository;
        }
        [ValidateInput(false)]
        public ViewResult Testing(int TestID)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<EFTestDbContext>());
            TestPreview TempTest=test.Tests.First(x=>x.Id==TestID);
            IEnumerable<Question> questions = question.Question.Where(x => x.TestID == TestID);
            TestComplitingViewModel model = new TestComplitingViewModel();
            model.Tests = TempTest;
            model.Questions = questions;
            model.Answers = answers.Answers;
            return View(model);
        }
        [ValidateInput(false)]
        public ViewResult TestResult()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<EFTestDbContext>());
            ViewBag.Error = "";
            if (Request["answer" + 1] != null)
            {
                int CurTest = 0;
                int i = 2;
                var result = Request["answer" + 1];
                int LastLength = 0;
                int NewLength = 0;
                do
                {
                    LastLength = result.Length;
                    if (i == 2) result += ",";
                    result += Request.Unvalidated["answer" + (i)];
                    NewLength = result.Length;
                    if (NewLength > LastLength)
                        result += ",";
                    i++;
                }
                while (NewLength > LastLength);
                result = result.Remove(result.Length - 1, 1);
                string[] answer = result.Split(',');
                int NumOfQuestions = answer.Length;
                i = 0;
                foreach (string a in answer)
                {
                    string[] b = a.Split(':');
                    if (b[1] == "yes")
                    {
                        i++;
                        //SelAnswers.Add(answers.Answers.First(x => x.Id == Convert.ToInt32(b[2])));
                    }
                    CurTest = int.Parse(b[4]);

                }
                TestResultViewModel model = new TestResultViewModel();
                TestPreview MyTest = test.Tests.First(x => x.Id == CurTest);
                model.Answers = answers.Answers;
                model.Questions = question.Question;
                model.ResultStrings = answer;
                ViewBag.Result = result + "NumOfQ=" + NumOfQuestions + "NumOfCorr=" + i + "TestID=" + CurTest;
                ViewBag.Questions = NumOfQuestions;
                ViewBag.Correct = i;
                ViewBag.TestTheme = MyTest.Theme;
                ViewBag.TestID = CurTest;
                return View(model);
            }
            else
            {
                ViewBag.Error = "Error";
                return View(new TestResultViewModel());
                
            }
        }
    }
}