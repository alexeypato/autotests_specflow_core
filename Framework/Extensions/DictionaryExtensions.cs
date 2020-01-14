using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
            throw new KeyNotFoundException($"'{key}' key was not present in the dictionary");
        }

        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, Func<TValue>> dictionary, TKey key)
        {
            return dictionary.GetValue<TKey, Func<TValue>>(key).Invoke();
        }

        public static bool DoesContainSubset(this Dictionary<string, List<string>> subsetDictionary, Dictionary<string, List<string>> supersetDictionary)
        {
            return subsetDictionary.Keys.All(key => supersetDictionary.ContainsKey(key) && subsetDictionary[key]
                                                        .All(value => supersetDictionary[key].Contains(value)));
        }

        public static void Add<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value, bool rewriteExisting = false)
        {
            if (rewriteExisting && dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }
    }
}
