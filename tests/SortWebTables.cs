using System.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace SeleniumLearning
{
    [Parallelizable(ParallelScope.Self)]
    public class SortWebTables
    {
        private IWebDriver driver;

        [SetUp]
        public void StartBrowser()
        {
            string? headlessParam = TestContext.Parameters["headless"] ?? ConfigurationManager.AppSettings["headless"];
            bool headless = headlessParam == "true";

            string browserName = TestContext.Parameters["browser"] ?? ConfigurationManager.AppSettings["browser"] ?? "Edge";

            switch (browserName)
            {
                case "Chrome":
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("--no-sandbox");
                    if (headless)
                    {
                        chromeOptions.AddArgument("--headless=new");
                        chromeOptions.AddArgument("--window-size=1920,1080");
                        chromeOptions.AddArgument("--disable-dev-shm-usage");
                        chromeOptions.AddArgument("--disable-gpu");
                    }
                    driver = new ChromeDriver(chromeOptions);
                    break;
                case "Firefox":
                    var firefoxOptions = new FirefoxOptions();
                    if (headless)
                    {
                        firefoxOptions.AddArgument("--headless");
                        firefoxOptions.AddArgument("--width=1920");
                        firefoxOptions.AddArgument("--height=1080");
                    }
                    driver = new FirefoxDriver(firefoxOptions);
                    break;
                default:
                    var edgeOptions = new EdgeOptions();
                    edgeOptions.AddArgument("--no-sandbox");
                    if (headless)
                    {
                        edgeOptions.AddArgument("--headless=new");
                        edgeOptions.AddArgument("--window-size=1920,1080");
                        edgeOptions.AddArgument("--disable-dev-shm-usage");
                        edgeOptions.AddArgument("--disable-gpu");
                    }
                    driver = new EdgeDriver(edgeOptions);
                    break;
            }

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            if (!headless)
            {
                driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);
            }
            driver.Url = "https://rahulshettyacademy.com/seleniumPractise/#/offers";
        }
        [Test, Category("Regression")]
        public void SortTable()
        {
            SelectElement select = new SelectElement(driver.FindElement(By.Id("page-menu")));
            select.SelectByValue("20");

            IList<string> veggiesA = driver.FindElements(By.XPath("//td[1]"))
                .Select(veggie => veggie.Text)
                .Order()
                .ToList();

            driver.FindElement(By.CssSelector("th[aria-label*='fruit name']")).Click();
            IList<string> veggiesB = driver.FindElements(By.XPath("//td[1]"))
                .Select(i => i.Text)
                .ToList();

            Assert.That(veggiesA, Is.EqualTo(veggiesB));

        }
        [TearDown]
        public void StopBrowser()
        {
            if (driver != null)

            {
                driver.Quit();
                driver.Dispose();
            }
        }
    }
}




