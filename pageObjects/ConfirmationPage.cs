using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CSharpSelFramework.pageObjects
{
    public class ConfirmationPage
    {
        private readonly IWebDriver _driver;
        public ConfirmationPage(IWebDriver driver)
        {
            this._driver = driver;
        }

        //Driver.FindElement(By.CssSelector("#country")).SendKeys("ind");
        //wait.Until(d => d.FindElement(By.LinkText("India")).Displayed);
        //Driver.FindElement(By.LinkText("India")).Click();
        //Driver.FindElement(By.CssSelector("label[for='checkbox2']")).Click();
        //Driver.FindElement(By.CssSelector("input[value='Purchase']")).Click();
        //string confirmationText = Driver.FindElement(By.CssSelector(".alert-dismissible"))
        //         .Text;
        private IWebElement checkBox =>_driver.FindElement(By.CssSelector("label[for='checkbox2']"));
        private IWebElement purchaseButton => _driver.FindElement(By.CssSelector("input[value='Purchase']"));
        private IWebElement countryField => _driver.FindElement(By.CssSelector("#country"));
        private IWebElement confirmation => _driver.FindElement(By.CssSelector(".alert-dismissible"));
        public void EnterCountry (string keyWord)
        {
            countryField.SendKeys(keyWord);
        }
        public IWebElement WaitForDropDown(string expectedTextInDropDown)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(8));
            wait.Until(d => d.FindElement(By.LinkText(expectedTextInDropDown)).Displayed);
            return _driver.FindElement(By.LinkText(expectedTextInDropDown));
        }

        public void ClickCheckBox()
        {
            checkBox.Click();
        }

        public void ClickPurchaseButton()
        {
            purchaseButton.Click();
        }

        public string GetConfirmationText()
        {
            return confirmation.Text;
        }
    }
}
