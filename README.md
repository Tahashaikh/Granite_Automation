# Granite Automation Tests

This repository contains automated tests for the Granite application using SpecFlow and Allure for reporting.

## Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download) installed on your machine.
- Allure command-line tool installed. You can find installation instructions [here](https://docs.qameta.io/allure/#_installing_a_commandline).

## Commands

### Running Tests
To run the tests, navigate to the `Tests` directory and execute:
```bash
cd D:\Rider\Granite_Automation\Tests
dotnet test
```

### Viewing Allure Reports
To view the Allure report after running the tests, execute:
```bash
allure serve "D:\Rider\Granite_Automation\Tests\allure-results"
```

## Writing Tests
- Tests are written in Gherkin syntax in the `Features` directory. Each feature file corresponds to a specific functionality of the application.
- Step definitions are located in the `StepDefinitions` directory, where you can map Gherkin steps to C# methods.

## Project Structure
- **Features/**: Contains Gherkin feature files.
- **Hooks/**: Contains hooks for Allure reporting and SpecFlow lifecycle management.
- **StepDefinitions/**: Contains C# methods that implement the steps defined in the feature files.
- **allure.runsettings**: Configuration file for Allure reporting.
- **specflow.json**: Configuration file for SpecFlow.

## Notes
- Ensure that your tests are properly annotated with the necessary attributes for Allure reporting.
- Use unique names for scenarios to avoid conflicts in reporting.

For any further questions or issues, feel free to reach out!
