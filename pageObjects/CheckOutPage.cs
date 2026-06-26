using OpenQA.Selenium;

namespace CSharpSelFramework.pageObjects
{
    public class CheckOutPage
    {
        private readonly IWebDriver _driver;
        public CheckOutPage(IWebDriver driver)
        {
            this._driver = driver;
        }
        // IList<IWebElement> checkoutCards = Driver.FindElements(By.CssSelector("h4 a"));
        private IList<IWebElement> checkoutCards => _driver.FindElements(By.CssSelector("h4 a"));

        //Driver.FindElement(By.CssSelector(".btn-success")).Click();
        private IWebElement checkOutButton => _driver.FindElement(By.CssSelector(".btn-success"));

        public ConfirmationPage CheckOut()
        {
            checkOutButton.Click();
            return new ConfirmationPage(_driver);

        }
        public IList<IWebElement> GetCards()
        {
            return checkoutCards;
        }
        
    }
}
