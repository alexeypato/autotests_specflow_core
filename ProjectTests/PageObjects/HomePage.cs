using Framework.Base;
using Framework.Enums;
using Framework.Extensions;
using OpenQA.Selenium;

namespace ProjectTests.PageObjects
{
    public class HomePage : PageBase
    {
        private Lang Lang => GetElementExists(By.TagName("html")).GetAttribute("lang")
            .GetEnumValueByDescription<Lang>();

        public WebElement MainImage => GetElementVisible(By.XPath("//div[@id='hplogo']"));

        public HomePage(IWebDriver driver) : base(driver) { }

        public string GetSearchTitle()
        {
            return GetElementClickable(By.XPath("//div[not(@jsname)]/center/input[@name='btnK']"))
                .GetAttribute("value");
        }

        public string GetSearchExpectedTitle()
        {
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
