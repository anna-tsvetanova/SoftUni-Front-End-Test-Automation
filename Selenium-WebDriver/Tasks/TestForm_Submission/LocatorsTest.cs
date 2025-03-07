using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V131.HeadlessExperimental;

namespace NewSeleniumWebdriver
{
    public class LocatorsTest
    {
        public IWebDriver _driver;

        [SetUp]
        public void Setup() {

            var options = new ChromeOptions();
            options.AddArgument("--headless=new");
            _driver = new ChromeDriver(options);
            _driver.Navigate().GoToUrl("file:///C:/Users/annat/Downloads/03.SimpleForm%20(1)/SimpleForm/Locators.html");
        
        }

        [Test]
        public void FillTheForm_Task5()
        {
            var formTitle = _driver.FindElement(By.TagName("h2"));
            Assert.That(formTitle.Text, Is.EqualTo("Contact Form"));

            var maleButton = _driver.FindElement(By.XPath("//input[@value='m']"));
            maleButton.Click();
            Assert.That(maleButton.Selected);

            var firstNameInput = _driver.FindElement(By.XPath("//input[@id='fname']"));
            firstNameInput.Clear(); 
            firstNameInput.SendKeys("Butch");
            Assert.That(firstNameInput.GetAttribute("value"), Is.EqualTo("Butch"));

            var lastNameInput = _driver.FindElement(By.XPath("//input[@id='lname']"));
            lastNameInput.Clear();
            lastNameInput.SendKeys("Coolidge");
            Assert.That(lastNameInput.GetAttribute("value"),Is.EqualTo("Coolidge"));

            Assert.That(_driver.FindElement(By.XPath("//h3")).Displayed);

            var phoneInput = _driver.FindElement(By.CssSelector("div.additional-info>p>input[type=text]"));
            phoneInput.SendKeys("0888999777");
            Assert.That(phoneInput.GetAttribute("value"), Is.EqualTo("0888999777"));

            var newsletterCheckbox = _driver.FindElement(By.XPath("//input[@name='newsletter']"));
            newsletterCheckbox.Click();
            Assert.That(newsletterCheckbox.Selected);

            _driver.FindElement(By.XPath("//input[@type='submit']")).Click();

            
            Assert.That(_driver.FindElement(By.TagName("h1")).Text, Is.EqualTo("Thank You!")); 


        }

        [TearDown]
        public void TearDown() {
            _driver.Quit();
            _driver.Dispose();

        }

    }
}
