using System;
using System.IO;
using Framework.Extensions;
using Framework.WebDriverFactory;
using Microsoft.Extensions.Configuration;

namespace Framework.Common
{
    public static class ConfigReader
    {
        private const string SettingsFile = "appsettings.json";

        public static readonly string BaseDirectory = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin", StringComparison.Ordinal));
        public static readonly Browser BrowserType = Environment.GetEnvironmentVariable("BROWSER")?.GetEnumValueByDescription<Browser>() 
                                                     ?? Browser.Chrome;

        public static void SetConfig()
        {
            Config.Instance = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(SettingsFile)
                .Build()
                .Get<Config>();
        }
    }
}
