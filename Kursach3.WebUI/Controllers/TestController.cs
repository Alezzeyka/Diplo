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
    public class TestController : Controller
    {
        private ITestRepository repository;
        public int pageSize = 4;
        private IQuestionRepository questionRepository;
        public TestController(ITestRepository repo,IQuestionRepository question)
        {
            repository = repo;
            questionRepository = question;
        }
        public ViewResult List(string category, int page=1)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<EFTestDbContext>());
            TestListViewModel model = new TestListViewModel
            {
                Tests = repository.Tests
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(TestPreview => TestPreview.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ?
                    repository.Tests.Count() :
                    repository.Tests.Where(TestPreview => TestPreview.Category == category).Count()
                },
                CurrentCategory = category,
                questions = questionRepository.Question
            };
            return View(model);
        }
        public FileContentResult GetImage(int Id)
        {
            TestPreview test = repository.Tests
                .FirstOrDefault(t => t.Id == Id);

            if (test != null)
            {
                return File(test.ImageData, test.ImageMimeType);
            }
            else
            {
                return null;
            }
        }

    }
    
}
