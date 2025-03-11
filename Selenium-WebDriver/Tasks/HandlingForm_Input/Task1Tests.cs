using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Task1
{
    public class Task1Tests
    {
        IWebDriver driver;   // създ.променлива

        [SetUp]
        public void Setup()
        {
                   // initialize, вдигане на driver
            driver = new ChromeDriver();  
            
                   // На browser-a за сесията, к ще отвори InplicitWait ще е 10 сек
                   //подсигуряваме, че ако има забавяне нашите тестове няма да гърмят!
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                        
                   // отиваме на конкретен web page 
            driver.Navigate().GoToUrl("http://practice.bpbonline.com/");
            
        }

        [Test]
        public void Test1()
        {
                  //Click on My Account link. Бутонът няма да ми трябва повече и няма нужда
                  //да го пазя в променлива. Chain-вам със Click и забравям за него!
            driver.FindElement(By.XPath("//span[text()='My Account']")).Click();
            
                  //Click on Continue button
            driver.FindElement(By.XPath("//span[text()='Continue']")).Click();
            // ИЛИ By.XPath("//a[@class='ui-button ui-widget ui-state-default ui-corner-all
            // ui-button-text-icon-primary ui-priority-secondary']//span[@class='ui-button-text']").Click; 

            
            // Fill in the form               //SendKeys -> попълване на инфо
            driver.FindElement(By.XPath("//input[@value='m']")).Click();
            driver.FindElement(By.XPath("//input[@name='firstname']")).SendKeys("TestFirstName");
            driver.FindElement(By.Name("lastname")).SendKeys("TestLastName");
            driver.FindElement(By.XPath("//input[@name='dob']")).SendKeys("10/10/2000");

                     // Generate Random email with random number
            Random random = new Random();
            int number = random.Next(1000, 9999);
            String email = "testEmail" + number.ToString() + "@gmail.com";

                   //CssSelector -> име на @трибута и неговата стойност        
                   //SendKeys: подавам моя email, който генерирах
            driver.FindElement(By.CssSelector("[name='email_address']")).SendKeys(email);

            driver.FindElement(By.CssSelector("[name='company']")).SendKeys("TestCompany");
            driver.FindElement(By.Name("street_address")).SendKeys("Borisova");
            driver.FindElement(By.CssSelector("[name='suburb']")).SendKeys("Ruse");
            driver.FindElement(By.XPath("//td[text()='Post Code:']/following-sibling::td//input")).SendKeys("7000");
            driver.FindElement(By.CssSelector("[name='city']")).SendKeys("Ruse");
            driver.FindElement(By.CssSelector("[name='state']")).SendKeys("Ruse");

                 //Select country from dropdown. Install NuGet package Selenium Support!
            new SelectElement(driver.FindElement(By.XPath("//select[@name='country']"))).SelectByText("Bulgaria");

            driver.FindElement(By.XPath("//input[@name='telephone']")).SendKeys("0890000000");

                                      // css -> [name='newsletter']
            driver.FindElement(By.XPath("//input[@name='newsletter']")).Click();
            driver.FindElement(By.CssSelector("[name='password']")).SendKeys("Test123");
            driver.FindElement(By.CssSelector("[name='confirmation']")).SendKeys("Test123");

            //Submit the form        //XPath //span[text()='Continue']
            driver.FindElement(By.CssSelector("[type = 'submit']")).Click();

            //Assert account creation -  succeeded!
            Assert.That(driver.FindElement(By.TagName("h1")).Text, Is.EqualTo("Your Account Has Been Created!"));

            //Click on Log Off Link
            driver.FindElement(By.XPath("//span[text()='Log Off']")).Click();

            //Click on Continue button
            driver.FindElement(By.XPath("//span[text()='Continue']")).Click();

            // Print success message to the Console
            Console.WriteLine("User Account Created with email: " + email); 

        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            // хем затваря Window на browser, хем стопира процесите, хем зачиства памет!
            // Много важно! Иначе Сrash!
            driver.Dispose();
        }
    }
}
