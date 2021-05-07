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
        string textToFind = "string";
        private string dURL = "https://senseregndans.azurewebsites.net/";


        //
        [TestInitialize]
        public void EdgeDriverInitialize()
        {
            // Initialiser webdriver, fundet på C-drevet. Der benyttes chrome, fremfor Edge. 
            var options = new EdgeOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal
            };
            _driver = new EdgeDriver("C:\\Webdriver");

            // sæt driver-URL til at matche vores hjemmeside.
            _driver.Url = dURL;
        }

        [TestMethod]
        //Test om driveren går ind på den rigtige hjemmeside, ved at teste om sidens titel er den vi regner med.
        public void VerifyPageTitle()
        {
            Assert.AreEqual("Title", _driver.Title);
        }

        [TestMethod]
        //Test at vores tabel indeholder data, ved at finde en forekomst af et bestemt ord, vi ved at tabellen skal indeholde.
        public void TestList()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IWebElement pirliste = wait.Until(d => d.FindElement(By.Id("pirListe")));
            Assert.IsTrue(pirliste.Text.Contains(textToFind));
        }

        [TestMethod]
        //Test af hideTableButton, som skal skjule indholdet af tabellen på title-siden. Vi tester ved at se om vi kan finde en text i tabellen, som forekom da tabellen var synlig, men nu ikke bør forekomme.
        public void HideButtonTest()
        {
            //Da vi havde problemer med at IWebElement hideTableButton refererede til en "forældet" instans, gentager vi processen med at sætte referencen 3 gange, for at give den øget chance for at virke.
            //for (int i = 0; i < 3; i++)
            //{
                try
                {
                    //Der oprettes reference til tabellen.
                    WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                    IWebElement pirliste = wait.Until(d => d.FindElement(By.Id("pirListe")));

                    //Vi sætter reference til og trykker på hideTable-knappen på hjemmesiden.
                    IWebElement hideTableButton = wait.Until(d => d.FindElement(By.Id("hideTableButton")));
                    hideTableButton.Submit();

                    Assert.IsFalse(pirliste.Text.Contains(textToFind));
                }
                catch (StaleElementReferenceException e)
                {
                    //Hvis metoden efter 3 forsøg, stadig ikke har en gyldig reference til hidetable-knappen, skal den fange en exception og udskrive den til consollen.
                    Console.WriteLine(e);
                }
            //}
        }

        [TestMethod]
        //Test af ShowTableButton, som skal vise indholdet af tabellen på hjemmesiden, hvis det er skjult.
        public void ShowButtonTest()
        {
            //Der bruges igen et for-loop for at sikre en gyldig reference til knapperne.
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    //Der laves reference til tabellen.
                    WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                    IWebElement pirliste = wait.Until(d => d.FindElement(By.Id("pirListe")));

                    //Vi trykker på hidetable-knappen først.
                    IWebElement hideTableButton = wait.Until(d => d.FindElement(By.Id("hideTableButton")));
                    hideTableButton.Submit();

                    //Vi sætter reference til og trykker på showtable-knappen.
                    IWebElement showTableButton = _driver.FindElement(By.Id("showTableButton"));
                    showTableButton.Submit();
                    Assert.IsTrue(pirliste.Text.Contains(textToFind));
                }
                catch (StaleElementReferenceException e)
                {
                    //Hvis metoden efter 3 forsøg, stadig ikke har en gyldig reference til hidetable-knappen, skal den fange en exception og udskrive den til consollen.
                    Console.WriteLine(e);
                }

            }
        }

        [TestMethod]
        public void SensorNameTest()
        {
            string nameToFind = "TestRoom";
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IWebElement sensorListe = wait.Until(d => d.FindElement(By.Id("sensorListe")));
            Assert.IsTrue(sensorListe.Text.Contains(nameToFind));
        }

        [TestCleanup]
        //Afslut driveren.
        public void EdgeDriverCleanup()
        {
            _driver.Quit();
        }
    }
}
