using Framework.Base;
using OpenQA.Selenium;
using ProjectTests.Resources.Google;

namespace ProjectTests.PageObjects.GoogleAccounts
{
    public class SignInPage : PageBase
    {
        public SignInPage(IWebDriver driver) : base(driver) { }

        public void ChooseAccountType(string menuItem)
        {
            var elementLocator = By.XPath($"//*[normalize-space()='{menuItem}']");
            GetElementClickable(elementLocator).Click();
            //if (IsElementPresent(elementLocator))
            //{
            //    GetElementClickable(elementLocator).Click();
            //}
        }

        public void ClickCreateAccount()
        {
            GetElementClickable(By.XPath($"//*[normalize-space()='{SignInPageRes.CreateAccount}']")).Click();
        }
    }
}
