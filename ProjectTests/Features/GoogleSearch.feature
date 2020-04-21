Feature: Performing a Google Search

    As a user on the Google search page
    I want to search for Selenium-Webdriver
    Because I want to learn more about it

Background:
    Given I am navigated to Google page

@UiTest
Scenario: Searching for unknown term
    When I set '124334sdgdsafgsdfg2323424' to the search field
        And  I press 'Enter'
    Then I expect that search element is hidden

@UiTest
Scenario Outline: Searching for term "<searchItem>"
    When I set '<searchItem>' to the search field
        And  I press 'Enter'
    Then I expect that search element is displayed

Examples:
    | searchItem         |
    | Selenium Webdriver |
    | Docker             |

@UiTest
Scenario: Verifying text in search button on the Google page
	Then Text in the search button is displayed according to the current language
