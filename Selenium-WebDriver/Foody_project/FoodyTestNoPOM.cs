using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace EXAM_Task2_FOODY
{
    public class Tests
    {
        private IWebDriver driver;

        private readonly string BaseUrl = "http://softuni-qa-loadbalancer-2137572849.eu-north-1.elb.amazonaws.com:85/";

        private string lastCreatedFoodName; 

        private string lastCreatedFoodDescription;
        private Random random; 
        
        [OneTimeSetUp]
        public void Setup()
        {
            random = new Random();
            driver = new ChromeDriver();   
            
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);

            //Login to the system
            driver.Navigate().GoToUrl(BaseUrl + "User/Login");

            driver.FindElement(By.XPath("//input[@name='Username']")).SendKeys("examPrepDiz_2");
            driver.FindElement(By.XPath("//input[@name='Password']")).SendKeys("examPrepDiz_2");
            driver.FindElement(By.XPath("//button[@type='submit']")).Click(); 

        }

        [Test, Order(1)]
        public void AddFoodWithInvalidData()
        {
            //Arrange
            string invalidFoodName = "";
            string invalidFoodDescription = "";

            driver.FindElement(By.XPath("//a[text()='Add Food']")).Click();

            //Act
            driver.FindElement(By.XPath("//input[@name='Name']")).SendKeys(invalidFoodName);
            driver.FindElement(By.XPath("//input[@name='Description']")).SendKeys(invalidFoodDescription);
            driver.FindElement(By.XPath("//button[@type='submit']")).Click();

            //Assert
            Assert.That(driver.Url, Is.EqualTo(BaseUrl + "Food/Add"));

            var errorMessage = driver.FindElement(By.XPath("//div[@class='text-danger validation-summary-errors']//li")).Text;
            Assert.That(errorMessage.Trim(), Is.EqualTo("Unable to add this food revue!")); 
        }


        [Test, Order(2)]
        public void AddRandomFoodWithValidData() 
        {
            //Arrange
            lastCreatedFoodName = "Title_" + random.Next(999, 99999).ToString();
            lastCreatedFoodDescription = "Description_" + random.Next(999, 99999).ToString();

            driver.FindElement(By.XPath("//a[text()='Add Food']")).Click();

            //Act
            driver.FindElement(By.XPath("//input[@name='Name']")).SendKeys(lastCreatedFoodName);
            driver.FindElement(By.XPath("//input[@name='Description']")).SendKeys(lastCreatedFoodDescription);
            driver.FindElement(By.XPath("//button[@type='submit']")).Click();

            //Assert
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            var homePageElement = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//h1[text()='FOODY']"))); 

            Assert.That(driver.Url, Is.EqualTo(BaseUrl));
            Assert.That(driver.Title, Is.EqualTo("Home Page - Foody.WebApp"));

            var lastCreatedFood = driver.FindElement(By.XPath("(//div[@class='col-lg-6 order-lg-1'])[last()]//h2"));
            Assert.That(lastCreatedFood.Text, Is.EqualTo(lastCreatedFoodName)); 

        }


        [Test, Order(3)]
        public void EditLastCreatedFood() 
        
        {
            //Arrange
            driver.FindElement(By.XPath("//a[text()='FOODY']")).Click();
            var editedName = "Edited";
            var lastFoodEditButton = driver.FindElement(By.XPath("(//div[@class='row gx-5 align-items-center'])[last()]//a[text()='Edit']"));

            Actions actions = new Actions(driver);
            actions.MoveToElement(lastFoodEditButton).Perform(); 

            lastFoodEditButton.Click();

            //Act
            driver.FindElement(By.XPath("//input[@name='Name']")).SendKeys(editedName);
            driver.FindElement(By.XPath("//button[@type='submit']")).Click();

            //Assert
            var lastCreatedFoodNameText = driver.FindElement(By.XPath("(//div[@class='row gx-5 align-items-center'])[last()]//h2")).Text;

            Assert.That(lastCreatedFoodNameText, Is.EqualTo(lastCreatedFoodName)); 

            Console.WriteLine("The title remains unchanged due to unimplemented functionality");
        }

        [Test, Order(4)]
        public void SearchForFoodTitle() 
        {
            //Arrange
            driver.FindElement(By.XPath("//a[text()='FOODY']")).Click();

            //Act
            driver.FindElement(By.XPath("//input[@type='search']")).SendKeys(lastCreatedFoodName);
            driver.FindElement(By.XPath("//button[@type='submit']")).Click();

            //Assert
            var wait = new WebDriverWait (driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.UrlContains("keyword=Title")); 

            var allFoodContainers = driver.FindElements(By.XPath("//div[@class='row gx-5 align-items-center']"));

            Assert.That(allFoodContainers.Count(), Is.EqualTo(1));

            var lastCreatedFoodNameText = driver.FindElement(By.XPath("(//div[@class='row gx-5 align-items-center'])[last()]//h2")).Text;

            Assert.That(lastCreatedFoodNameText, Is.EqualTo(lastCreatedFoodName));
        }

        [Test, Order(5)]
        public void DeleteLastAddedFood() 
        {
            //Arrange
            driver.FindElement(By.XPath("//a[text()='FOODY']")).Click();

            var initialCount = driver.FindElements(By.XPath("//div[@class='row gx-5 align-items-center']")).Count;

            var lastFoodContainer = driver.FindElement(By.XPath("(//div[@class='row gx-5 align-items-center'])[last()]"));

            Actions actions = new Actions(driver); 
            actions.MoveToElement(lastFoodContainer).Perform();

            Assert.That(lastFoodContainer.Enabled, Is.True);
            Assert.That(lastFoodContainer.Displayed, Is.True);

            //Act
            driver.FindElement(By.XPath("(//div[@class='row gx-5 align-items-center'])[last()]//a[text()='Delete']")).Click();

            //Asserts
            var countAfterDeletion = driver.FindElements(By.XPath("//div[@class='row gx-5 align-items-center']")).Count;

            Assert.That(countAfterDeletion, Is.EqualTo(initialCount-1)); 
        }

        [Test, Order(6)]
        public void SearchForDeletedFood() 
        {
            //Arrange
            driver.FindElement(By.XPath("//a[text()='FOODY']")).Click();
            
            //Act
            driver.FindElement(By.XPath("//input[@type='search']")).SendKeys(lastCreatedFoodName);
            driver.FindElement(By.XPath("//button[@type='submit']")).Click();

            //Asserts
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//img[@class='img-fluid rounded-circle']"))); 

            var noResultMessage = driver.FindElement(By.XPath("//h2[text()='There are no foods :(']"));
            var addFoodButton = driver.FindElement(By.XPath("//a[text()='Add food']")); 

            Assert.That(noResultMessage.Text, Is.EqualTo("There are no foods :("));
            Assert.That(addFoodButton.Displayed, Is.True); 

        }
        

        private string GenerateRandomString(string prefix)
        {
            random = new Random();
            return prefix + random.Next(999, 99999).ToString();
        }


        [OneTimeTearDown]
        public void OneTimeTearDown() 
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}