using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Framework.Common;
using Framework.Enums;
using Framework.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;

namespace Framework.Base
{
    public class PageBase
    {
        private const int DefaultTimeout = (int)TimeoutValue.High;
        private readonly IWebDriver driver;

        public PageBase(IWebDriver driver)
        {
            this.driver = driver;
        }

        public string ExecuteScript(string script, params object[] args)
        {
            return driver.ExecuteJavaScript<string>(script, args);
        }

        public T ExecuteScript<T>(string script, params object[] args)
        {
            return driver.ExecuteJavaScript<T>(script, args);
        }

        public string GetCurrentPageUrl()
        {
            return driver.Url;
        }

        public string GetDocumentHtml()
        {
            return ExecuteScript("return document.documentElement.outerHTML");
        }

        public WebElement GetElementClickable(By locator, int timeoutInSeconds = DefaultTimeout)
        {
            IWebElement element;
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            try
            {
                element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Locator: '{locator}'. Exception ---\n{exc.StackTrace}");
                throw new NoSuchElementException();
            }
            return new WebElement(driver, element);
        }

        public WebElement GetElementExists(By locator, int timeoutInSeconds = DefaultTimeout)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            IWebElement element;
            try
            {
                element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(locator));
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Locator: '{locator}'. Exception ---\n{exc.StackTrace}");
                throw new NoSuchElementException();
            }
            return new WebElement(driver, element);
        }

        public WebElement GetElementVisible(By locator, int timeoutInSeconds = DefaultTimeout)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            IWebElement element;
            try
            {
                element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Locator: '{locator}'. Exception ---\n{exc.StackTrace}");
                throw new NoSuchElementException();
            }
            return new WebElement(driver, element);
        }

        public ReadOnlyCollection<IWebElement> GetElements(By locator)
        {
            WaitForPageReadyStateComplete();
            return driver.FindElements(locator);
        }

        public string GetPageLanguage()
        {
            return ExecuteScript("return document.documentElement.lang;");
        }

        public void GoToUrl(string url)
        {
            driver.Navigate().GoToUrl(url);
        }

        public bool IsBlockVisible(IList<FieldValueDto> dataSet, By blockLocator, Func<FieldValueDto, By> getBlockItem)
        {
            var blocks = GetElements(blockLocator);
            var logBlockItems = new List<By>();
            foreach (var block in blocks)
            {
                ScrollToElement(block);
                try
                {
                    var isBlockDisplayed = dataSet.All(data =>
                    {
                        var itemLocator = getBlockItem(data);
                        logBlockItems.Add(itemLocator);
                        return block.FindElement(itemLocator).Displayed;
                    });
                    if (isBlockDisplayed)
                    {
                        return true;
                    }
                }
                catch
                {
                    // ignored
                }
            }

            if (logBlockItems.Count > 0)
                Console.WriteLine($"Item didn't found. XPath: {logBlockItems.Last()}");
            return false;
        }

        public bool IsElementNotVisible(By locator, int timeoutInSeconds = DefaultTimeout)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            var isInvisible = false;
            try
            {
                isInvisible = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(locator));
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Waiting for element {locator} to be invisible caused an exception: {exc.StackTrace}");
            }

            return isInvisible;
        }

        public bool IsElementPresent(By locator)
        {
            return GetElements(locator).Count > 0;
        }

        public bool IsElementVisible(By locator, bool isExpectedToBeVisible, int timeoutInSeconds = DefaultTimeout)
        {
            if (isExpectedToBeVisible)
            {
                return GetElementVisible(locator, timeoutInSeconds) != null;
            }
            return !IsElementNotVisible(locator, timeoutInSeconds);
        }

        public void NavigateBack()
        {
            driver.Navigate().Back();
        }

        public void OpenNewTab(ScenarioContext scenarioContext)
        {
            ExecuteScript("window.open();");
            SwitchToNewWindow(scenarioContext);
        }

        public void PressKeys(string keys)
        {
            new Actions(driver).SendKeys(GetElementVisible(By.XPath("//body")).Element, keys).Perform();
        }

        public void Refresh()
        {
            driver.Navigate().Refresh();
        }

        public void ScrollToElement(IWebElement element)
        {
            const string scrollElementInto = "arguments[0].scrollIntoView(true);"
                                             + "var viewPortHeight = Math.max(document.documentElement.clientHeight, window.innerHeight || 0);"
                                             + "var elementTop = arguments[0].getBoundingClientRect().top;"
                                             + "window.scrollBy(0, elementTop-(viewPortHeight/2));";
            try
            {
                driver.ExecuteJavaScript(scrollElementInto, element);
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Scroll To Element exception: {exc.StackTrace}");
            }
        }

        public void SwitchToDefaultContent()
        {
            driver.SwitchTo().DefaultContent();
        }

        public void SwitchToIframe(By locator)
        {
            driver.SwitchTo().Frame(GetElementClickable(locator).Element);
        }

        public void SwitchToMainWindow(ScenarioContext scenarioContext)
        {
            var handles = (IList<string>)scenarioContext.GetContextKey(ContextKey.WindowHandles);
            driver.SwitchTo().Window(handles?.First());
        }

        public void SwitchToNewWindow(ScenarioContext scenarioContext)
        {
            var handles = (IList<string>)scenarioContext.GetContextKey(ContextKey.WindowHandles);
            var newHandle = driver.WindowHandles.First(handle => !handles.Contains(handle));
            handles.Add(newHandle);
            scenarioContext.SetContextKey(ContextKey.WindowHandles, handles);
            driver.SwitchTo().Window(newHandle);
            if (ConfigReader.BrowserType.Equals(Browser.IE))
            {
                driver.Manage().Window.Maximize();
            }
        }

        public void SwitchToLastWindow(ScenarioContext scenarioContext)
        {
            var handles = (IList<string>)scenarioContext.GetContextKey(ContextKey.WindowHandles);
            driver.SwitchTo().Window(handles.Last());
        }

        public void SwitchToPreviousWindow(ScenarioContext scenarioContext)
        {
            var handles = (IList<string>)scenarioContext.GetContextKey(ContextKey.WindowHandles);
            var previousHandle = handles.Count > 2
                ? handles[^2]
                : handles[^1];
            driver.SwitchTo().Window(previousHandle);
        }

        public void WaitForPageReadyStateComplete()
        {
            Utils.WaitUntil(() => ExecuteScript("return document.readyState").Equals("complete"));
        }
    }
}
