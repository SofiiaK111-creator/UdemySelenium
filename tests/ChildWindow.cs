using CSharpSelFramework.utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumLearning
{
    [Parallelizable(ParallelScope.Self)]
    public class ChildWindow:Base
    {
        [Test]
        public void workWithChildWindow()
        {
            string emailForLogIn = "mentor@rahulshettyacademy.com";
            string homePage = Driver.CurrentWindowHandle;
            Driver.FindElement(By
                  .LinkText("Free Access to InterviewQues/ResumeAssistance/Material"))
                  .Click();
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.WindowHandles.Count == 2);
            string childWindowName = Driver.WindowHandles.First(h => h != homePage);
            Driver.SwitchTo().Window(childWindowName);
            wait.Until(d => d.FindElement(By.CssSelector(".red")).Displayed);
            TestContext.Progress.WriteLine(Driver.FindElement(By.CssSelector(".red")).Text);
            string allText = Driver.FindElement(By.CssSelector(".red")).Text;
            string[] parts =  allText.Split("at");
            string emailPart = parts[1];
            // mentor@rahulshettyacademy.com with below templ
            string[] partsTwo = emailPart.Trim().Split(" ");
            string email = partsTwo[0];
            Assert.That(email, Is.EqualTo(emailForLogIn));

            Driver.SwitchTo().Window(homePage);
            Driver.FindElement(By.ClassName("form-control")).SendKeys(email);



        }
    }
}

