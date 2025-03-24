using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace _02_SearchWithExplicitWait
{
    public class ExplicitWaitTests
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
            driver.FindElement(By.CssSelector("[name='keywords']")).SendKeys("keyboard");
            
            driver.FindElement(By.CssSelector("[alt='Quick Find']")).Click();

            //set the Implicit wait to 0
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);

            //set Explicit WAIT
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));


            var productName = wait.Until(drv => drv.FindElement(By.XPath("//td//a[text()='Microsoft Internet Keyboard PS/2']")));
            Assert.That(productName.Text, Is.EqualTo("Microsoft Internet Keyboard PS/2"));

            //Assert.That(productName.Displayed, Is.True);

            var buyButton = wait.Until(drv => drv.FindElement(By.XPath("//span[text()='Buy Now']"))); 
            buyButton.Click();

            // set back the Implicit wait to 10 sec
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                   
            var myCartHeader = driver.FindElement(By.XPath("//h1"));
            Assert.That(myCartHeader.Text, Is.EqualTo("What's In My Cart?"));

            var productNameInCart = driver.FindElement(By.XPath("//div[@class='contentText']//td//a//strong"));
            Assert.That(productNameInCart.Text, Is.EqualTo("Microsoft Internet Keyboard PS/2"));
        }

        [Test]
        public void Search_With_NonExisting_ProductName() 
        {
            driver.FindElement(By.CssSelector("[name='keywords']")).SendKeys("junk");

            driver.FindElement(By.CssSelector("[alt='Quick Find']")).Click();

            //set the Implicit wait to 0
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);

            // create WebDriverWait object --> timeout set to 10 secs  EXPLICIT WAIT 
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            try
            {
                // wait to identify the BuyNow link using XPath
                var buyNowButton = wait.Until(drv => drv.FindElement(By.XPath("//span[text()='Buy Now']")));
                buyNowButton.Click();

                //if button is found, fail the test --> as it should not exist
                Assert.Fail("Buy button was present on the page");
            }

            catch (WebDriverTimeoutException ex)
            {
                // Expected exception for non-existing product
                Assert.Pass("BuyNow button was not present in the page");
            }
            finally 
            {
                // Reset the Implicit wait because next test needs 10 sec (as in SetUp) 
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
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
