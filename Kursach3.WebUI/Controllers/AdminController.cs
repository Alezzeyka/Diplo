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
using System.IO.Compression;
using System.IO.MemoryMappedFiles;
using System.Data.Entity;
using Kursach3.WebUI.Infrastructure.Abstract;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Net.Mail;

namespace Kursach3.WebUI.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        private ITestRepository TestRepository;
        private IQuestionRepository QuestionRepository;
        private IAnswersRepository AnswersRepository;
        private IPicturesRepository PicturesRepository;
        private ILessionssRepository lessionssRepository;
        private IMultiChoiceRepository MultiChoiceRepository;
        private IFileRepository FileRepository;
        private IChapterRepository ChapterRepository;
        public AdminController(ITestRepository testRepository, IQuestionRepository questionRepository, IAnswersRepository answersRepository, IPicturesRepository picturesRepository, ILessionssRepository lessionss, IMultiChoiceRepository lines, IFileRepository file, IChapterRepository chapter)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<EFTestDbContext>());
            TestRepository = testRepository;
            QuestionRepository = questionRepository;
            AnswersRepository = answersRepository;
            PicturesRepository = picturesRepository;
            lessionssRepository = lessionss;
            MultiChoiceRepository = lines;
            FileRepository = file;
            ChapterRepository = chapter;
        }
        public ActionResult Index()
        {

            AdminIndexViewModel adminIndexViewModel = new AdminIndexViewModel();
            adminIndexViewModel.Tests = TestRepository.Tests;
            return View(adminIndexViewModel);
        }
        public void CalcScore(Question question)
        {
            if(question.MultiChoice==false)
            {
                question.NumOfCorrectAnswers = AnswersRepository.Answers.Where(x => x.QuestionID == question.Id && x.IsCorrect == true && x.LineAnswer == false).Count();
                question.Score = AnswersRepository.Answers.Where(x => x.QuestionID == question.Id && x.IsCorrect == true && x.LineAnswer == false).Sum(x => x.AnswerScore);
                QuestionRepository.SaveQuestion(question);
                TestPreview test = TestRepository.Tests.First(x => x.Id == question.TestID);
                CalcScore(test);
            }
            else 
            {
                question.NumOfCorrectAnswers = MultiChoiceRepository.Lines.Where(x => x.QuestionID == question.Id).Sum(x => x.NumOfCorrectAnswers);
                question.Score = MultiChoiceRepository.Lines.Where(x => x.QuestionID == question.Id).Sum(x => x.LineScore);
                QuestionRepository.SaveQuestion(question);
                TestPreview test = TestRepository.Tests.First(x => x.Id == question.TestID);
                CalcScore(test);
            }
        }
        public void CalcScore(MultiChoice Line)
        {
         Line.NumOfCorrectAnswers= AnswersRepository.Answers.Where(x => x.QuestionID == Line.Id && x.IsCorrect == true && x.LineAnswer == true).Count();
         Line.LineScore = AnswersRepository.Answers.Where(x => x.QuestionID == Line.Id && x.IsCorrect == true && x.LineAnswer==true).Sum(x => x.AnswerScore);
         MultiChoiceRepository.SaveLine(Line);
         Question question = QuestionRepository.Question.First(x => x.Id == Line.QuestionID);
         CalcScore(question);
        }
        public void CalcScore(TestPreview test)
        {
            test.NumOfQ = QuestionRepository.Question.Where(x => x.TestID == test.Id).Count();
            test.MaxScore = QuestionRepository.Question.Where(x => x.TestID == test.Id).Sum(x => x.Score);
            TestRepository.SaveTest(test);
        }
        public ActionResult QuestionIndex(int? ID,int? QId)
        {
            if ((ID != null) || (QId != null))
            {
                TestPreview test = new TestPreview();
                if(QId != null)
                {
                    Question question = QuestionRepository.Question.First(x => x.Id == QId);
                    test = TestRepository.Tests.First(x => x.Id == question.TestID);

                    QuestionIndexViewModel questions = new QuestionIndexViewModel();
                    questions.Questions = QuestionRepository.Question.Where(x => x.TestID == question.TestID);
                    questions.TestID = test.Id;
                    return View(questions);
                }
                else
                {
                    test = TestRepository.Tests.First(x => x.Id == ID);
                    QuestionIndexViewModel questions = new QuestionIndexViewModel();
                    questions.Questions = QuestionRepository.Question.Where(x => x.TestID == ID);
                    questions.TestID = test.Id;
                    return View(questions);
                }
            }
            else 
            {
                QuestionIndexViewModel questions = new QuestionIndexViewModel();
                questions.Questions = QuestionRepository.Question;
                return View(questions);
            }
        }
        public ActionResult AnswerIndex(int? ID,int? LineID)
        {
            if ((ID != 0)&&(LineID==0))
            {
                Question question = QuestionRepository.Question.First(x => x.Id == ID);
                AnswersIndexViewModel answers = new AnswersIndexViewModel();
                answers.Answers = AnswersRepository.Answers.Where(x => x.QuestionID == ID && x.LineAnswer == false);
                answers.QuestionID = question.Id;
                answers.Line = 0;
                return View(answers);
            } else
            if((LineID!=0)&&(ID==0))
            {
                MultiChoice choice = MultiChoiceRepository.Lines.First(x => x.Id == LineID);
                AnswersIndexViewModel answers = new AnswersIndexViewModel();
                answers.Answers = AnswersRepository.Answers.Where(x => x.QuestionID == LineID && x.LineAnswer==true);
                answers.QuestionID = choice.Id;
                answers.Line = 1;
                return View(answers);
            } else
            if((LineID==null)&&(ID==null))
            {
                AnswersIndexViewModel answers = new AnswersIndexViewModel();
                answers.Answers = AnswersRepository.Answers;
                return View(answers);
            }
            else
            {
                TempData["error"] = string.Format("error");
                return RedirectToAction("Index");
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
                    if (a.MultiChoice == false)
                    {
                        IEnumerable<Answers> answers = AnswersRepository.Answers.Where(x => x.QuestionID == a.Id);
                        AnswersRepository.DeleteAnswers(answers);
                    }
                    else 
                    {
                        IEnumerable<MultiChoice> choices = MultiChoiceRepository.Lines.Where(x => x.QuestionID == a.Id);
                         foreach(var b in choices)
                        {
                            AnswersRepository.DeleteAnswers(AnswersRepository.Answers.Where(x => x.QuestionID == b.Id));
                        }
                        MultiChoiceRepository.DeleteLines(choices);
                    }
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
                    test.Category = "Історія України";
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
                TestPreview test = TestRepository.Tests.First(x => x.Id == question.TestID);
                if (question.MultiChoice == false)
                {
                    AnswersRepository.DeleteAnswers(AnswersRepository.Answers.Where(x => x.QuestionID == ID));
                    Question deletedQuestion = QuestionRepository.DeleteQuest(question.Id);
                    if (deletedQuestion != null)
                    {
                        TempData["message"] = string.Format("Вопрос был удален");
                    }
                    
                }
                else
                {
                    IEnumerable<MultiChoice> choices = MultiChoiceRepository.Lines.Where(x => x.QuestionID == question.Id);
                    foreach(var a in choices)
                    {
                        AnswersRepository.DeleteAnswers(AnswersRepository.Answers.Where(x => x.QuestionID == a.Id));
                    }
                    MultiChoiceRepository.DeleteLines(choices);
                    Question deletedQuestion = QuestionRepository.DeleteQuest(question.Id);
                    if (deletedQuestion != null)
                    {
                        TempData["message"] = string.Format("Вопрос был удален");
                    }
                }
                CalcScore(test);
                int Id = test.Id;
                return RedirectToAction("QuestionIndex", new { Id });
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
                CalcScore(test);
                int Id = test.Id;
                return RedirectToAction("QuestionIndex",new { Id });
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
                    if (deletedAnswer.LineAnswer == false)
                    {
                        Question question = QuestionRepository.Question.First(x => x.Id == deletedAnswer.QuestionID);
                        int Id = question.Id;
                        int LineID = 0;
                        CalcScore(question);
                        TempData["message"] = string.Format("Ответ был удален");
                        return RedirectToAction("AnswerIndex",new { Id,LineID });
                    }
                    else 
                    {
                        MultiChoice line = MultiChoiceRepository.Lines.First(x => x.Id == deletedAnswer.QuestionID);
                        CalcScore(line);
                        int LineID = line.Id;
                        int Id = 0;
                        TempData["message"] = string.Format("Ответ был удален");
                        return RedirectToAction("AnswerIndex", new { Id, LineID });
                    }
                }
            return RedirectToAction("Index");
            }
        }
        
        [HttpPost]
        public ActionResult EditAnswer(Answers answer)
        {
            if (ModelState.IsValid)
            {
                Question question1 = new Question();
                MultiChoice multi = new MultiChoice();
                int LineID = 0;
                int Id = 0;
                if(answer.LineAnswer==false)
                {
                    question1 = QuestionRepository.Question.First(x => x.Id == answer.QuestionID);
                    Id = question1.Id;
                }
                else
                {
                    multi = MultiChoiceRepository.Lines.First(x => x.Id == answer.QuestionID);
                    LineID = multi.Id;
                }
                if ((answer.LineAnswer == false)&&(AnswersRepository.Answers.Where(x=>x.QuestionID==question1.Id && x.LineAnswer==false).Count()<4))
                {
                    AnswersRepository.SaveAnswer(answer);
                    Question question = QuestionRepository.Question.First(x => x.Id == answer.QuestionID);
                    CalcScore(question);
                    Id = question.Id;
                    LineID = 0;
                    TempData["message"] = string.Format("Изменения в ответе были сохранены");
                    return RedirectToAction("AnswerIndex", new { Id, LineID });
                }
                else if ((answer.LineAnswer == true)&&(AnswersRepository.Answers.Where(x => x.QuestionID == multi.Id && x.LineAnswer == true).Count()<5))
                {
                    AnswersRepository.SaveAnswer(answer);
                    MultiChoice line = MultiChoiceRepository.Lines.First(x => x.Id == answer.QuestionID);
                    CalcScore(line);
                    LineID = line.Id;
                    Id = 0;
                    TempData["message"] = string.Format("Изменения в ответе были сохранены");
                    return RedirectToAction("AnswerIndex", new { Id, LineID });
                }
                else
                {
                    TempData["error"] = string.Format("Достигнуто максимальное количество ответов");
                    return RedirectToAction("AnswerIndex", new { Id, LineID });
                }
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
        public ViewResult CreateAnswer(int QuestionID, int? line)
        {
            if(line!=null)
            return View("EditAnswer", new Answers { QuestionID=QuestionID,LineAnswer=true});
            else
            return View("EditAnswer", new Answers { QuestionID = QuestionID,LineAnswer = false });
        }
        public ActionResult PicIndex()
        {
            return View(PicturesRepository.Pictures);
        }
        [HttpGet]
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
                TestPreview test = TestRepository.Tests.First(x => x.Id == question.TestID);
                question.ImgId = Id;
                QuestionRepository.SaveQuestion(question);
                TempData["message"] = string.Format("Изменения в вопросе \"{0}\" были сохранены, вбрано изображение \"{1}\"", question.Id, Id);
                return RedirectToAction("QuestionIndex",new { test.Id });
            }
            catch
            {
                Question question = QuestionRepository.Question.First(x => x.Id == QuestId);
                TestPreview test = TestRepository.Tests.First(x => x.Id == question.TestID);
                TempData["error"] = string.Format("Ошибка в выборе изображения");
                return RedirectToAction("QuestionIndex", new { test.Id });
            }
        }
        public ActionResult SetAnsPic(int AnswerId, int Id)
        {
            try
            {
                Answers answer = AnswersRepository.Answers.First(x => x.Id == AnswerId);
                Question question = QuestionRepository.Question.First(x => x.Id == answer.QuestionID);
                answer.ImgId = Id;
                AnswersRepository.SaveAnswer(answer);
                TempData["message"] = string.Format("Изменения в ответе \"{0}\" были сохранены, вбрано изображение \"{1}\"", answer.Id, Id);
                return RedirectToAction("AnswerIndex",new { question.Id });
            }
            catch
            {
                Answers answer = AnswersRepository.Answers.First(x => x.Id == AnswerId);
                Question question = QuestionRepository.Question.First(x => x.Id == answer.QuestionID);
                TempData["error"] = string.Format("Ошибка в выборе изображения");
                return RedirectToAction("AnswerIndex", new { question.Id });
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
                return RedirectToAction("LessIndex");
            }
            catch
            {
                TempData["error"] = string.Format("Ошибка в выборе изображения");
                return RedirectToAction("LessIndex");
            }
        }
        
        public ActionResult LessIndex()
        {
            return View(lessionssRepository.Lessions);
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
                Lessions lessions = lessionssRepository.Lessions.First(x => x.Id == ID);
                return View(lessions);
            }
            else
            {
                Lessions lessions = lessionssRepository.Lessions.First(x => x.Id == ID);
                Lessions deletedLession = lessionssRepository.DeleteLession(lessions.Id);
                if (deletedLession != null)
                {
                    TempData["message"] = string.Format("Урок \"{0}\" был удален", deletedLession.Theme);
                }
                return RedirectToAction("LessIndex");
            }
        }
        [HttpPost]

        public ActionResult EditLession(Lessions lessions)
        {
            if (ModelState.IsValid)
            {
                lessionssRepository.Savelession(lessions);
                TempData["message"] = string.Format("Изменения в уроке \"{0}\" были сохранены", lessions.Theme);
                return RedirectToAction("LessIndex");
            }
            else
            {

                return View(lessions);
            }
        }
        public ActionResult AnswerLineIndex(int? ID, int? LineID)
        {
            if (ID != null)
            {
                Question question = QuestionRepository.Question.First(x => x.Id == ID);
                AnswersLineIndexViewModel Lines = new AnswersLineIndexViewModel
                {
                    Lines = MultiChoiceRepository.Lines.Where(x => x.QuestionID == question.Id),
                    QuestionID=question.Id
                };
            return View(Lines);
            }
            if (LineID != null)
            {
                MultiChoice Line = MultiChoiceRepository.Lines.First(x=>x.Id==LineID);
                Question question = QuestionRepository.Question.First(x => x.Id == Line.QuestionID);
                AnswersLineIndexViewModel Lines = new AnswersLineIndexViewModel
                {
                    Lines = MultiChoiceRepository.Lines.Where(x => x.QuestionID == question.Id),
                    QuestionID = question.Id
                };
                return View(Lines);
            }
            else
            {
                AnswersLineIndexViewModel Lines = new AnswersLineIndexViewModel();
                Lines.Lines = MultiChoiceRepository.Lines;
                return View(Lines);
            }
        }
        public ActionResult EditAnswerLine(int? ID, int? Delete)
        {
            if (Delete == null)
            {
                MultiChoice line = MultiChoiceRepository.Lines.First(x => x.Id == ID);
                return View(line);
            }
            else
            {
                MultiChoice line = MultiChoiceRepository.Lines.First(x => x.Id == ID);
                Question question = QuestionRepository.Question.First(x => x.Id == line.QuestionID);
                AnswersRepository.DeleteAnswers(AnswersRepository.Answers.Where(x => x.QuestionID == line.Id));
                MultiChoice deletedLine = MultiChoiceRepository.DeleteLine(line.Id);
                if (deletedLine != null)
                {
                    CalcScore(question);
                    TempData["message"] = string.Format("Ответ был удален");
                }
                int Id = question.Id;
                return RedirectToAction("AnswerLineIndex", new { Id });
            }
        }
        [HttpPost]
        public ActionResult EditAnswerLine(MultiChoice Line)
        {
            if (ModelState.IsValid)
            {
                Question question = QuestionRepository.Question.First(x => x.Id == Line.QuestionID);
                int Id = question.Id;
                if (MultiChoiceRepository.Lines.Where(x => x.QuestionID == question.Id).Count() < 4)
                {
                    MultiChoiceRepository.SaveLine(Line);
                    CalcScore(question);
                    TempData["message"] = string.Format("Изменения сохранены");
                    return RedirectToAction("AnswerLineIndex", new { Id });
                }
                else
                {
                    TempData["error"] = string.Format("Нельзя добавить больше вариантов, достигнуто максимальное значение");
                    return RedirectToAction("AnswerLineIndex", new { Id });
                }
            }
            else
            {
                return View(Line);
            }
        }
        [HttpGet]
        public ViewResult CreateAnswerLine(int QuestionID)
        {
            MultiChoice line = new MultiChoice { QuestionID = QuestionID };
            return View("EditAnswerLine",line);
        }
        public ActionResult FileIndex()
        {
            return View(FileRepository._Files);
        }
        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload,string Name)
        {
            try
            {

                if (upload != null)
                {

                    // получаем имя файла
                    string fileName = System.IO.Path.GetFileName(upload.FileName);
                    // сохраняем файл в папку Files в проекте
                    upload.SaveAs(Server.MapPath("~/Files/" + fileName));
                    Files file = new Files
                    {
                        FileName = upload.FileName,
                        FileType = upload.ContentType,
                        Name = Name
                    };
                    string path = Server.MapPath("~/Files/" + fileName);
                    file.FilePath = path;
                    FileRepository.SaveFile(file);
                    TempData["message"] = string.Format("Файл успешно загружен");
                }
                else
                {
                    TempData["error"] = string.Format("Вы не выбрали файл");
                }
                    return RedirectToAction("FileIndex");
            }
            catch
            {
                TempData["error"] = string.Format("Файл слишком большой");
                return RedirectToAction("FileIndex");
            }
        }
        public ActionResult DeleteFile(int ID)
        {
            try
            {
                Files file = FileRepository._Files.First(x => x.Id == ID);
                System.IO.File.Delete(file.FilePath);
                var result = FileRepository.DeleteFile(file.Id);
                if (result != null)
                {
                    TempData["message"] = string.Format("Файл успешно удален");
                    return RedirectToAction("Fileindex");
                }
                else
                {
                    TempData["error"] = string.Format("error");
                    return RedirectToAction("Fileindex");
                }
            }
            catch(Exception ex)
            {
                TempData["error"] = string.Format("error" + ex.Message);
                return RedirectToAction("Fileindex");
            }
        }
        public ActionResult ChapterIndex(int? ID)
        {
            if (ID != null)
            {
                Lessions lession = lessionssRepository.Lessions.First(x => x.Id == ID);
                ChapterIndexViewModel model = new ChapterIndexViewModel
                {
                    LessionId = lession.Id,
                    Chapters = ChapterRepository._Chapters.Where(x => x.LessionId == ID)
                };
                return View(model);
            }
            else
            {
                ChapterIndexViewModel model = new ChapterIndexViewModel
                {
                    Chapters = ChapterRepository._Chapters
                };
                return View(model);
            }
        }
        public ActionResult EditChapter(int? ID, int? Delete)
        {
            if (Delete == null)
            {
                Chapter chapter = ChapterRepository._Chapters.First(x => x.Id == ID);
                return View(chapter);
            }
            else
            {
                
                Chapter chapter = ChapterRepository._Chapters.First(x => x.Id == ID);
                Lessions lession = lessionssRepository.Lessions.First(x => x.Id == chapter.LessionId);
                Chapter deletedChapter = ChapterRepository.DeleteChapter(chapter.Id);
                if (deletedChapter != null)
                {
                    TempData["message"] = string.Format("Абзац был удален");
                }
                int Id = lession.Id;
                return RedirectToAction("ChapterIndex", new { Id });
            }
        }
        [HttpPost]
        public ActionResult EditChapter(Chapter chapter)
        {
            if (ModelState.IsValid)
            {
                Lessions lession = lessionssRepository.Lessions.First(x => x.Id == chapter.LessionId);
                int Id = lession.Id;
                ChapterRepository.SaveChapter(chapter);
                TempData["message"] = string.Format("Изменения сохранены");
                    return RedirectToAction("ChapterIndex", new { Id });
            }
            else
            {
                return View(chapter);
            }
        }
        [HttpGet]
        public ViewResult CreateChapter(int LessionID)
        {
            Chapter chapter = new Chapter { LessionId = LessionID };
            return View("EditChapter", chapter);
        }
        public ActionResult SetChapterPicIndex(int Id)
        {
            SetPicIndexModel model = new SetPicIndexModel { ChapterId = Id };
            model.Pictures = PicturesRepository.Pictures;
            return View(model);
        }
        public ActionResult SetChapterPic(int ChapterId, int Id)
        {
            try
            {
                Chapter chapter = ChapterRepository._Chapters.First(x => x.Id == ChapterId);
                chapter.ImgId = Id;
                Lessions lession = lessionssRepository.Lessions.First(x => x.Id == chapter.LessionId);
                int ID = lession.Id;
                ChapterRepository.SaveChapter(chapter);
                TempData["message"] = string.Format("Изменения были сохранены, вбрано изображение \"{0}\"",Id);
                return RedirectToAction("ChapterIndex", new { ID });
            }
            catch
            {
                TempData["error"] = string.Format("Ошибка в выборе изображения");
                return RedirectToAction("LessIndex");
            }
        }
        public ActionResult SetVideoIndex(int Id)
        {
            FilePicIndexViewModel model = new FilePicIndexViewModel { ChapterId = Id };
            model.Files = FileRepository._Files.Where(x => x.FileType.Contains("video") == true);
            return View(model);
        }
        public ActionResult SetVideo(int ChapterId, int Id)
        {
            try
            {
                Chapter chapter = ChapterRepository._Chapters.First(x => x.Id == ChapterId);
                chapter.VideoId = Id;
                Lessions lession = lessionssRepository.Lessions.First(x => x.Id == chapter.LessionId);
                int ID = lession.Id;
                ChapterRepository.SaveChapter(chapter);
                TempData["message"] = string.Format("Изменения были сохранены, выбрано видео \"{0}\"", Id);
                return RedirectToAction("ChapterIndex", new { ID });
            }
            catch
            {
                TempData["error"] = string.Format("Ошибка в выборе видео");
                return RedirectToAction("LessIndex");
            }
        }
        public ActionResult SetPDFIndex(int Id)
        {
            FilePicIndexViewModel model = new FilePicIndexViewModel { ChapterId = Id };
            model.Files = FileRepository._Files.Where(x => x.FileType.Contains("application") == true);
            return View(model);
        }
        public ActionResult SetPDF(int ChapterId, int Id)
        {
            try
            {
                Chapter chapter = ChapterRepository._Chapters.First(x => x.Id == ChapterId);
                chapter.PdfId = Id;
                Lessions lession = lessionssRepository.Lessions.First(x => x.Id == chapter.LessionId);
                int ID = lession.Id;
                ChapterRepository.SaveChapter(chapter);
                TempData["message"] = string.Format("Изменения были сохранены, выбран файл \"{0}\"", Id);
                return RedirectToAction("ChapterIndex", new { ID });
            }
            catch
            {
                TempData["error"] = string.Format("Ошибка в выборе изображения");
                return RedirectToAction("LessIndex");
            }
        }
    }
}