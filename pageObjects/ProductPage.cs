using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CSharpSelFramework.pageObjects
{
    public class ProductPage
    {
        private readonly IWebDriver _driver;
        public ProductPage(IWebDriver driver)
        {
            this._driver = driver;
        }
        // product.FindElement(By.CssSelector(".card-footer button")).Click()

        private By cardTitle = By.CssSelector(".card-title a");
        private By cardButton = By.CssSelector(".card-footer button");

        //Driver.FindElement(By.PartialLinkText("Checkout")).Click();
        private IWebElement checkOutButton => _driver.FindElement(By.PartialLinkText("Checkout"));
        private IList<IWebElement> cards => _driver.FindElements(By.TagName("app-card"));
        public IList<IWebElement> GetCards()
        {
            return cards;
        }
        public void WaitForPageDisplay()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(8));
            wait.Until(d => d.FindElement(By.PartialLinkText("Checkout")).Displayed);
        }
        public By getCardTitle()
        {
            return cardTitle;
        }
        public By AddToCartButton()
        {
           return cardButton;
        }
        public CheckOutPage CheckOut()
        {
            checkOutButton.Click();
            return new CheckOutPage(_driver);
        }
    }
}
