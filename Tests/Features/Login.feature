Feature: Login
  As a user
  I want to login to the application
  So that I can access my account

  Scenario: Valid login
    Given I am on the login page
    When I enter username "YOUR_USERNAME" and password "YOUR_PASSWORD"
    And I click login
    Then I should see the dashboard
