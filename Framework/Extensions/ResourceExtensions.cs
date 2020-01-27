using System.Collections;
using System.Globalization;
using System.Linq;
using System.Resources;

namespace Framework.Extensions
{
    public static class ResourceExtensions
    {
        public static string GetCurrentCultureValue(this ResourceManager resourceManager, string enValue)
        {
            return resourceManager.GetString(resourceManager.GetKeyByEnValue(enValue));
        }

        private static string GetKeyByEnValue(this ResourceManager resourceManager, string value)
        {
            var resourceSet = resourceManager.GetResourceSet(CultureInfo.GetCultureInfo("EN"), true, true);
            var key = resourceSet.OfType<DictionaryEntry>().FirstOrDefault(e => e.Value.ToString() == value).Key;
            if (key != null)
            {
                return key.ToString();
            }
            throw new System.ArgumentNullException($"Can not find key by value: {value} for EN localization");
        }
    }
}
