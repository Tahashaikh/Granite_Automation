using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Framework.Pages
{
    public class LoginPage
    {
        private readonly IPage page;

        public LoginPage(IPage page) => this.page = page;

        public async Task NavigateAsync() {
            await page.GotoAsync("https://pms-qa.granite.health/");
        }

        public async Task EnterUsername(string username)
        {
            await page.Locator("frame").ContentFrame.Locator("input[name=\"email\"]").ClickAsync();
            await page.Locator("frame").ContentFrame.Locator("input[name=\"email\"]").FillAsync(username);
        }
        public async Task EnterPassword(string password) { 
            await page.Locator("frame").ContentFrame.Locator("input[name=\"email\"]").ClickAsync();
            await page.Locator("frame").ContentFrame.Locator("input[name=\"password\"]").FillAsync(password);
        }

        public async Task ClickLogin() {
            await page.Locator("frame").ContentFrame.GetByRole(AriaRole.Button, new() { Name = "Submit Sign In" }).ClickAsync();
            await page.PauseAsync();
        }
        
    }
}