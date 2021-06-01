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
using Kursach3.WebUI.Infrastructure.Abstract;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Net.Mail;

namespace Kursach3.WebUI.Controllers
{
    [Authorize(Roles = "Користувач")]
    public class TestController : Controller
    {
        private ITestRepository repository;
        public int pageSize = 5;
        private IQuestionRepository questionRepository;
        private IPicturesRepository picturesRepository;
        private ILessionssRepository LessionssRepository;
        private IFileRepository FileRepository;
        private IChapterRepository ChapterRepository;
        public TestController(ITestRepository repo,IQuestionRepository question, IPicturesRepository pictures, ILessionssRepository lessionss, IFileRepository file, IChapterRepository chapter)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<EFTestDbContext>());
            repository = repo;
            questionRepository = question;
            picturesRepository = pictures;
            LessionssRepository = lessionss;
            FileRepository = file;
            ChapterRepository = chapter;
        }
        public ViewResult List(string category, int page=1)
        {
         
            IEnumerable<TestPreview> testLIst = repository.Tests.Where(x => x.ZNO == false && x.MaxScore >0);
            TestListViewModel model = new TestListViewModel
            {
                Tests = testLIst
                    .Where(p => category == null || p.Course.ToString() == category)
                    .OrderBy(TestPreview => TestPreview.Course)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ?
                    repository.Tests.Count() :
                    repository.Tests.Where(TestPreview => TestPreview.Course.ToString() == category && TestPreview.MaxScore > 0).Count()
                },
                CurrentCategory = category,
                pictures = picturesRepository.Pictures
            };
            return View(model);
        }
        public ViewResult ZNOList(string category, int page = 1)
        {
           
            IEnumerable<TestPreview> testLIst = repository.Tests.Where(x => x.ZNO == true && x.MaxScore > 0);
                TestListViewModel model = new TestListViewModel
                {
                    Tests = testLIst
                        .Where(p => category == null || p.Course.ToString() == category && p.MaxScore > 0)
                        .OrderBy(TestPreview => TestPreview.Course)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = pageSize,
                        TotalItems = category == null ?
                        repository.Tests.Count() :
                        repository.Tests.Where(TestPreview => TestPreview.Course.ToString() == category && TestPreview.MaxScore > 0).Count()
                    },
                    CurrentCategory = category,
                    pictures = picturesRepository.Pictures
                };
                return View(model);
        }
        public ViewResult LessionsList(string category, int page = 1)
        {
           
            IEnumerable<Lessions> lessions = LessionssRepository.Lessions;
            LessionsListViewModel model = new LessionsListViewModel
            {
                Lessions = lessions
                    .Where(p => category == null || p.Course.ToString() == category)
                    .OrderBy(TestPreview => TestPreview.Course)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ?
                    repository.Tests.Count() :
                    repository.Tests.Where(TestPreview => TestPreview.Course.ToString() == category).Count()
                },
                CurrentCategory = category,
                pictures = picturesRepository.Pictures
            };
            return View(model);
        }
        public ActionResult Lession(int ID)
        {
            Lessions lessions = LessionssRepository.Lessions.First(x => x.Id == ID);
            List<Picture> pictures = new List<Picture>();
            List<Files> Files = new List<Files>();
            LessionViewModel model = new LessionViewModel
            {
                Lession = LessionssRepository.Lessions.First(x => x.Id == ID),
                Chapters = ChapterRepository._Chapters.Where(x=>x.LessionId==lessions.Id)  
            };
            if (model.Lession.ImgId != 0) model.Pictures.Append(picturesRepository.Pictures.First(x => x.Id == lessions.ImgId));
            foreach(var a in model.Chapters)
            {
                if (a.ImgId != 0) pictures.Add(picturesRepository.Pictures.First(x => x.Id == a.ImgId));
                if (a.PdfId != 0) Files.Add(FileRepository._Files.First(x => x.Id == a.PdfId));
                if (a.VideoId != 0) Files.Add(FileRepository._Files.First(x => x.Id == a.VideoId));
            }
            model.Pictures = pictures;
            model.Files =Files;
            return View(model);
        }
    }
    
}
