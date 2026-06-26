using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using AventStack.ExtentReports.MarkupUtils;
using AventStack.ExtentReports.Model;
using AventStack.ExtentReports.Reporter;
using ICSharpCode.SharpZipLib.Core;
using Microsoft.Testing.Platform.Configurations;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;


namespace CSharpSelFramework.utilities
{
    public class Base
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
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
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
            InitBrowser(browserName!);
            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            _driver.Value!.Manage().Window.Maximize();
            _driver.Value!.Url = "https://rahulshettyacademy.com/loginpagePractise/";
        }

        public void InitBrowser(string BrowserName)
        {

            switch (BrowserName)
            {
                case "Firefox":
                    _driver.Value = new FirefoxDriver();
                    break;
                case "Chrome":
                    _driver.Value = new ChromeDriver();
                    break;
                case "Edge":
                    _driver.Value = new EdgeDriver();
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
                test.Fail("Test failed ", captureScreenShot(_driver.Value!, fileName));
                test.Log(Status.Fail, "test failed with logtrace " + stackTrace);
            }
            else if (status == TestStatus.Passed)
            {


            }
            extent.Flush();

            {
                _driver.Value!.Quit();
                _driver.Value!.Dispose();
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
