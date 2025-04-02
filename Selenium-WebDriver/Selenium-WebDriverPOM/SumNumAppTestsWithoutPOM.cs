using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V132.PWA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_SumNumApp
{
    public class SumNumAppTestsWithoutPOM
    {
        IWebDriver driver;


        private SumNumberPOM sumPage;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
           

            sumPage = new SumNumberPOM(driver);
            sumPage.OpenPage(); 
        }

        [Test]
        public void TestWithCorrectData() 
        {
            // my assisting method instead of  line 26 - 31          

            var result = sumPage.SumTwoNumbers("10","10");            

            Assert.That(result, Is.EqualTo("Sum: 20"));
        }

        [Test]
        public void TestWithIncorrectData() 
        {
            Assert.That(sumPage.SumTwoNumbers("IncorrectData", "10"), Is.EqualTo("Sum: invalid input"));
            sumPage.ResetForm();
        }
              

        [TearDown]
        public void TearDown() 
        {
            driver.Quit();
            driver.Dispose();
        }

    }
}

