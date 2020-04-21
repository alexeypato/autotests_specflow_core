using Framework.Base;
using Framework.Enums;
using Framework.Extensions;
using OpenQA.Selenium;

namespace ProjectTests.PageObjects
{
    public class GooglePage : PageBase
    {
        private Lang Lang => GetElementExists(By.TagName("html")).GetAttribute("lang")
            .GetEnumValueByDescription<Lang>();

        public WebElement MainImage => GetElementVisible(By.XPath("//div[@id='hplogo']"));

        public GooglePage(IWebDriver driver) : base(driver) { }

        public string GetSearchTitle()
        {
            return GetElementClickable(By.XPath("//div[not(@jsname)]/center/input[@name='btnK']"))
                .GetAttribute("value");
        }

        public string GetSearchExpectedTitle()
        {
            return Lang switch
            {
                Lang.Be => "Пошук Google",
                Lang.Fr => "Recherche Google",
                Lang.Nl => "Google zoeken",
                Lang.RuBy => "Поиск в Google",
                _ => "Google Search"
            };
        }

        public bool IsSearchElementDisplayed(bool falseCase)
        {
            return IsElementVisible(By.XPath("//*[@id='search']//div"), falseCase);
        }

        public void SetSearchField(string value)
        {
            GetElementClickable(By.CssSelector("[name=q]")).Set(value);
        }
    }
}
