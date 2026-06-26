using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using CSharpSelFramework.pageObjects;

namespace CSharpSelFramework.pageObjects
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        public LoginPage(IWebDriver driver)
        {
            this._driver = driver;
        }

        //driver.FindElement(By.Id("username")).SendKeys("rahulshettyacademy");
        //Driver.FindElement(By.Name("password")).SendKeys("Learning@830$3mK2");
        //Driver.FindElement(By.XPath("//div[@class='form-group'][5]/label/span/input"))
        //          .Click();
        //Driver.FindElement(By.CssSelector("input[value='Sign In']")).Click();
        
        private IWebElement usernameField => _driver.FindElement(By.Id("username"));
        private IWebElement passwordField => _driver.FindElement(By.Name("password"));
        private IWebElement checkbox => _driver.FindElement(By.CssSelector("#terms"));
        private IWebElement signInButton => _driver.FindElement(By.CssSelector("input[value='Sign In']"));


        public ProductPage ValidLogin (string userName, string userPassword)
        {
            usernameField.SendKeys(userName);
            passwordField.SendKeys(userPassword);
            checkbox.Click();
            signInButton.Click();
            return new ProductPage(_driver);
        }
    }
}
