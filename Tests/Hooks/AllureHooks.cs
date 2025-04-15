using Allure.Net.Commons;
using TechTalk.SpecFlow;

namespace Tests.Hooks;

[Binding]
public class AllureHooks
{
    private readonly ScenarioContext _scenarioContext;
    private readonly FeatureContext _featureContext;
    private readonly AllureLifecycle _allureLifecycle;
    private bool _stepFailed;
    private static string? _currentFeatureUuid;

    public AllureHooks(ScenarioContext scenarioContext, FeatureContext featureContext)
    {
        _scenarioContext = scenarioContext;
        _featureContext = featureContext;
        _allureLifecycle = AllureLifecycle.Instance;
        _stepFailed = false;
    }

    [BeforeTestRun]
    public static void BeforeTestRun()
    {
        AllureLifecycle.Instance.CleanupResultDirectory();
    }

    [BeforeStep]
    public void BeforeStep()
    {
        var stepInfo = _scenarioContext.StepContext.StepInfo;
        var stepResult = new StepResult
        {
            name = stepInfo.Text,
            status = _stepFailed ? Status.skipped : Status.passed,
            statusDetails = new StatusDetails
            {
                message = _stepFailed ? "Skipped" : "Passed"
            }
        };

        _allureLifecycle.StartStep(stepResult);
    }

    [AfterStep]
    public void AfterStep()
    {
        if (_scenarioContext.TestError != null)
        {
            _stepFailed = true;
            _allureLifecycle.UpdateStep(step =>
            {
                step.status = Status.failed;
                step.statusDetails = new StatusDetails
                {
                    message = "Failed"
                };
            });
        }
        else if (_stepFailed)
        {
            _allureLifecycle.UpdateStep(step =>
            {
                step.status = Status.skipped;
                step.statusDetails = new StatusDetails
                {
                    message = "Skipped"
                };
            });
        }
        else
        {
            _allureLifecycle.UpdateStep(step => 
            {
                step.status = Status.passed;
                step.statusDetails = new StatusDetails
                {
                    message = "Passed"
                };
            });
        }

        _allureLifecycle.StopStep();
    }

    [BeforeFeature]
    public static void BeforeFeature(FeatureContext featureContext)
    {
        _currentFeatureUuid = featureContext.FeatureInfo.Title;
    }

    [BeforeScenario]
    public void BeforeScenario()
    {
        _stepFailed = false;
        var scenarioInfo = _scenarioContext.ScenarioInfo;
        
        // Create a truly unique ID for each scenario
        string uniqueId = $"{scenarioInfo.Title}_{DateTime.Now.Ticks}";
        
        var testResult = new TestResult
        {
            uuid = uniqueId,
            historyId = $"{_featureContext.FeatureInfo.Title}.{scenarioInfo.Title}",
            name = scenarioInfo.Title,
            fullName = $"{_featureContext.FeatureInfo.Title}.{scenarioInfo.Title}",
            labels = new List<Label>
            {
                Label.Feature(_featureContext.FeatureInfo.Title),
                Label.Story(scenarioInfo.Title),
                Label.Suite(_featureContext.FeatureInfo.Title)
            },
            statusDetails = new StatusDetails
            {
                message = "Running"
            }
        };

        _allureLifecycle.StartTestCase(testResult);
    }

    [AfterScenario]
    public void AfterScenario()
    {
        if (_scenarioContext.TestError != null)
        {
            _allureLifecycle.UpdateTestCase(testResult => 
            {
                testResult.status = Status.failed;
                testResult.statusDetails = new StatusDetails
                {
                    message = "Failed",
                    trace = _scenarioContext.TestError.StackTrace
                };
            });
        }
        else
        {
            _allureLifecycle.UpdateTestCase(testResult => 
            {
                testResult.status = Status.passed;
                testResult.statusDetails = new StatusDetails
                {
                    message = "Passed"
                };
            });
        }

        _allureLifecycle.StopTestCase();
        _allureLifecycle.WriteTestCase();
    }
}
