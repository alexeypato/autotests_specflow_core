Feature: Home Page Test 2
	Verification of the search button text

	
@link:autotests_specflow_core
@UiTest
Scenario: Verifying text in search button on the Home page
	Given I am navigated to Home page
	Then Text in the search button is displayed according to the current language on Home page
