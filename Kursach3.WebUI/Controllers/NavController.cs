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

            IEnumerable<string> categories = repository.Tests
                .Select(game => game.Category)
                .Distinct()
                .OrderBy(x => x);
            return PartialView("FlexMenu", categories);
        }
    }
}
