using Framework.Base;
using TechTalk.SpecFlow;

namespace ProjectTests.Steps.CommonSteps
{
    [Binding]
    public class NavigateSteps : StepsBase
    {
        private readonly PageBase _pageBase;

        public NavigateSteps(ScenarioContext scenarioContext) : base(scenarioContext)
        {
            _pageBase = new PageBase(Driver);
        }

        [Given(@"I am navigated to '(.*)' page")]
        public void GivenIAmNavigatedToPage(string url)
        {
            _pageBase.GoToUrl(url);
        }

    }
}
