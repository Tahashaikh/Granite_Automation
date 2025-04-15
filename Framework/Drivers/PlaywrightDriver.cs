using Microsoft.Playwright;
using Microsoft.Extensions.Configuration;
using System.Runtime.InteropServices;

namespace Framework.Drivers;

public class PlaywrightDriver
{
    public static IPage Page;
    public static IBrowser Browser;
    public static IBrowserContext Context;

    [DllImport("user32.dll")]
    static extern int GetSystemMetrics(int nIndex);

    private static (int width, int height) GetScreenSize()
    {
        // GetSystemMetrics(0) returns the width of the primary display monitor
        // GetSystemMetrics(1) returns the height of the primary display monitor
        return (GetSystemMetrics(0), GetSystemMetrics(1));
    }

    public static async Task InitBrowser()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();

        var browserType = config["Browser"];
        var channel = config["Channel"];
        var headless = bool.Parse(config["Headless"] ?? "true");

        // Get the actual screen dimensions
        var (screenWidth, screenHeight) = GetScreenSize();

        // Adjust for browser UI elements (title bar, scrollbars, etc.)
        var viewportWidth = screenWidth - 30;  // Account for scrollbar and window borders
        var viewportHeight = screenHeight - 150;  // Account for title bar, taskbar, and window borders

        var pw = await Playwright.CreateAsync();

        // Set up browser launch options
        var launchOptions = new BrowserTypeLaunchOptions 
        { 
            Headless = headless 
        };

        // Only set channel for Chromium if specified
        if (browserType?.ToLower() == "chromium" && !string.IsNullOrEmpty(channel))
        {
            launchOptions.Channel = channel;
        }

        // Launch browser based on type
        Browser = browserType?.ToLower() switch
        {
            "firefox" => await pw.Firefox.LaunchAsync(launchOptions),
            "webkit" => await pw.Webkit.LaunchAsync(launchOptions),
            _ => await pw.Chromium.LaunchAsync(launchOptions)
        };

        Context = await Browser.NewContextAsync(new()
        {
            ViewportSize = new ViewportSize 
            { 
                Width = viewportWidth, 
                Height = viewportHeight 
            },
            IgnoreHTTPSErrors = true
        });
        
        Page = await Context.NewPageAsync();
    }

    public static async Task CloseBrowser()
    {
        if (Browser != null)
        {
            await Browser.CloseAsync();
        }
    }
}