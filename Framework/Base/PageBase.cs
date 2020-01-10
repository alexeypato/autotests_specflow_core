using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

namespace Framework.Base
{
    public class PageBase
    {
        private const int DefaultTimeout = 30;
        private readonly IWebDriver _driver;

        public PageBase(IWebDriver driver)
        {
            _driver = driver;
        }

        public T ExecuteScript<T>(string script, params object[] args)
        {
            return _driver.ExecuteJavaScript<T>(script, args);
        }

        public WebElement GetElementClickable(By locator, int timeoutInSeconds = DefaultTimeout)
        {
            IWebElement element = null;
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
            try
            {
                element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            return new WebElement(_driver, element);
        }

        public WebElement GetElementExists(By locator, int timeoutInSeconds = DefaultTimeout)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
            IWebElement element = null;
            try
            {
                element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(locator));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            return new WebElement(_driver, element);
        }

        public WebElement GetElementVisible(By locator, int timeoutInSeconds = DefaultTimeout)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
            IWebElement element = null;
            try
            {
                element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            return new WebElement(_driver, element);
        }

        public void GoToUrl(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }

        public void ScrollToElement(IWebElement element)
        {
            const string scrollElementInto = "arguments[0].scrollIntoView(true);"
                                             + "var viewPortHeight = Math.max(document.documentElement.clientHeight, window.innerHeight || 0);"
                                             + "var elementTop = arguments[0].getBoundingClientRect().top;"
                                             + "window.scrollBy(0, elementTop-(viewPortHeight/2));";
            try
            {
                _driver.ExecuteJavaScript(scrollElementInto, element);
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Scroll To Element exception: {exc.Message}");
            }
        }
    }
}
