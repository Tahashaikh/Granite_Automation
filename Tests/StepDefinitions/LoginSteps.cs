using Framework.Drivers;
using Framework.Pages;
using FluentAssertions;
using TechTalk.SpecFlow;
using Microsoft.Extensions.Configuration;

namespace Tests.Steps;

[Binding]
public class LoginSteps
{
    private LoginPage? _loginPage;
    private readonly IConfiguration _config;
    private readonly ScenarioContext _scenarioContext;

    public LoginSteps(ScenarioContext scenarioContext)
    {
        _config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        _scenarioContext = scenarioContext;
    }

    [Given(@"I am on the login page")]
    public async Task GivenIAmOnTheLoginPage()
    {
        await PlaywrightDriver.InitBrowser();
        _loginPage = new LoginPage(PlaywrightDriver.Page, _config);
        await _loginPage.NavigateAsync();
    }

    [When(@"I enter username ""([^""]*)"" and password ""([^""]*)""")]
    public async Task WhenIEnterUsernameAndPassword(string username, string password)
    {
        if (_loginPage == null) throw new InvalidOperationException("LoginPage is not initialized");

        // Use values from config if placeholders are provided
        var actualUsername = username == "YOUR_USERNAME" 
            ? _config["TestData:Username"] ?? throw new InvalidOperationException("Username not found in config") 
            : username;
            
        var actualPassword = password == "YOUR_PASSWORD" 
            ? _config["TestData:Password"] ?? throw new InvalidOperationException("Password not found in config") 
            : password;

        await _loginPage.EnterUsername(actualUsername);
        await _loginPage.EnterPassword(actualPassword);
    }

    [When(@"I click login")]
    public async Task WhenIClickLogin()
    {
        if (_loginPage == null) throw new InvalidOperationException("LoginPage is not initialized");
        await _loginPage.ClickLogin();
    }

    [Then(@"I should see the dashboard")]
    public async Task ThenIShouldSeeTheDashboard()
    {
        // We'll implement dashboard verification later
        await Task.CompletedTask;
    }

    [AfterScenario]
    public async Task AfterScenario()
    {
        await PlaywrightDriver.CloseBrowser();
    }
}