using System;
using Framework.Base;
using Framework.Common;
using NUnit.Framework;
using ProjectTests.PageObjects;
using TechTalk.SpecFlow;

namespace ProjectTests.Steps
{
    [Binding]
    public class GooglePageSteps : StepsBase
    {
        private readonly GooglePage googlePage;

        public GooglePageSteps(ScenarioContext scenarioContext) : base(scenarioContext)
        {
            googlePage = new GooglePage(Driver);
        }

        [Given(@"I am navigated to Google page")]
        public void GivenIAmNavigatedToGooglePage()
        {
            googlePage.GoToUrl(Config.Instance.Url);
        }

        [When(@"I set '(.*)' to the search field")]
        public void WhenISetToTheSearchField(string value)
        {
            googlePage.SetSearchField(value);
        }

        [When(@"I press Search")]
        public void WhenIPressSearch()
        {
            googlePage.PressSearch();
        }

        [Then(@"I expect that search element is (displayed|hidden)")]
        public void ThenIExpectThatSearchElementBecomesNotDisplayed(bool falseCase)
        {
            Assert.AreEqual(falseCase, googlePage.IsSearchElementDisplayed(falseCase),
               $"I expect that search element has {falseCase} visibility state");
        }

        [Then(@"Google page is opened")]
        public void ThenGooglePageIsOpened()
        {
            Assert.IsTrue(googlePage.MainImage != null, "Home page is opened");
        }

        [Then(@"Text in the search button is (displayed|hidden) according to the current language")]
        public void CheckSearchButtonText(bool isExpectedToBeVisible)
        {
            var pageLanguage = googlePage.GetPageLanguage();
            Console.WriteLine($@"Current Website Culture is '{pageLanguage}'");
            var expectedText = googlePage.GetSearchExpectedTitle();
            var actualText = googlePage.GetSearchTitle();
            Assert.AreEqual(isExpectedToBeVisible, expectedText.Equals(actualText),
                $"'{expectedText}' expected text, '{actualText}' actual text in the search button");
        }
    }
}
