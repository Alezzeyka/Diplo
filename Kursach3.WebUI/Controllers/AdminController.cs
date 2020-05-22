using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kursach3Domain.Abstract;
using Kursach3Domain.Entities;
using Kursach3Domain.Concrete;
using Kursach3.WebUI.Models;
using System.IO;
namespace Kursach3.WebUI.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        ITestRepository TestRepository;
        IQuestionRepository QuestionRepository;
        IAnswersRepository AnswersRepository;
        IPicturesRepository PicturesRepository;
        ILessionssRepository lessionssRepository;
        public AdminController(ITestRepository testRepository, IQuestionRepository questionRepository, IAnswersRepository answersRepository, IPicturesRepository picturesRepository, ILessionssRepository lessionss)
        {
            TestRepository = testRepository;
            QuestionRepository = questionRepository;
            AnswersRepository = answersRepository;
            PicturesRepository = picturesRepository;
            lessionssRepository = lessionss;
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
                IEnumerable<Question> questions = QuestionRepository.Question.Where(x => x.TestID == ID);
                foreach(var a in questions)
                {
                    IEnumerable<Answers> answers = AnswersRepository.Answers.Where(x => x.QuestionID == a.Id);
                    AnswersRepository.DeleteAnswers(answers);
                }
                QuestionRepository.DeleteQuests(questions);
                TestPreview deletedTest = TestRepository.DeleteTest(test.Id);
                if (deletedTest != null)
                {
                    TempData["message"] = string.Format("Тест \"{0}\" был удален", deletedTest.Theme);
                }
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
 
        public ActionResult Edit(TestPreview test)
        {
            if (ModelState.IsValid)
            {
                if(test.ZNO==false)
                {
                    test.Category = "История Украины";
                }
                TestRepository.SaveTest(test);
                TempData["message"] = string.Format("Изменения в тесте \"{0}\" были сохранены", test.Theme);
                return RedirectToAction("Index");
            }
            else
            {
                
                return View(test);
            }
        }
      
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
                IEnumerable < Answers > answers = AnswersRepository.Answers.Where(x => x.QuestionID == ID);
                AnswersRepository.DeleteAnswers(answers);
                Question deletedQuestion = QuestionRepository.DeleteQuest(question.Id);
                if (deletedQuestion != null)
                {
                    TempData["message"] = string.Format("Вопрос был удален");
                }
                TestPreview test = TestRepository.Tests.First(x => x.Id == question.TestID);
                test.NumOfQ = QuestionRepository.Question.Where(x => x.TestID == test.Id).Count();
                TestRepository.SaveTest(test);
                return RedirectToAction("QuestionIndex",question.TestID);
            }
        }
        [HttpPost]
    
        public ActionResult EditQuestion(Question question)
        {
            if (ModelState.IsValid)
            {
                QuestionRepository.SaveQuestion(question);
                TempData["message"] = string.Format("Изменения в вопросе были сохранены");
                TestPreview test = TestRepository.Tests.First(x => x.Id == question.TestID);
                test.NumOfQ = QuestionRepository.Question.Where(x => x.TestID == test.Id).Count();
                TestRepository.SaveTest(test);
                return RedirectToAction("QuestionIndex", question.TestID);
            }
            else
            {
              
                return View(question);
            }
        }

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
                    CalcScore(deletedAnswer);
                    TempData["message"] = string.Format("Ответ был удален");
                }
                Question question = QuestionRepository.Question.First(x => x.Id == deletedAnswer.QuestionID);
                question.NumOfCorrectAnswers = AnswersRepository.Answers.Where(x => x.QuestionID == question.Id && x.IsCorrect==true).Count();
                QuestionRepository.SaveQuestion(question);
                return RedirectToAction("AnswerIndex",answers.QuestionID);
            }
        }
        
        [HttpPost]
        public ActionResult EditAnswer(Answers answer)
        {
            if (ModelState.IsValid)
            {
                AnswersRepository.SaveAnswer(answer);
                CalcScore(answer);
                TempData["message"] = string.Format("Изменения в ответе были сохранены");
                Question question = QuestionRepository.Question.First(x => x.Id == answer.QuestionID);
                question.NumOfCorrectAnswers = AnswersRepository.Answers.Where(x => x.QuestionID == question.Id && x.IsCorrect == true).Count();
                QuestionRepository.SaveQuestion(question);
                return RedirectToAction("AnswerIndex", answer.QuestionID);
            }
            else
            {
               
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
        public ActionResult PicIndex()
        {
            return View(PicturesRepository.Pictures);
        }

        public ActionResult CreatePic()
        {
            return View("EditPic", new Picture());
        }

        public ActionResult EditPic(int? ID, int? Delete)
        {
            if (Delete == null)
            {
                Picture test = PicturesRepository.Pictures.First(x => x.Id == ID);
                return View(test);
            }
            else
            {
                Picture test = PicturesRepository.Pictures.First(x => x.Id == ID);
                Picture deletedTest = PicturesRepository.DeletePicture(test.Id);
                if (deletedTest != null)
                {
                    TempData["message"] = string.Format("Изображение \"{0}\" было удалено", deletedTest.Name);
                }
                return RedirectToAction("PicIndex");
            }
        }
        [HttpPost]
 
        public ActionResult EditPic(Picture pic, HttpPostedFileBase uploadImage)
        {
            if (uploadImage != null)
                if (ModelState.IsValid)
                {
                    byte[] imageData = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }
                    // установка массива байтов
                    pic.Image = imageData;
                    PicturesRepository.SavePicture(pic);
                    TempData["message"] = string.Format("Изменения в изображении \"{0}\" были сохранены", pic.Name);
                    return RedirectToAction("PicIndex");
                }
                else
                {
                    return View(pic);
                }
            else
            {
                TempData["error"] = string.Format("Вы не выбрали файл");
                return RedirectToAction("PicIndex");
            }
        }
        public ActionResult SetPicIndex(int Id)
        {
            SetPicIndexModel model = new SetPicIndexModel { TestId = Id };
            model.Pictures = PicturesRepository.Pictures;
            return View(model);
        }
        public ActionResult SetAnswerPicIndex(int Id)
        {
            SetPicIndexModel model = new SetPicIndexModel { AnswerId = Id };
            model.Pictures = PicturesRepository.Pictures;
            return View(model);
        }
        public ActionResult SetQuestionPicIndex(int Id)
        {
            SetPicIndexModel model = new SetPicIndexModel { QuestId = Id };
            model.Pictures = PicturesRepository.Pictures;
            return View(model);
        }
        public ActionResult SetLessionPicIndex(int Id)
        {
            SetPicIndexModel model = new SetPicIndexModel { LessId = Id };
            model.Pictures = PicturesRepository.Pictures;
            return View(model);
        }
        public ActionResult SetPic(int TestId, int Id)
        {
            try
            {
                TestPreview test = TestRepository.Tests.First(x => x.Id == TestId);
                test.ImgId = Id;
                TestRepository.SaveTest(test);
                TempData["message"] = string.Format("Изменения в тесте \"{0}\" были сохранены, вбрано изображение \"{1}\"", test.Theme, Id);
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["error"] = string.Format("Ошибка в выборе изображения");
                return RedirectToAction("Index");
            }
        }
        public ActionResult SetQuestPic(int QuestId, int Id)
        {
            try
            {
                Question question = QuestionRepository.Question.First(x => x.Id == QuestId);
                question.ImgId = Id;
                QuestionRepository.SaveQuestion(question);
                TempData["message"] = string.Format("Изменения в вопросе \"{0}\" были сохранены, вбрано изображение \"{1}\"", question.Id, Id);
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["error"] = string.Format("Ошибка в выборе изображения");
                return RedirectToAction("Index");
            }
        }
        public ActionResult SetAnsPic(int AnswerId, int Id)
        {
            try
            {
                Answers answer = AnswersRepository.Answers.First(x => x.Id == AnswerId);
                answer.ImgId = Id;
                AnswersRepository.SaveAnswer(answer);
                TempData["message"] = string.Format("Изменения в ответе \"{0}\" были сохранены, вбрано изображение \"{1}\"", answer.Id, Id);
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["error"] = string.Format("Ошибка в выборе изображения");
                return RedirectToAction("Index");
            }
        }
        public ActionResult SetLessPic(int LessId, int Id)
        {
            try
            {
                Lessions lessions = lessionssRepository.Lessions.First(x => x.Id == LessId);
                lessions.ImgId = Id;
                lessionssRepository.Savelession(lessions);
                TempData["message"] = string.Format("Изменения в лекции \"{0}\" были сохранены, вбрано изображение \"{1}\"", lessions.Theme, Id);
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["error"] = string.Format("Ошибка в выборе изображения");
                return RedirectToAction("Index");
            }
        }
        public void CalcScore(Answers answer)
        {
            int score = 0;
            Question question = QuestionRepository.Question.First(x => x.Id == answer.QuestionID);
            TestPreview test = TestRepository.Tests.First(x => x.Id == question.TestID);
            IEnumerable<Question> questions = QuestionRepository.Question.Where(x => x.TestID == test.Id);
            foreach (var a in questions)
            {
                IEnumerable<Answers> answers = AnswersRepository.Answers.Where(x => x.QuestionID == a.Id);
                foreach (var b in answers)
                {
                    if (b.IsCorrect == true)
                    {
                        score += b.AnswerScore;
                    }
                }
            }
            test.MaxScore = score;
            TestRepository.SaveTest(test);
            return;
        }
        public ActionResult LessIndex()
        {
            AdminIndexViewModel adminIndexViewModel = new AdminIndexViewModel();
            adminIndexViewModel.Tests = TestRepository.Tests;
            return View(adminIndexViewModel);
        }
        [HttpGet]
        public ViewResult CreateLess()
        {
            return View("EditLession", new Lessions());
        }
        public ActionResult EditLession(int? ID, int? Delete)
        {
            if (Delete == null)
            {
                TestPreview test = TestRepository.Tests.First(x => x.Id == ID);
                return View(test);
            }
            else
            {
                TestPreview test = TestRepository.Tests.First(x => x.Id == ID);
                IEnumerable<Question> questions = QuestionRepository.Question.Where(x => x.TestID == ID);
                foreach (var a in questions)
                {
                    IEnumerable<Answers> answers = AnswersRepository.Answers.Where(x => x.QuestionID == a.Id);
                    AnswersRepository.DeleteAnswers(answers);
                }
                QuestionRepository.DeleteQuests(questions);
                TestPreview deletedTest = TestRepository.DeleteTest(test.Id);
                if (deletedTest != null)
                {
                    TempData["message"] = string.Format("Тест \"{0}\" был удален", deletedTest.Theme);
                }
                return RedirectToAction("Index");
            }
        }
        [HttpPost]

        public ActionResult EditLession(TestPreview test)
        {
            if (ModelState.IsValid)
            {
                TestRepository.SaveTest(test);
                TempData["message"] = string.Format("Изменения в тесте \"{0}\" были сохранены", test.Theme);
                return RedirectToAction("Index");
            }
            else
            {

                return View(test);
            }
        }
    }
}