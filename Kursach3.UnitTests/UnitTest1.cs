using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Kursach3Domain.Abstract;
using Kursach3Domain.Entities;
using Kursach3.WebUI.Controllers;
using Kursach3.WebUI.Models;
using Kursach3.WebUI.HtmlHelpers;

namespace Kursach3.UnitTests

{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Организация (arrange)
            Mock<ITestRepository> mock = new Mock<ITestRepository>();
            mock.Setup(m => m.Tests).Returns(new List<TestPreview>
            {
                new TestPreview { Id = 1, Name = "Тест1"},
                new TestPreview { Id = 2, Name = "Тест2"},
                new TestPreview { Id = 3, Name = "Тест3"},
                new TestPreview { Id = 4, Name = "Тест4"},
                new TestPreview { Id = 5, Name = "Тест5"}
            });
            TestController controller = new TestController(mock.Object);
            controller.pageSize = 3;

            // Действие (act)
            TestListViewModel result = (TestListViewModel)controller.List(null,2).Model;

            // Утверждение (assert)
            List<TestPreview> testPreviews = result.ToList();
            Assert.IsTrue(testPreviews.Count == 2);
            Assert.AreEqual(testPreviews[0].Name, "Тест4");
            Assert.AreEqual(testPreviews[1].Name, "Тест5");
        }
        [TestMethod]
        public void Can_Generate_Page_Links()
        {

            // Организация - определение вспомогательного метода HTML - это необходимо
            // для применения расширяющего метода
            HtmlHelper myHelper = null;

            // Организация - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }
        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)
            Mock<ITestRepository> mock = new Mock<ITestRepository>();
            mock.Setup(m => m.Tests).Returns(new List<TestPreview>
    {
        new TestPreview { Id = 1, Name = "Тест1"},
        new TestPreview { Id = 2, Name = "Тест2"},
        new TestPreview { Id = 3, Name = "Тест3"},
        new TestPreview { Id = 4, Name = "Тест4"},
        new TestPreview { Id = 5, Name = "Тест5"}
    });
            TestController controller = new TestController(mock.Object);
            controller.pageSize = 3;

            // Act
            TestListViewModel result
                = (TestListViewModel)controller.List(null,2).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }
        [TestMethod]
        public void Can_Filter_Games()
        {
            // Организация (arrange)
            Mock<ITestRepository> mock = new Mock<ITestRepository>();
            mock.Setup(m => m.Tests).Returns(new List<TestPreview>
    {
        new TestPreview { Id = 1, Name = "Тест1", Category="Cat1"},
        new TestPreview { Id = 2, Name = "Тест2", Category="Cat2"},
        new TestPreview { Id = 3, Name = "Тест3", Category="Cat1"},
        new TestPreview { Id = 4, Name = "Тест4", Category="Cat2"},
        new TestPreview { Id = 5, Name = "Тест5", Category="Cat3"}
    });
            TestController controller = new TestController(mock.Object);
            controller.pageSize = 3;

            // Action
            List<TestPreview> result = ((TestListViewModel)controller.List("Cat2", 1).Model)
                .Games.ToList();

            // Assert
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Тест2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "Тест4" && result[1].Category == "Cat2");
        }
    [TestMethod]
    public void Indicates_Selected_Category()
    {
        // Организация - создание имитированного хранилища
        Mock<ITestRepository> mock = new Mock<ITestRepository>();
        mock.Setup(m => m.Tests).Returns(new TestPreview[] {
        new TestPreview { Id = 1, Name = "Тест1", Category="История"},
        new TestPreview { Id = 2, Name = "Тест2", Category="Математика"}
    });

        // Организация - создание контроллера
        NavController target = new NavController(mock.Object);

        // Организация - определение выбранной категории
        string categoryToSelect = "Математика";

        // Действие
        string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

        // Утверждение
        Assert.AreEqual(categoryToSelect, result);
    }
        [TestMethod]
        public void Can_Create_Categories()
        {
            // Организация - создание имитированного хранилища
            Mock<ITestRepository> mock = new Mock<ITestRepository>();
            mock.Setup(m => m.Tests).Returns(new List<TestPreview> {
        new TestPreview { Id = 1, Name = "Тест1", Category="Математика"},
        new TestPreview { Id = 2, Name = "Тест2", Category="Математика"},
        new TestPreview { Id = 3, Name = "Тест3", Category="История"},
        new TestPreview { Id = 4, Name = "Тест4", Category="Украинский язык"},
    });

            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Действие - получение набора категорий
            List<string> results = ((IEnumerable<string>)target.Menu().Model).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 3);
            Assert.AreEqual(results[0], "Украинский язык");
            Assert.AreEqual(results[1], "Математика");
            Assert.AreEqual(results[2], "История");
        }
        [TestMethod]
        public void Generate_Category_Specific_Game_Count()
        {
            /// Организация (arrange)
            Mock<ITestRepository> mock = new Mock<ITestRepository>();
            mock.Setup(m => m.Tests).Returns(new List<TestPreview>
    {
        new TestPreview { Id = 1, Name = "Игра1", Category="Cat1"},
        new TestPreview { Id = 2, Name = "Игра2", Category="Cat2"},
        new TestPreview { Id = 3, Name = "Игра3", Category="Cat1"},
        new TestPreview { Id = 4, Name = "Игра4", Category="Cat2"},
        new TestPreview { Id = 5, Name = "Игра5", Category="Cat3"}
    });
            TestController controller = new TestController(mock.Object);
            controller.pageSize = 3;

            // Действие - тестирование счетчиков товаров для различных категорий
            int res1 = ((TestListViewModel)controller.List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((TestListViewModel)controller.List("Cat2").Model).PagingInfo.TotalItems;
            int res3 = ((TestListViewModel)controller.List("Cat3").Model).PagingInfo.TotalItems;
            int resAll = ((TestListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            // Утверждение
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    }
}
