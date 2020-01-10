using Framework.Base;
using OpenQA.Selenium;

namespace Project.PageObjects
{
    public class HomePage : PageBase
    {
        public WebElement MainImage => GetElementVisible(By.XPath("//img[@alt='Google']"));
        public WebElement SearchButton =>
            GetElementClickable(By.XPath("//div[not(@jsname)]/center/input[@name='btnK']"));

        public HomePage(IWebDriver driver) : base(driver) { }

    }
}
