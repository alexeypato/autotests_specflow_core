using System.Collections.Generic;
using Allure.Commons;
using Framework.Common;
using Framework.Extensions;
using Framework.WebDriverFactory;
using OpenQA.Selenium.Support.Extensions;
using TechTalk.SpecFlow;
using ContextKey = Framework.Extensions.ContextKey;

namespace Project.Hooks
{
    [Binding]
    public class BeforeHooks : Hooks
    {
        public BeforeHooks(ScenarioContext scenarioContext) : base(scenarioContext) { }

        [BeforeTestRun]
        public static void CreateTestRun()
        {
            ConfigReader.SetConfig();
        }

        [BeforeScenario]
        public void Setup()
        {
            InitializeWebDriver();
            AllureLifecycle.Instance.SetCurrentTestActionInException(() =>
            {
                var screen = Driver.TakeScreenshot().AsByteArray;
                AllureLifecycle.Instance.AddAttachment("Screenshot", AllureLifecycle.AttachFormat.ImagePng, screen);
            });
        }

        private void InitializeWebDriver()
        {
            Driver = WebDriver.GetWebDriver(BrowserType);
            Driver.Manage().Cookies.DeleteAllCookies();
            Driver.Manage().Window.Maximize();
            ScenarioContext.SetContextKey(ContextKey.WebDriver, Driver);
            ScenarioContext.SetContextKey(ContextKey.WindowHandles, new List<string> { Driver.CurrentWindowHandle });
        }
    }
}
