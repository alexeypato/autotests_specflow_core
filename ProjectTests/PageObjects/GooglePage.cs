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
        private WebElement SearchButton =>
            GetElementClickable(By.XPath("//div[not(@jsname)]/center/input[@name='btnK']"));

        public WebElement MainImage => GetElementVisible(By.XPath("//div[@id='hplogo']"));

        public GooglePage(IWebDriver driver) : base(driver) { }

        public string GetSearchTitle()
        {
            return SearchButton.GetAttribute("value");
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

        public void PressSearch()
        {
            SearchButton.Click();
        }

        public void SetSearchField(string value)
        {
            var element = GetElementClickable(By.CssSelector("[name=q]"));
            element.Set(value);
        }
    }
}