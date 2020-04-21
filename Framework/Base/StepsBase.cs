using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Framework.Common;
using Framework.Extensions;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
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

        protected T ConvertToJsonObject<T>(Table table) where T : new()
        {
            var dataSet = table.CreateSet<FieldValueDto>().ToList();

            var obj = new T();
            var savedNodeName = string.Empty;

            foreach (var data in dataSet)
            {
                var elements = data.Field.SplitByDot();

                object parentNode = obj;
                PropertyInfo nodeToUpdate = null;
                var isEmptyList = false;
                for (var i = 0; i < elements.Count; i++)
                {
                    //if the parameter contains '[]', so we need to create an inner list of the elements
                    var regex = Regex.Match(elements[i], @"!?\[[a-z]\]").Value;

                    var type = parentNode?.GetType();

                    if (string.IsNullOrEmpty(regex))    //there are no inner lists
                    {
                        var emptyListRegex = Regex.Match(elements[i], @"!?\[\]").Value;
                        if (!string.IsNullOrEmpty(emptyListRegex))
                        {
                            nodeToUpdate = type?.GetProperty(elements[i].Replace(emptyListRegex, string.Empty));
                            isEmptyList = true;
                            continue;
                        }

                        nodeToUpdate = type?.GetProperty(elements[i]);

                        if (i < elements.Count - 1)
                        {
                            if (nodeToUpdate?.GetValue(parentNode, null) == null)
                            {
                                nodeToUpdate?.SetValue(parentNode, Activator.CreateInstance(nodeToUpdate.PropertyType));
                            }
                            parentNode = nodeToUpdate?.GetValue(parentNode, null);
                        }

                        isEmptyList = false;
                    }
                    else
                    {
                        //get a collection type
                        nodeToUpdate = type?.GetProperty(elements[i].Replace(regex, string.Empty));

                        //initialize the collection
                        if (nodeToUpdate?.GetValue(parentNode, null) == null)
                            nodeToUpdate?.SetValue(parentNode, Activator.CreateInstance(nodeToUpdate.PropertyType));

                        //move pointer to the inner node
                        parentNode = nodeToUpdate?.GetValue(parentNode, null);

                        //add new item to the collection in case '[]' value has been changed
                        if (!savedNodeName.Equals(elements[i]))
                        {
                            savedNodeName = elements[i];

                            if (parentNode != null)
                            {
                                var item = Activator.CreateInstance(parentNode.GetType().GetGenericArguments().Single());
                                var addItemMethod = nodeToUpdate.PropertyType.GetMethod("Add");
                                addItemMethod?.Invoke(parentNode, new[] { item });
                            }
                        }

                        //get the index of the last added item
                        var countMethod = nodeToUpdate?.PropertyType.GetMethod("get_Count");
                        if (countMethod != null)
                        {
                            var index = (int)countMethod.Invoke(parentNode, null);

                            //move to the item of the collection, that will be updated
                            var getItemMethod = nodeToUpdate.PropertyType.GetMethod("get_Item");
                            parentNode = getItemMethod?.Invoke(parentNode, new object[] { --index });
                        }

                        //get a property of the item, that will be updated
                        nodeToUpdate = parentNode?.GetType().GetProperty(elements[++i]);
                        isEmptyList = false;
                    }
                }
                var value = ReplaceKnownKeywords(data.Value);

                if (isEmptyList)
                {
                    if (nodeToUpdate != null)
                    {
                        var instance = Activator.CreateInstance(nodeToUpdate.PropertyType);
                        var list = (IList)instance;
                        if (!value.Equals(string.Empty))
                        {
                            value.SplitByComma()
                                .ForEach(listItem => list.Add(ChangeType(listItem, nodeToUpdate?.PropertyType.GenericTypeArguments[0])));
                        }
                        nodeToUpdate.SetValue(parentNode, list);
                    }

                    continue;
                }

                if (nodeToUpdate != null)
                    nodeToUpdate.SetValue(parentNode, ChangeType(value, nodeToUpdate.PropertyType));
            }
            return obj;
        }

        protected string ReplaceKnownKeywords(string value)
        {
            var matchesToReplace = Regex.Matches(value, "{[A-Za-z0-9-+_=>]*}");
            foreach (var matchToReplace in matchesToReplace)
            {
                var valueToReplace = matchToReplace.ToString();
                if (valueToReplace.Equals("{GenerateEmail}"))
                {
                    var generatedEmail = string.Format(Config.Instance.RegistrationEmailTemplate, Guid.NewGuid().ToString().Substring(24));
                    Console.WriteLine($"Generated email: {generatedEmail}");
                    value = value.Replace(valueToReplace, generatedEmail);
                    if (ScenarioContext.ContainsKey(ContextKey.GeneratedEmail))
                    {
                        ScenarioContext.SetContextKey(ContextKey.PreviousEmail, ScenarioContext.GetContextKey(ContextKey.GeneratedEmail));
                    }
                    ScenarioContext.SetContextKey(ContextKey.GeneratedEmail, value);
                }
                else if (valueToReplace.StartsWith("{=>"))
                {
                    var keyName = Regex.Match(valueToReplace, "{=>(.*)}").Groups[1].Value;
                    value = value.Replace(valueToReplace, Utils.GetRandomString());
                    ScenarioContext.SetContextKey(keyName, value);
                }
            }
            return ReplaceStoredValues(value);
        }

        private static object ChangeType(object value, Type conversion)
        {
            var targetType = conversion;

            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (value == null)
                {
                    return null;
                }

                targetType = Nullable.GetUnderlyingType(targetType);
            }

            return Convert.ChangeType(value, targetType);
        }

        private string ReplaceStoredValues(string value)
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
