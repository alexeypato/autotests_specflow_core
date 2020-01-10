Feature: Home Page Test 2
	Verification of the search button text

	
@link:alexeypato+github
@UiTest
Scenario: Verifying text in search button on the Home page
	Given I am navigated to Home page
	Then 'Google Search' text is displayed in the search button on Home page
