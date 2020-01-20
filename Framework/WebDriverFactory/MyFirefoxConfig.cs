using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using AngleSharp.Html.Parser;
using WebDriverManager.DriverConfigs.Impl;

namespace Framework.WebDriverFactory
{
    public class MyFirefoxConfig : FirefoxConfig
    {
        public override string GetName()
        {
            return "Firefox";
        }

        public override string GetUrl32()
        {
            return GetUrl();
        }

        public override string GetUrl64()
        {
            return GetUrl();
        }

        private static string GetUrl()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return "https://github.com/mozilla/geckodriver/releases/download/v<version>/geckodriver-v<version>-macos.tar.gz";
            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "https://github.com/mozilla/geckodriver/releases/download/v<version>/geckodriver-v<version>-linux64.tar.gz" : "https://github.com/mozilla/geckodriver/releases/download/v<version>/geckodriver-v<version>-win64.zip";
        }

        public override string GetBinaryName()
        {
            return "geckodriver" + (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ".exe" : string.Empty);
        }

        public override string GetLatestVersion()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            using (var webClient = new WebClient())
                return new HtmlParser()
                    .ParseDocument(webClient.DownloadString("https://github.com/mozilla/geckodriver/releases"))
                    .QuerySelectorAll(".release-header .f1 a").Select(element => element.TextContent)
                    .FirstOrDefault()?.Remove(0, 1);
        }
    }
}
