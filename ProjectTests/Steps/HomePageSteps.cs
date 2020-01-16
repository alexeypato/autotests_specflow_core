﻿using Framework.Base;
using Framework.Common;
using NUnit.Framework;
using ProjectTests.PageObjects;
using TechTalk.SpecFlow;

namespace ProjectTests.Steps
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

        [Then(@"Text in the search button is (displayed|hidden) according to the current language on Home page")]
        public void ThenTextInTheSearchButtonIsDisplayedAccordingToTheCurrentLanguageOnHomePage(bool isExpectedToBeVisible)
        {
            var expectedText = _homePage.GetSearchButtonTitle();
            var actualText = _homePage.SearchButton.GetAttribute("value");
            Assert.AreEqual(isExpectedToBeVisible, expectedText.Equals(actualText),
                $"'{expectedText}' expected text, '{actualText}' actual text in the search button on Home page");
        }
    }
}