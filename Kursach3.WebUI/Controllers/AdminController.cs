using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kursach3Domain.Abstract;
using Kursach3Domain.Entities;
using Kursach3.WebUI.Models;
namespace Kursach3.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        // GET: Admin
        ITestRepository TestRepository;
        IQuestionRepository QuestionRepository;
        IAnswersRepository AnswersRepository;
        public AdminController(ITestRepository testRepository, IQuestionRepository questionRepository, IAnswersRepository answersRepository)
        {
            TestRepository = testRepository;
            QuestionRepository = questionRepository;
            AnswersRepository = answersRepository;
        }
        public ActionResult Index()
        {
            AdminIndexViewModel adminIndexViewModel = new AdminIndexViewModel();
            adminIndexViewModel.Tests = TestRepository.Tests;
            return View(adminIndexViewModel);
        }
        public ActionResult QuestionIndex(int? ID)
        {
            if (ID != null)
            {
                TestPreview test = TestRepository.Tests.First(x => x.Id==ID);
                QuestionIndexViewModel questions = new QuestionIndexViewModel();
                questions.Questions = QuestionRepository.Question.Where(x => x.TestID == ID);
                questions.TestID = test.Id;
                return View(questions);
            }
            else 
            {
                QuestionIndexViewModel questions = new QuestionIndexViewModel();
                questions.Questions = QuestionRepository.Question;
                return View(questions);
            }
        }
        public ActionResult AnswerIndex(int? ID)
        {
            if (ID != null)
            {
                Question question = QuestionRepository.Question.First(x => x.Id == ID);
                AnswersIndexViewModel answers = new AnswersIndexViewModel();
                answers.Answers = AnswersRepository.Answers.Where(x => x.QuestionID == ID);
                answers.QuestionID = question.Id;
                return View(answers);
            }
            else 
            {
                AnswersIndexViewModel answers = new AnswersIndexViewModel();
                answers.Answers = AnswersRepository.Answers;
                return View(answers);
            }
        }
        [ValidateInput(false)]
        public ActionResult Edit(int? ID,int? Delete)
        {
            if (Delete == null)
            {
                TestPreview test = TestRepository.Tests.First(x => x.Id == ID);
                return View(test);
            }
            else
            {
                TestPreview test = TestRepository.Tests.First(x => x.Id == ID);
                TestPreview deletedTest = TestRepository.DeleteTest(test.Id);
                if (deletedTest != null)
                {
                    TempData["message"] = string.Format("Тест \"{0}\" был удален",
                        deletedTest.Theme);
                }
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(TestPreview test, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if(image!=null)
                {
                    test.ImageMimeType = image.ContentType;
                    test.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(test.ImageData, 0, image.ContentLength);
                }
                TestRepository.SaveTest(test);
                TempData["message"] = string.Format("Изменения в тесте \"{0}\" были сохранены", test.Theme);
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(test);
            }
        }
        [ValidateInput(false)]
        public ActionResult EditQuestion(int? ID,int? Delete)
        {
            if (Delete == null)
            {
                Question question = QuestionRepository.Question.First(x => x.Id == ID);
                return View(question);
            }
            else
            {
                Question question = QuestionRepository.Question.First(x => x.Id == ID);
                Question deletedQuestion = QuestionRepository.DeleteQuest(question.Id);
                if (deletedQuestion != null)
                {
                    TempData["message"] = string.Format("Вопрос был удален");
                }
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditQuestion(Question question)
        {
            if (ModelState.IsValid)
            {
                QuestionRepository.SaveQuestion(question);
                TempData["message"] = string.Format("Изменения в вопросе были сохранены");
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(question);
            }
        }
        [ValidateInput(false)]
        public ActionResult EditAnswer(int? ID,int? Delete)
        {
            if (Delete == null)
            {
                Answers answer = AnswersRepository.Answers.First(x => x.Id == ID);
                return View(answer);
            }
            else 
            {
                Answers answers = AnswersRepository.Answers.First(x => x.Id == ID);
                Answers deletedAnswer = AnswersRepository.DeleteAnswer(answers.Id);
                if (deletedAnswer != null)
                {
                    TempData["message"] = string.Format("Ответ был удален");
                }
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditAnswer(Answers answer)
        {
            if (ModelState.IsValid)
            {
                AnswersRepository.SaveAnswer(answer);
                TempData["message"] = string.Format("Изменения в ответе были сохранены");
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(answer);
            }
        }
        [HttpGet]
        public ViewResult Create()
        {
            return View("Edit", new TestPreview());
        }
        [HttpGet]
        public ViewResult CreateQuestion(int TestID)
        {
            
            return View("EditQuestion", new Question { TestID=TestID});
        }
        [HttpGet]
        public ViewResult CreateAnswer(int QuestionID)
        {
        
            return View("EditAnswer", new Answers { QuestionID=QuestionID});
        }
    }
}