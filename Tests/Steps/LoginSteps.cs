using Framework.Pages;
using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Tests.Steps
{
    [Binding]
    public class LoginSteps
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IPage _page;
        private LoginPage _loginPage;


        [BeforeScenario]
        public async Task Setup()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            var context = await _browser.NewContextAsync();
            _page = await context.NewPageAsync();
            _loginPage = new LoginPage(_page);
        }

        [Given(@"I am on the login page")]
        public async Task GivenIAmOnTheLoginPage() => await _loginPage.NavigateAsync();

        [When(@"I enter username ""(.*)"" and password ""(.*)""")]
        public async Task WhenIEnterUsernameAndPassword(string username, string password)
        {
            await _loginPage.EnterUsername(username);
            await _loginPage.EnterPassword(password);
        }

        [When(@"I click login")]
        public async Task WhenIClickLogin() => await _loginPage.ClickLogin();

        [Then(@"I should see the dashboard")]
        public async Task ThenIShouldSeeTheDashboard()
        {
            var content = await _page.InnerTextAsync("h1");
            Assert.That(content, Does.Contain("Tracker"));
        }

        [AfterScenario]
        public async Task TearDown()
        {
            await _page.CloseAsync();
            await _browser.CloseAsync();
        }
    } 
}
