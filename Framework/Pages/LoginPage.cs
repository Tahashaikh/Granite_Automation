using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Framework.Pages
{
    public class LoginPage
    {
        private readonly IPage _page;

        public LoginPage(IPage page) => _page = page;

        public async Task NavigateAsync() {
            await _page.GotoAsync("https://pms-qa.granite.health/"); 
        }

        public async Task EnterUsername(string username) {
            await _page.FillAsync("#email", username); 
        }

        public async Task EnterPassword(string password) { 
            await _page.FillAsync("#password", password); 
        }

        public async Task ClickLogin() {
            await _page.ClickAsync("#login"); 
        }
    }
}
