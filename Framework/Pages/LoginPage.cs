using Microsoft.Playwright;
using Microsoft.Extensions.Configuration;
using static Microsoft.Playwright.Assertions;

namespace Framework.Pages;

public class LoginPage
{
    private readonly IPage _page;
    private readonly string _baseUrl;

    public LoginPage(IPage page, IConfiguration config)
    {
        _page = page;
        _baseUrl = config["BaseUrl"] ?? throw new ArgumentNullException(nameof(config), "BaseUrl is not configured");
    }

    public async Task NavigateAsync()
    {
        await _page.GotoAsync(_baseUrl);
    }

    private IFrameLocator GetLoginFrame()
    {
        return _page.FrameLocator("frame");
    }

    public async Task EnterUsername(string username)
    {
        var frame = GetLoginFrame();
        await frame.Locator("input[name=\"email\"]").ClickAsync();
        await frame.Locator("input[name=\"email\"]").FillAsync(username);
    }

    public async Task EnterPassword(string password)
    {
        var frame = GetLoginFrame();
        await frame.Locator("input[name=\"password\"]").ClickAsync();
        await frame.Locator("input[name=\"password\"]").FillAsync(password);
    }

    public async Task ClickLogin()
    {
        var frame = GetLoginFrame();
        await frame.GetByRole(AriaRole.Button, new() { Name = "Submit Sign In" }).ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
    public async Task Verification()
    {
        await Expect(_page.Locator("frame").ContentFrame.Locator("#nav-tracker").GetByText("Tracker")).ToBeVisibleAsync();
    }
}