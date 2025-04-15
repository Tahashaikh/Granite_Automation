using TechTalk.SpecFlow;
using Allure.Net.Commons;

namespace Tests.Hooks;

[Binding]
public class Hooks
{
    [BeforeTestRun]
    public static void BeforeTestRun()
    {
        AllureLifecycle.Instance.CleanupResultDirectory();
    }
}