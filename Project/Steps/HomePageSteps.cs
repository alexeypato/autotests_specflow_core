using Framework.Base;
using Framework.Common;
using NUnit.Framework;
using Project.PageObjects;
using TechTalk.SpecFlow;

namespace Project.Steps
{
    [Binding]
    public class HomePageSteps : StepsBase
    {
        private readonly HomePage _homePage;

        public HomePageSteps(ScenarioContext scenarioContext) : base(scenarioContext)
        {
            _homePage = new HomePage(Driver);
        }

        [Given(@"I am navigated to Home page")]
        public void GivenIAmNavigatedToShopApplication()
        {
            _homePage.GoToUrl(Config.Instance.Url);
        }

        [Then(@"Home page is opened")]
        public void ThenHomePageIsOpened()
        {
            Assert.IsTrue(_homePage.MainImage.Displayed, "Home page is opened");
        }

        [Then(@"'(.*)' text is (displayed|hidden) in the search button on Home page")]
        public void ThenTextIsDisplayedInTheSearchButtonOnHomePage(string text, bool isExpectedToBeVisible)
        {
            var actualText = _homePage.SearchButton.GetAttribute("value");
            Assert.AreEqual(isExpectedToBeVisible, text.Equals(actualText),
                $"'{text}' expected text, '{actualText}' actual text in the search button on Home page");
        }
    }
}
