using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;

namespace _03_WindowHandles
{
    public class Tests
    {
        
        IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver(); 
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/windows"); 
        }

        [Test]
        public void HandleMultipleWindows()
        {
            // initially 1 windowTab!
            ReadOnlyCollection<string> windowHandlesIdsBefore = driver.WindowHandles;

            // Click on the "Click Here" link to open a new window
            driver.FindElement(By.XPath("//a[text()='Click Here']")).Click();

            //Get all IDs of window handles, stored in ReadOnlyCollection --> 2 windowTabs!
            ReadOnlyCollection<string> windowHandlesIds = driver.WindowHandles;

            //switch to the new window! already have 2 IDs --> driver directed to 2tab
            driver.SwitchTo().Window(windowHandlesIds[1]);

            //Ensure there are at least 2 windows open
            Assert.That(windowHandlesIds.Count, Is.EqualTo(2));

            var newWindowTitle = driver.FindElement(By.XPath("//h3"));
            Assert.That(newWindowTitle.Text, Is.EqualTo("New Window"));

            //return focus on TAB1!!!
            driver.SwitchTo().Window(windowHandlesIds[0]);
        }

        [Test]
        public void NoSuchWindowInteraction() 
        {
            // Click on the "Click Here" link to open a new window
            driver.FindElement(By.XPath("//a[text()='Click Here']")).Click();

            //Get all IDs of window handles, stored in ReadOnlyCollection
            ReadOnlyCollection<string> windowHandles = driver.WindowHandles;

            //switch to the new window! --> driver directed to 2tab
            driver.SwitchTo().Window(windowHandles[1]);

            //Close the new Tab!
            driver.Close();

            try
            {
                //attempt to switch back to the closed window
                driver.SwitchTo().Window(windowHandles[1]);
            }
            catch (NoSuchWindowException ex)
            {
                //Log the exception
                Assert.Pass("Window was closed");
            }
            finally 
            {
                //switch back to the original window
                driver.SwitchTo().Window(windowHandles[0]);
            }
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose(); 
        }
    }
}
