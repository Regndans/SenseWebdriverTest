using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;

namespace SenseWebdriverTest
{
    [TestClass]
    public class EdgeDriverTest
    {
        // In order to run the below test(s), 
        // please follow the instructions from http://go.microsoft.com/fwlink/?LinkId=619687
        // to install Microsoft WebDriver.

        private EdgeDriver _driver;

        [TestInitialize]
        public void EdgeDriverInitialize()
        {
            // Initialize edge driver 
            var options = new EdgeOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal
            };
            _driver = new EdgeDriver("C:\\Webdriver");
        }

        [TestMethod]
        public void VerifyPageTitle()
        {
            // Replace with your own test logic
            _driver.Url = "https://senseregndans.azurewebsites.net/";
            Assert.AreEqual("Title", _driver.Title);
        }

        [TestMethod]
        public void TestList()
        {
            string textToFind = "Nothing Detected";

            _driver.Url = "https://senseregndans.azurewebsites.net/";
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IWebElement pirliste = wait.Until(d => d.FindElement(By.Id("pirListe")));

            Assert.IsTrue(pirliste.Text.Contains(textToFind));
            
            IWebElement hideButton = _driver.FindElementById("hideTableButton");
            hideButton.Submit();
            Assert.IsFalse(pirliste.Text.Contains(textToFind));

            IWebElement showTableButton = _driver.FindElementById("showTableButton");
            showTableButton.Submit();
            Assert.IsTrue(pirliste.Text.Contains(textToFind));
        }

        [TestCleanup]
        public void EdgeDriverCleanup()
        {
            _driver.Quit();
        }
    }
}
