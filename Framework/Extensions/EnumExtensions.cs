using System;
using System.ComponentModel;

namespace Framework.Extensions
{
    public static class EnumExtensions
    {
        public static string ToDescription(this Enum value)
        {
            var attributes = (DescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            throw new ArgumentException($"Description for the value {value} has not been provided");
        }
    }
}
