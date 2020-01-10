using System;
using System.Linq;
using System.Text.RegularExpressions;
using Framework.Extensions;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using ContextKey = Framework.Extensions.ContextKey;

namespace Framework.Base
{
    public class StepsBase
    {
        protected readonly ScenarioContext ScenarioContext;
        protected readonly IWebDriver Driver;

        protected StepsBase(ScenarioContext scenarioContext)
        {
            ScenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
            if (scenarioContext.ContainsKey(ContextKey.WebDriver))
            {
                Driver = scenarioContext.GetContextKey(ContextKey.WebDriver) as IWebDriver;
            }
        }

        protected string ReplaceStoredValues(string value)
        {
            const string storedValueRegex = @"\{(.+?)\}";
            if (Regex.IsMatch(value, storedValueRegex))
            {
                Regex.Matches(value, storedValueRegex)
                    .Select(match => match.Groups[1].Value).ToList()
                    .ForEach(
                        match => value = ScenarioContext.ContainsKey(match)
                            ? value.Replace($"{{{match}}}", ScenarioContext.GetContextKey(match).ToString())
                            : value
                    );
            }
            return value;
        }
    }
}
