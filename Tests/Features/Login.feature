Feature: Login

  Scenario: Valid login
    Given I am on the login page
    When I enter username "marlene@wisb.com" and password "123456Aa!"
    And I click login
    Then I should see the dashboard
