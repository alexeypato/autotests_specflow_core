using Framework.Base;
using TechTalk.SpecFlow;

namespace ProjectTests.Steps.CommonSteps
{
    [Binding]
    public class CommonSteps : StepsBase
    {
        private readonly PageBase pageBase;

        public CommonSteps(ScenarioContext scenarioContext) : base(scenarioContext)
        {
            pageBase = new PageBase(Driver);
        }

        [When(@"I press '(.*)'")]
        public void WhenIPress(string keys)
        {
            pageBase.PressKeys(keys);
        }

    }
}
