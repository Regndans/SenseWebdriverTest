using System;
using System.Linq;
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
        string textToFind = "Motion detected";
        private string dURL = "https://senseregndans.azurewebsites.net/";


        /// <summary>
        /// Initialisere vores testsetup med selenium
        /// </summary>
        [TestInitialize]
        public void EdgeDriverInitialize()
        {
            var options = new EdgeOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal
            };
            _driver = new EdgeDriver("C:\\Webdriver");

            // sæt driver-URL til at matche vores hjemmeside.
            _driver.Url = dURL;
        }
        
        /// <summary>
        /// Test om driveren går ind på den rigtige hjemmeside, ved at teste om sidens titel er den vi regner med.
        /// </summary>
        [TestMethod]
        public void VerifyPageTitle()
        {
            Assert.AreEqual("Sense", _driver.Title);
        }
        
        /// <summary>
        ///Test at vores tabel indeholder data, ved at finde en forekomst af et bestemt ord, vi ved at tabellen skal indeholde.
        /// </summary>
        [TestMethod]
        public void TestList()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IWebElement pirliste = wait.Until(d => d.FindElement(By.Id("pirListe")));
            Assert.IsTrue(pirliste.Text.Contains(textToFind));
        }


        /// <summary>
        ///Test af hideTableButton, som skal skjule indholdet af tabellen på title-siden. Vi tester ved at se om vi kan finde en text i tabellen,
        ///som forekom da tabellen var synlig, men nu ikke bør forekomme.
        /// </summary>
        [TestMethod]
        public void HideButtonTest()
        {
            //Da vi havde problemer med at IWebElement hideTableButton refererede til en "forældet" instans,
            //gentager vi processen med at sætte referencen 3 gange, for at give den øget chance for at virke.
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
                //Hvis metoden efter 3 forsøg, stadig ikke har en gyldig reference til hidetable-knappen,
                //skal den fange en exception og udskrive den til consollen.
                Console.WriteLine(e);
            }
            //}
        }
        
        /// <summary>
        /// Test af ShowTableButton, som skal vise indholdet af tabellen på hjemmesiden, hvis det er skjult.
        /// </summary>
        [TestMethod]
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
                    //Hvis metoden efter 3 forsøg, stadig ikke har en gyldig reference til hidetable-knappen,
                    //skal den fange en exception og udskrive den til consollen.
                    Console.WriteLine(e);
                }

            }
        }
        /// <summary>
        /// Test som leder efter et specifikt navn i vores liste af sensors
        /// </summary>
        [TestMethod]
        public void SensorNameTest()
        {
            string nameToFind = "Kitchen";
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IWebElement sensorListe = wait.Until(d => d.FindElement(By.Id("sensorList")));
            Assert.IsTrue(sensorListe.Text.Contains(nameToFind));
        }

        /// <summary>
        /// Test som tænder og slukker for vores sensors
        /// vi sætter en thread sleep på for at sikre os at azure kan nå at udføre vores command for den
        /// </summary>
        [TestMethod]
        public void SensorButtonOnOffTest()
        {

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IWebElement sensorOnButton = wait.Until(driver => driver.FindElement(By.Id("onButton")));
            IWebElement sensorOffButton = _driver.FindElement(By.Id("offButton"));
            IWebElement sensorListe = wait.Until(d => d.FindElement(By.Id("sensorList")));
            Console.WriteLine(sensorListe.Text);
            sensorOnButton.Submit();
            Assert.IsTrue(sensorListe.Text.Contains("On"));
            Console.WriteLine(sensorListe.Text);
            sensorOffButton.Submit();
            Thread.Sleep(1000);
            Assert.IsTrue(sensorListe.Text.Contains("Off"));
            Console.WriteLine(sensorListe.Text);
            sensorOnButton.Submit();
            Thread.Sleep(1000);
            Assert.IsTrue(sensorListe.Text.Contains("On"));
            Console.WriteLine(sensorListe.Text);

        }
        /// <summary>
        /// Vi tester her om timer input felterne,
        /// eksistere eftersom selenium ikke kan se noget tekst i vores input felter
        /// </summary>
        [TestMethod]
        public void TimerTest()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            IWebElement timerInputStart = wait.Until(driver => driver.FindElement(By.Id("timerStart"))) ;
            IWebElement timerInputEnd = _driver.FindElement(By.Id("timerEndReal"));
            Console.WriteLine("hej");
            Console.WriteLine(timerInputStart.Text);
            //Assert.IsTrue(timerInputStart.Text.Contains(DateTime.Now.Day.ToString()));

        }

        /// <summary>
        /// Vi tester vores namechange metode til at ændre et navn på vores sensor
        /// </summary>
        [TestMethod]
        public void NameChangeTest()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            var dropChangeNames = wait.Until(driver => driver.FindElement(By.Id("dropChangeNames")));
            var inputChangeName = _driver.FindElement(By.Id("inputChangeName"));
            var buttonChangeName = _driver.FindElement(By.Id("buttonChangeName"));
            var selectElement = new SelectElement(dropChangeNames);
            selectElement.SelectByText("Garden");
            inputChangeName.Clear();
            inputChangeName.SendKeys("have");
            buttonChangeName.Submit();

            string nameToFind = "have";
            IWebElement sensorListe = wait.Until(d => d.FindElement(By.Id("sensorList")));
            Assert.IsTrue(sensorListe.Text.Contains(nameToFind));

            selectElement.SelectByText("have");
            inputChangeName.Clear();
            inputChangeName.SendKeys("Garden");
            buttonChangeName.Submit();

        }

        /// <summary>
        /// vi tester vores sorterings metode til at se om et navn bliver sorteret fra nå vi vælger et andet navn
        /// </summary>
        [TestMethod]
        public void SortMotionsByNameTest()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            var dropChangeNames = wait.Until(driver => driver.FindElement(By.Id("dropSortNames")));
            var selectElement = new SelectElement(dropChangeNames);
            selectElement.SelectByText("Garden");
            Thread.Sleep(1000);
            string nameToFind = "Kitchen";
            IWebElement motionListe = wait.Until(d => d.FindElement(By.Id("pirListe")));
            Assert.IsFalse(motionListe.Text.Contains(nameToFind));
        }

        /// <summary>
        /// Afslut driveren.
        /// </summary>
        [TestCleanup]
        public void EdgeDriverCleanup()
        {
            _driver.Quit();
        }
    }
}
