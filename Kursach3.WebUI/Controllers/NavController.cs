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
    public class NavController : Controller
    {
       
        private ITestRepository repository;
        public NavController(ITestRepository repo)
        {
            repository = repo;
        }
        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;
            IEnumerable<TestPreview> tests = repository.Tests.Where(x => x.ZNO == false);
            IEnumerable<string> categories = tests
                .Where(x => x.ImgId != 0 && x.NumOfQ>=1)
                .OrderBy(x => x.Course)
                .Select(test => test.Course.ToString())
                .Distinct();
            return PartialView("FlexMenu", categories);
        }
        public PartialViewResult ZNOMenu(string category = null)
        {
            ViewBag.SelectedCategory = category;
            IEnumerable<TestPreview> tests = repository.Tests.Where(x => x.ZNO == true);
            IEnumerable<string> categories = tests
                .Where(x => x.ImgId != 0 && x.NumOfQ >= 1)
                .OrderBy(x => x.Course)
                .Select(test => test.Course.ToString())
                .Distinct();
            return PartialView("ZNOFlexMenu", categories);
        }
    }
}
