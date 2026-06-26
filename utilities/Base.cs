using System.Configuration;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Model;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;


namespace CSharpSelFramework.utilities
{
    public abstract class Base
    {
        public ExtentReports extent;
        public ExtentTest test;
        //private IWebDriver _driver;
        public ThreadLocal<IWebDriver> _driver = new();
        public IWebDriver Driver => _driver.Value!;
        string browserName;
        [OneTimeSetUp]
        public void Setup()
        {
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory)?.Parent?.Parent?.FullName ?? workingDirectory;
            string reportPath = Path.Combine(projectDirectory, "index.html");
            var htmlReporter = new ExtentSparkReporter(reportPath);
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
            extent.AddSystemInfo("Host name", "Local host");
            extent.AddSystemInfo("Environment", "QA");
            extent.AddSystemInfo("User name", "Sofiia Kostiuk");


        }
        [SetUp]

        public void StartBrowser()

        {
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
            browserName = TestContext.Parameters["browserName"]!;
            if (browserName == null)
            {
                browserName = ConfigurationManager.AppSettings["browser"]!;
            }
            string? headlessParam = TestContext.Parameters["headless"] ?? ConfigurationManager.AppSettings["headless"];
            bool headless = headlessParam == "true";
            InitBrowser(browserName!, headless);
            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            if (!headless)
            {
                _driver.Value!.Manage().Window.Maximize();
            }
            _driver.Value!.Url = "https://rahulshettyacademy.com/loginpagePractise/";
        }

        public void InitBrowser(string BrowserName, bool headless = false)
        {

            switch (BrowserName)
            {
                case "Firefox":
                    var firefoxOptions = new FirefoxOptions();
                    if (headless)
                    {
                        firefoxOptions.AddArgument("--headless");
                        firefoxOptions.AddArgument("--width=1920");
                        firefoxOptions.AddArgument("--height=1080");
                    }
                    _driver.Value = new FirefoxDriver(firefoxOptions);
                    break;
                case "Chrome":
                    var chromeOptions = new ChromeOptions();
                    if (headless)
                    {
                        chromeOptions.AddArgument("--headless=new");
                        chromeOptions.AddArgument("--window-size=1920,1080");
                        chromeOptions.AddArgument("--no-sandbox");
                        chromeOptions.AddArgument("--disable-dev-shm-usage");
                    }
                    _driver.Value = new ChromeDriver(chromeOptions);
                    break;
                case "Edge":
                    var edgeOptions = new EdgeOptions();
                    if (headless)
                    {
                        edgeOptions.AddArgument("--headless=new");
                        edgeOptions.AddArgument("--window-size=1920,1080");
                        edgeOptions.AddArgument("--no-sandbox");
                        edgeOptions.AddArgument("--disable-dev-shm-usage");
                    }
                    _driver.Value = new EdgeDriver(edgeOptions);
                    break;
            }
        }

        public static JsonReader GetDataParser()
        {
            return new JsonReader();
        }
        [TearDown]
        public void AfterTest()
        {
            var stackTrace = TestContext.CurrentContext.Result.StackTrace;
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            DateTime time = DateTime.Now;
            string fileName = "Screenshot_" + time.ToString("hh_mm_ss") + ".png";
            if (status == TestStatus.Failed)
            {
                if (_driver.Value != null)
                {
                    test.Fail("Test failed ", captureScreenShot(_driver.Value, fileName));
                }
                test.Log(Status.Fail, "test failed with logtrace " + stackTrace);
            }
            else if (status == TestStatus.Passed)
            {


            }
            extent.Flush();

            if (_driver.Value != null)
            {
                _driver.Value.Quit();
                _driver.Value.Dispose();
            }


        }
        public Media captureScreenShot(IWebDriver driver, string screenShotName)
        {
            ITakesScreenshot ts = (ITakesScreenshot)driver;
            string screenshot = ts.GetScreenshot().AsBase64EncodedString;
            return MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot, screenShotName).Build();
        }
    }
}
