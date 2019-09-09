using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minesweeper;
using Minesweeper.Controllers;

namespace Minesweeper.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("Input", result.ViewBag.Input);
        }


        [TestMethod()]
        public void CalculoTest()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            JsonResult result = controller.Calculo("44\n*...\n....\n.*..\n....\n35\n**...\n.....\n.*...\n00") as JsonResult;

            // Assert
            Assert.AreEqual("Field #1\n*100\n2210\n1*10\n1110\n\nField #2\n**100\n33200\n1*100\n\n", result.Data);            
        }

    }
}
