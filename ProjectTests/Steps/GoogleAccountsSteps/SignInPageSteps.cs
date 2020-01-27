using Framework.Base;
using ProjectTests.PageObjects.GoogleAccounts;
using ProjectTests.Resources.Google;
using TechTalk.SpecFlow;

namespace ProjectTests.Steps.GoogleAccountsSteps
{
    [Binding]
    public class SignInPageSteps : StepsBase
    {
        private readonly SignInPage _signInPage;

        public SignInPageSteps(ScenarioContext scenarioContext) : base(scenarioContext)
        {
            _signInPage = new SignInPage(Driver);
        }

        [When(@"I start to create a new account from Google Sign In page")]
        public void WhenIClickCreateAccountButtonOnGoogleSignInPage()
        {
            _signInPage.ClickCreateAccount();
            _signInPage.ChooseAccountType(SignInPageRes.ForMyself);
        }

    }
}
