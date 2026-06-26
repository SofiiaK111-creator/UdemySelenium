using CSharpSelFramework.pageObjects;
using CSharpSelFramework.utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
// for terminal
//dotnet test CSharpSelFramework.csproj --filter TestCategory=Smoke -- 'TestRunParameters.Parameter(name=\"browserName\", value=\"Edge\")'


namespace CSharpSelFramework.tests
{
    [Parallelizable(ParallelScope.Self)]
    
    public class E2Etest : Base
    {

        [Test, TestCaseSource("AddTestDataConfig"!), Category("Smoke")]
        //[Test]
        //[TestCase("rahulshettyacademy", "Learning@830$3mK2")]
        [Parallelizable(ParallelScope.All)]

        public void EndToEndFlow(string userName, string password, string[] expectedProducts)
        {
            LoginPage loginPage = new LoginPage(Driver);
            string[] actualProducts;

            //driver.FindElement(By.Id("username")).SendKeys("rahulshettyacademy");
            //Driver.FindElement(By.Name("password")).SendKeys("Learning@830$3mK2");
            //Driver.FindElement(By.XPath("//div[@class='form-group'][5]/label/span/input"))
            //      .Click();
            //Driver.FindElement(By.CssSelector("input[value='Sign In']")).Click();

            ProductPage productPage = loginPage.ValidLogin(userName, password);
            productPage.WaitForPageDisplay();
            IList<IWebElement> products = productPage.GetCards();

            //WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(8));
            //wait.Until(d => d.FindElement(By.PartialLinkText("Checkout")).Displayed);
            //IList<IWebElement> products = Driver.FindElements(By.TagName("app-card"));

            foreach (IWebElement product in products)
            {

                if (expectedProducts.Contains(product
                                    .FindElement(productPage.getCardTitle()).Text))
                {
                    product.FindElement(productPage.AddToCartButton()).Click();
                }
            }

            //Driver.FindElement(By.PartialLinkText("Checkout")).Click();
            CheckOutPage checkOutPage = productPage.CheckOut();

            IList<IWebElement> checkoutCards = checkOutPage.GetCards();
            actualProducts = new string[checkoutCards.Count];

            for (int i = 0; i < checkoutCards.Count; i++)
            {
                actualProducts[i] = checkoutCards[i].Text;
            }
            Assert.That(expectedProducts, Is.EqualTo(actualProducts));

            //Driver.FindElement(By.CssSelector(".btn-success")).Click();
            ConfirmationPage confirmationPage = checkOutPage.CheckOut();

            //Driver.FindElement(By.CssSelector("#country")).SendKeys("ind");
            //wait.Until(d => d.FindElement(By.LinkText("India")).Displayed);
            //Driver.FindElement(By.LinkText("India")).Click();
            //Driver.FindElement(By.CssSelector("label[for='checkbox2']")).Click();
            //Driver.FindElement(By.CssSelector("input[value='Purchase']")).Click();
            confirmationPage.EnterCountry("ind");
            IWebElement optionInDropDown = confirmationPage.WaitForDropDown("India");
            optionInDropDown.Click();
            confirmationPage.ClickCheckBox();
            confirmationPage.ClickPurchaseButton();
            string confirmationText = confirmationPage.GetConfirmationText();
            Assert.That(confirmationText, Does.Contain("Success"));
        }
        [Test, Category("Regression")]
        public void LocatorsIdentification()
        {
            Driver.FindElement(By.Id("username")).SendKeys("rahulshettyacademy");
            Driver.FindElement(By.Id("username")).Clear();
            Driver.FindElement(By.Id("username")).SendKeys("rahulshetty");
            Driver.FindElement(By.Name("password")).SendKeys("12345");
            Driver.FindElement(By.XPath("//div[@class='form-group'][5]/label/span/input")).Click();
            Driver.FindElement(By.CssSelector("input[value='Sign In']")).Click();
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(8));
            wait.Until(d => d.FindElement(By.Id("signInBtn")).GetAttribute("value") == "Sign In");

            string errorMessage = Driver.FindElement(By.ClassName("alert-danger")).Text;
            TestContext.Progress.WriteLine(errorMessage);
        }
        public static IEnumerable<TestCaseData> AddTestDataConfig()
        {
            yield return new TestCaseData(GetDataParser().ExtractData("username"), GetDataParser()
                .ExtractData("password"), GetDataParser().ExtractDataArray("products"));
            yield return new TestCaseData(GetDataParser().ExtractData("username"), GetDataParser()
                .ExtractData("password"), GetDataParser().ExtractDataArray("products"));
            yield return new TestCaseData(GetDataParser().ExtractData("username_wrong"), GetDataParser()
                .ExtractData("password_wrong"), GetDataParser().ExtractDataArray("products"));
        }


    }
}
