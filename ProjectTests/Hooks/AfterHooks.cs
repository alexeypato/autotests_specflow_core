using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Allure.Commons.Model;
using Framework.Common;
using Framework.Extensions;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace ProjectTests.Hooks
{
    [Binding]
    public class AfterHooks : Hooks
    {
        public AfterHooks(ScenarioContext scenarioContext) : base(scenarioContext) { }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            CreateAllureEnvFile();
        }

        [AfterScenario]
        public void TearDown()
        {
            if (ScenarioContext.TestError != null)
            {
                var path = Utils.MakeScreenShot(Driver);
                AllureLifecycle.AddAttachment(path);
            }

            Driver.Quit();
            Driver.Dispose();

            AllureHackForScenarioOutlineTests();
        }

        [AfterStep]
        public void LogStepResult()
        {
            var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(LevelOfParallelismAttribute), false);
            if (attributes.Length > 0)
            {
                if (attributes[0] is LevelOfParallelismAttribute attribute)
                {
                    var levelOfParallelism = (int)attribute.Properties.Get(attribute.Properties.Keys.First());

                    if (levelOfParallelism > 1)
                    {
                        var stepText = StepContext.StepInfo.StepDefinitionType + " " + StepContext.StepInfo.Text;
                        Console.WriteLine(stepText);
                        var stepTable = StepContext.StepInfo.Table;
                        if (stepTable != null && stepTable.ToString() != string.Empty) Console.WriteLine(stepTable);
                        var error = ScenarioContext.TestError;
                        Console.WriteLine(error != null ? "-> error: " + error.Message : "-> done.");
                    }
                }
            }
        }

        private void AllureHackForScenarioOutlineTests()
        {
            ScenarioContext.TryGetValue(out TestResult result);
            AllureLifecycle.UpdateTestCase(result.uuid, test =>
            {
                test.name = ScenarioContext.ScenarioInfo.Title;
                test.historyId = Guid.NewGuid().ToString();
            });
        }

        private static void CreateAllureEnvFile()
        {
            new XDocument(
                    new XElement("environment",
                        new XElement("parameter",
                            new XElement("key", "OS"),
                            new XElement("value", System.Runtime.InteropServices.RuntimeInformation.OSDescription)
                        ),
                        new XElement("parameter",
                            new XElement("key", "Browser"),
                            new XElement("value", BrowserType.ToDescription())
                        )
                    )
                )
                .Save(string.Format("{0}{1}allure-results{1}environment.xml", Directory.GetCurrentDirectory(), Path.DirectorySeparatorChar));
        }
    }
}
