using Framework.Base;
using Framework.Enums;
using Framework.Extensions;
using OpenQA.Selenium;

namespace Project.PageObjects
{
    public class HomePage : PageBase
    {
        private Lang Lang => GetElementExists(By.XPath("/html[@itemscope]")).GetAttribute("lang")
            .GetEnumValueByDescription<Lang>();

        public WebElement MainImage => GetElementVisible(By.XPath("//img[@alt='Google']"));
        public WebElement SearchButton =>
            GetElementClickable(By.XPath("//div[not(@jsname)]/center/input[@name='btnK']"));

        public HomePage(IWebDriver driver) : base(driver) { }

        public string GetSearchButtonTitle()
        {
            switch (Lang)
            {
                case Lang.Be:
                    return "Пошук Google";
                case Lang.RuBy:
                    return "Поиск в Google";
                default:
                    return "Google Search";
            }
        }
    }
}
