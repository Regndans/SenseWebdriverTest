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
        string textToFind = "Kitchen2";

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
            _driver.Url = "https://senseregndans.azurewebsites.net/";
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IWebElement pirliste = wait.Until(d => d.FindElement(By.Id("pirListe")));
            Assert.IsTrue(pirliste.Text.Contains(textToFind));
        }

        [TestMethod]
        public void HideButtonTest()
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    _driver.Url = "https://senseregndans.azurewebsites.net/";
                    WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                    IWebElement pirliste = wait.Until(d => d.FindElement(By.Id("pirListe")));
                    IWebElement hideTableButton = wait.Until(d => d.FindElement(By.Id("hideTableButton")));
                    hideTableButton.Submit();
                    Assert.IsFalse(pirliste.Text.Contains(textToFind));
                }
                catch (StaleElementReferenceException e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        [TestMethod]
        public void ShowButtonTest()
        {

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    _driver.Url = "https://senseregndans.azurewebsites.net/";
                    WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                    IWebElement pirliste = wait.Until(d => d.FindElement(By.Id("pirListe")));

                    IWebElement hideTableButton = wait.Until(d => d.FindElement(By.Id("hideTableButton")));
                    hideTableButton.Submit();

                    IWebElement showTableButton = _driver.FindElement(By.Id("showTableButton"));
                    showTableButton.Submit();
                    Assert.IsTrue(pirliste.Text.Contains(textToFind));
                }
                catch (StaleElementReferenceException e)
                {
                    Console.WriteLine(e);
                }

            }
        }

        [TestCleanup]
        public void EdgeDriverCleanup()
        {
            _driver.Quit();
        }
    }
}
