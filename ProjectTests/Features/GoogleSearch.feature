﻿Feature: Performing a Google Search

    As a user on the Google search page
    I want to search for Selenium-Webdriver
    Because I want to learn more about it

Background:
    Given I am navigated to Google page

@UiTest
Scenario: Searching for unknown term
    When I set 'waw4f34f34erv3434we4aw4w4wa4aw4g44' to the search field
        And  I press Search
    Then I expect that search element is hidden

@UiTest
Scenario Outline: Searching for known term
    When I set '<searchItem>' to the search field
        And  I press Search
    Then I expect that search element is displayed

Examples:
    | searchItem         |
    | Selenium Webdriver |
    | Docker             |

@UiTest
Scenario: Verifying text in search button on the Google page
	Then Text in the search button is displayed according to the current language
