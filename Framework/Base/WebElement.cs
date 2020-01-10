using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using Framework.Common;
using Framework.Extensions;
using Framework.WebDriverFactory;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

namespace Framework.Base
{
    public class WebElement : IWebElement
    {
        private readonly IWebDriver _driver;
        private readonly IWebElement _element;

        public bool Active => GetAttribute("class").Contains("active");

        public bool Disabled => GetAttribute("disabled") != null;

        public bool Displayed => _element.Displayed;

        public bool Enabled => _element.Enabled;

        public Point Location => _element.Location;

        public bool Selected => IsSelected();

        public Size Size => _element.Size;

        public string TagName => _element.TagName;

        public string Text => _element.Text;

        public WebElement(IWebDriver driver, IWebElement webElement)
        {
            _element = webElement;
            _driver = driver;
        }

        public void AcceptAllFileTypes()
        {
            if (!ConfigReader.BrowserType.ToDescription().Contains(Browser.Safari.ToDescription()))
                return;

            ScrollIntoView();
            try
            {
                _driver.ExecuteJavaScript("arguments[0].accept = 'image/*, video/*, application/*, text/*';", _element);
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Accept All File Types exception: {exc.Message}");
            }
        }

        public void Blur()
        {
            _driver.ExecuteJavaScript("arguments[0].blur();", _element);
        }

        public void Clear()
        {
            try
            {
                _element.Clear();
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Clear Element exception: {exc.Message}");
            }
        }

        public void Click()
        {
            try
            {
                _element.Click();
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Click Element exception: {exc.Message}");
                ClickByJs();
            }
        }

        public void ClickByJs()
        {
            _driver.ExecuteJavaScript("arguments[0].click();", _element);
        }

        public void DisplayElement()
        {
            try
            {
                _driver.ExecuteJavaScript("arguments[0].style.display = 'block';", _element);
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Display Element exception: {exc.Message}");
            }
        }

        public IWebElement FindElement(By by)
        {
            return _element.FindElement(by);
        }

        public WebElement FindWebElement(By by)
        {
            return new WebElement(_driver, FindElement(by));
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return _element.FindElements(by);
        }

        public void Focus()
        {
            _driver.ExecuteJavaScript("arguments[0].focus();", _element);
        }

        public string GetAttribute(string attributeName)
        {
            try
            {
                return _element.GetAttribute(attributeName);
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Get Attribute exception: {exc.Message}");
                return _element.GetAttribute(attributeName);
            }
        }

        public string GetCssValue(string propertyName)
        {
            return _element.GetCssValue(propertyName);
        }

        public string GetProperty(string propertyName)
        {
            return _element.GetProperty(propertyName);
        }

        public string GetTextByJs()
        {
            return _driver.ExecuteJavaScript<string>("return arguments[0].textContent;", _element);
        }

        public void MoveToElement()
        {
            var actions = new Actions(_driver);
            actions.MoveToElement(_element);
            actions.Perform();
        }

        public void ScrollIntoView()
        {
            try
            {
                _driver.ExecuteJavaScript("arguments[0].scrollIntoView(true);", _element);
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Scroll Into View exception: {exc.Message}");
            }
        }

        public void SelectByText(string value)
        {
            try
            {
                new SelectElement(_element).SelectByText(value);
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Select by Text exception: {exc.Message}");
            }
        }

        public void Set(string value)
        {
            if (value.Equals(string.Empty))
            {
                SetByJs(value);
            }
            else
            {
                ClearText();
                SendKeys(value);
            }
        }

        public void SetByJs(string text)
        {
            ClearText();
            Thread.Sleep(TimeSpan.FromSeconds((int)TimeoutValue.Low));
            _driver.ExecuteJavaScript($"arguments[0].value = '{text}';", _element);
            Thread.Sleep(TimeSpan.FromSeconds((int)TimeoutValue.Low));
            SendKeys(Keys.Right);
            SendKeys(" ");
            SendKeys(Keys.Backspace);
        }

        public void SetWithDelay(string value, bool isCleared = true)
        {
            if (isCleared) ClearText();
            SendKeysWithDelay(value);
        }

        public void SendKeys(string text)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            try
            {
                _element.SendKeys(text);
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Send Keys Element exception: {exc.Message}");
            }
        }

        public void Submit()
        {
            _element.Submit();
        }

        private void ClearText()
        {
            Click();
            Clear();
            if (!ConfigReader.BrowserType.ToDescription().Contains(Browser.Safari.ToDescription()))
            {
                SendKeys(Keys.Control + "a");
            }
            else
            {
                SendKeys(Keys.Command + "a");
            }
            SendKeys(Keys.Backspace);
        }

        private bool IsSelected()
        {
            try
            {
                return _element.Selected;
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Element Is Select exception: {exc.Message}");
                return IsChecked();
            }
        }

        private bool IsChecked()
        {
            return bool.Parse(_driver.ExecuteJavaScript<string>("return arguments[0].checked;", _element));
        }

        private void SendKeysWithDelay(string text, int delay = 100)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            try
            {
                var chars = text.ToCharArray().ToList();
                chars.ForEach(ch =>
                {
                    SendKeys(new StringBuilder().Append(ch).ToString());
                    Thread.Sleep(TimeSpan.FromMilliseconds(delay));
                });
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Send Keys With Delay exception: {exc.Message}");
            }
        }
    }
}
