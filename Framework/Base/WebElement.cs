using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using Framework.Common;
using Framework.Enums;
using Framework.Extensions;
using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

namespace Framework.Base
{
    public class WebElement : IWebElement
    {
        private readonly IWebDriver _driver;
        private static readonly ILog Logger = LogManager.GetLogger(typeof(WebElement));

        public readonly IWebElement Element;

        public bool Active => GetAttribute("class").Contains("active");

        public bool Disabled => GetAttribute("disabled") != null;

        public bool Displayed => Element.Displayed;

        public bool Enabled => Element.Enabled;

        public Point Location => Element.Location;

        public bool Selected => IsSelected();

        public Size Size => Element.Size;

        public string TagName => Element.TagName;

        public string Text => Element.Text;

        public WebElement(IWebDriver driver, IWebElement webElement)
        {
            Element = webElement;
            _driver = driver;
        }

        public void AcceptAllFileTypes()
        {
            if (!ConfigReader.BrowserType.ToDescription().Contains(Browser.Safari.ToDescription()))
                return;

            ScrollIntoView();
            try
            {
                _driver.ExecuteJavaScript("arguments[0].accept = 'image/*, video/*, application/*, text/*';", Element);
            }
            catch (Exception exc)
            {
                Logger.Error($"Accept All File Types exception: {exc.StackTrace}");
            }
        }

        public void Blur()
        {
            _driver.ExecuteJavaScript("arguments[0].blur();", Element);
        }

        public void Clear()
        {
            try
            {
                Element.Clear();
            }
            catch (Exception exc)
            {
                Logger.Error($"Clear Element exception: {exc.StackTrace}");
            }
        }

        public void Click()
        {
            try
            {
                Element.Click();
            }
            catch (Exception exc)
            {
                Logger.Error($"Click Element exception: {exc.StackTrace}");
                ClickByJs();
            }
        }

        public void ClickByJs()
        {
            _driver.ExecuteJavaScript("arguments[0].click();", Element);
        }

        public void DisplayElement()
        {
            try
            {
                _driver.ExecuteJavaScript("arguments[0].style.display = 'block';", Element);
            }
            catch (Exception exc)
            {
                Logger.Error($"Display Element exception: {exc.StackTrace}");
            }
        }

        public IWebElement FindElement(By by)
        {
            return Element.FindElement(by);
        }

        public WebElement FindWebElement(By by)
        {
            return new WebElement(_driver, FindElement(by));
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return Element.FindElements(by);
        }

        public void Focus()
        {
            _driver.ExecuteJavaScript("arguments[0].focus();", Element);
        }

        public string GetAttribute(string attributeName)
        {
            try
            {
                return Element.GetAttribute(attributeName);
            }
            catch (Exception exc)
            {
                Logger.Error($"Get Attribute exception: {exc.StackTrace}");
                return Element.GetAttribute(attributeName);
            }
        }

        public string GetCssValue(string propertyName)
        {
            return Element.GetCssValue(propertyName);
        }

        public string GetProperty(string propertyName)
        {
            return Element.GetProperty(propertyName);
        }

        public string GetTextByJs()
        {
            return _driver.ExecuteJavaScript<string>("return arguments[0].textContent;", Element);
        }

        public void MoveToElement()
        {
            var actions = new Actions(_driver);
            actions.MoveToElement(Element);
            actions.Perform();
        }

        public void ScrollIntoView()
        {
            try
            {
                _driver.ExecuteJavaScript("arguments[0].scrollIntoView(true);", Element);
            }
            catch (Exception exc)
            {
                Logger.Error($"Scroll Into View exception: {exc.StackTrace}");
            }
        }

        public void SelectByText(string value)
        {
            try
            {
                new SelectElement(Element).SelectByText(value);
            }
            catch (Exception exc)
            {
                Logger.Error($"Select by Text exception: {exc.StackTrace}");
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
            _driver.ExecuteJavaScript($"arguments[0].value = '{text}';", Element);
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
                Element.SendKeys(text);
            }
            catch (Exception exc)
            {
                Logger.Error($"Send Keys Element exception: {exc.StackTrace}");
            }
        }

        public void Submit()
        {
            Element.Submit();
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
                return Element.Selected;
            }
            catch (Exception exc)
            {
                Logger.Error($"Element Is Select exception: {exc.StackTrace}");
                return IsChecked();
            }
        }

        private bool IsChecked()
        {
            return bool.Parse(_driver.ExecuteJavaScript<string>("return arguments[0].checked;", Element));
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
                Logger.Error($"Send Keys With Delay exception: {exc.StackTrace}");
            }
        }
    }
}
