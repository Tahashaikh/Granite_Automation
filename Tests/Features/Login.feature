Feature: Login

  Scenario: Valid login
    Given I am on the login page
    When I enter username "admin" and password "admin123"
    And I click login
    Then I should see the dashboard
