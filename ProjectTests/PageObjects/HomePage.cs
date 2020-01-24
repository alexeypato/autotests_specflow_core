using Framework.Base;
using Framework.Common;
using Framework.Enums;
using Framework.Extensions;
using log4net;
using OpenQA.Selenium;

namespace ProjectTests.PageObjects
{
    public class HomePage : PageBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(HomePage));
        private Lang Lang => GetElementExists(By.XPath("/html[@itemscope]")).GetAttribute("lang")
            .GetEnumValueByDescription<Lang>();

        public WebElement MainImage => GetElementVisible(By.XPath("//img[@alt='Google']"));

        public HomePage(IWebDriver driver) : base(driver) { }

        public string GetSearchTitle()
        {
            return GetElementClickable(By.XPath("//div[not(@jsname)]/center/input[@name='btnK']"))
                .GetAttribute("value");
        }

        public string GetSearchExpectedTitle()
        {
            Logger.Info($"Browser is '{ConfigReader.BrowserType.ToDescription()}'");
            switch (Lang)
            {
                case Lang.Be:
                    return "Пошук Google";
                case Lang.Fr:
                    return "Recherche Google";
                case Lang.RuBy:
                    return "Поиск в Google";
                default:
                    return "Google Search";
            }
        }
    }
}
