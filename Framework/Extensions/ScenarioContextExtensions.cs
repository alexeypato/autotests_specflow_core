using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Framework.Extensions
{
    public enum ContextKey
    {
        WebDriver,
        WindowHandles
    }

    public static class ScenarioContextExtensions
    {
        public static bool ContainsKey(this ScenarioContext context, ContextKey key)
        {
            return context.ContainsKey(key.ToString());
        }

        public static object GetContextKey(this ScenarioContext context, ContextKey key)
        {
            return GetContextKey(context, key.ToString());
        }

        public static object GetContextKey(this ScenarioContext context, string key)
        {
            if (context.ContainsKey(key))
            {
                return context[key];
            }
            throw new KeyNotFoundException($"There is no '{key}' key available in ScenarioContext");
        }

        public static void SetContextKey(this ScenarioContext context, ContextKey key, object keyValue)
        {
            SetContextKey(context, key.ToString(), keyValue);
        }

        public static void SetContextKey(this ScenarioContext context, string key, object keyValue)
        {
            if (!context.ContainsKey(key))
            {
                context.Add(key, keyValue);
            }
            else
            {
                context[key] = keyValue;
            }
        }
    }
}
