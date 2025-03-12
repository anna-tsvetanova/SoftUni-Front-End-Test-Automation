using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;

namespace Task2
{
    public class Task2Tests
    {
        IWebDriver driver; 

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();    

            //Add implicit Wait. Waiting until element searching
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            
            //Launch Chrome browser with given URL
            driver.Navigate().GoToUrl("https://practice.bpbonline.com/"); 

        }

        [Test]
        public void Test1()
        {
            //Identify the web table on the home page
            IWebElement tableElement = driver.FindElement(By.TagName("table"));

            //Find the number of rows    // 3 rows with 3 elements each
            ReadOnlyCollection<IWebElement> tableRows = tableElement.FindElements(By.XPath("//tbody//tr"));

            //Path to save the CSV file   //IO -> Input/Output
            string path = System.IO.Directory.GetCurrentDirectory() + "productInformation.csv";

            //If the file exists in the location, delete it
            //each test run generates a fresh CSV file with current data only!
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            //Traverse through table rows to find the table columns
            //Loop through each row and then each column within the row. 
            //return "td" -> table data (for 1row only)
            foreach (var tableRow in tableRows) 
            {
                ReadOnlyCollection<IWebElement> tableData = tableRow.
                    FindElements(By.TagName("td"));

                //Return 3 "td" in 1 row
                foreach (var currentTableData in tableData) 
                {
                    //Extract product name and cost
                    //Extract the text from each cell, split the text to
                    //separate product name[0] and cost[1], and format it.

                    String data = currentTableData.Text;
                    String[] productInfo = data.Split('\n'); 
                    String productInfoToWrite = productInfo[0] + "," + 
                        productInfo[1].Trim() + "\n";

                    //Write product information extracted to the file
                    File.AppendAllText(path, productInfoToWrite);

                }
            }

            //Verify the file was created and has content
            Assert.That(File.Exists(path), Is.True, "CSV file was not created");
            Assert.That(new FileInfo(path).Length, Is.GreaterThan(0), "CSV file is empty");

        }

        [TearDown]
        public void TearDown() 
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}

