using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Framework.Extensions
{
    public static class StringExtensions
    {
        public static TEnum GetEnumValueByDescription<TEnum>(this string description)
        {
            var type = typeof(TEnum);
            if (!type.IsEnum)
            {
                throw new InvalidOperationException($"Provided type '{type}' does not represent an Enum");
            }
            foreach (var field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute
                    && attribute.Description.Equals(description))
                {
                    return (TEnum)field.GetValue(null);
                }
            }
            throw new ArgumentException($"Enum value with description '{description}' was not found");
        }

        public static string ReplaceSpaces(this string value, string newValue)
        {

            return value.Replace(" ", newValue);
        }

        public static List<int> SplitByColon(this string value)
        {
            return value.Split(":").Select(item => Convert.ToInt32(item)).ToList();
        }

        public static List<string> SplitByDot(this string value)
        {
            return value.Split(".").ToList();
        }

        public static List<string> SplitByComma(this string value)
        {
            return value.Split(",").ToList();
        }
    }
}
