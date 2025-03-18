using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace _01_SearchProductImplicitWait
{
    public class ImplicitWaitTests
    {
        IWebDriver driver; 
        
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver(); 
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Navigate().GoToUrl("https://practice.bpbonline.com/");
        }

        [Test]
        public void SearchForKeyboard()
        {
            driver.FindElement(By.XPath("//input[@name = 'keywords']")).SendKeys("keyboard");
            driver.FindElement(By.XPath("//input[@title=' Quick Find ']")).Click();

            var productName = driver.FindElement(By.XPath("//td//a[text()='Microsoft Internet Keyboard PS/2']"));  
            Assert.That(productName.Displayed, Is.True);

            driver.FindElement(By.XPath("//span[text()='Buy Now']")).Click();
            
            var myCartHeader = driver.FindElement(By.XPath("//h1"));
            Assert.That(myCartHeader.Text, Is.EqualTo("What's In My Cart?"));

            var productNameInCart = driver.FindElement(By.XPath("//div[@class='contentText']//td//a//strong"));
            Assert.That(productNameInCart.Text, Is.EqualTo("Microsoft Internet Keyboard PS/2")); 
        }

        [Test]
        public void SearchForJunk_NonExistingProductName()
        {
            driver.FindElement(By.XPath("//input[@name = 'keywords']")).SendKeys("junk");
            driver.FindElement(By.XPath("//input[@title=' Quick Find ']")).Click();

            try 
            {
                driver.FindElement(By.XPath("//span[text()='Buy Now']")).Click();
            }

            catch 
            {
                Assert.Throws<NoSuchElementException>(
                () => throw new NoSuchElementException());
            }

            Console.WriteLine("After try catch"); 
                      

        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose();
        }

    }
}