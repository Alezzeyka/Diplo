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
    [Authorize(Roles = "Пользователь")]
    public class TestController : Controller
    {
        private ITestRepository repository;
        public int pageSize = 4;
        private IQuestionRepository questionRepository;
        private IPicturesRepository picturesRepository;
        public TestController(ITestRepository repo,IQuestionRepository question, IPicturesRepository pictures)
        {
            repository = repo;
            questionRepository = question;
            picturesRepository = pictures;
        }
        public ViewResult List(string category, int page=1)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<EFTestDbContext>());
            IEnumerable<TestPreview> testLIst = repository.Tests.Where(x => x.ZNO == false);
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
                    repository.Tests.Where(TestPreview => TestPreview.Course.ToString() == category).Count()
                },
                CurrentCategory = category,
                pictures = picturesRepository.Pictures
            };
            return View(model);
        }
        public ViewResult ZNOList(string category, int page = 1)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<EFTestDbContext>());
            IEnumerable<TestPreview> testLIst = repository.Tests.Where(x => x.ZNO == true);
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
                        repository.Tests.Where(TestPreview => TestPreview.Course.ToString() == category).Count()
                    },
                    CurrentCategory = category,
                    pictures = picturesRepository.Pictures
                };
                return View(model);
        }
    }
    
}
