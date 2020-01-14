using System;
using System.IO;
using Allure.Commons;
using Framework.Common;
using Framework.Enums;
using Framework.Extensions;
using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace Project.Hooks
{
    [Binding]
    public class Hooks : TechTalk.SpecFlow.Steps
    {
        protected static readonly Browser BrowserType = ConfigReader.BrowserType;
        protected IWebDriver Driver;
        protected new readonly ScenarioContext ScenarioContext;
        protected readonly AllureLifecycle AllureLifecycle;

        // ReSharper disable once InconsistentNaming
        // ReSharper disable once UnusedMember.Local
        private static string AllureConfigDir = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin", StringComparison.Ordinal));

        public Hooks(ScenarioContext scenarioContext)
        {
            ScenarioContext = scenarioContext;
            if (scenarioContext.ContainsKey(ContextKey.WebDriver))
            {
                Driver = scenarioContext.GetContextKey(ContextKey.WebDriver) as IWebDriver;
            }
            AllureLifecycle = AllureLifecycle.Instance;
        }

        [OneTimeSetUp]
        public void SetupForAllure()
        {
            Environment.CurrentDirectory = Path.GetDirectoryName(GetType().Assembly.Location);
        }
    }
}
