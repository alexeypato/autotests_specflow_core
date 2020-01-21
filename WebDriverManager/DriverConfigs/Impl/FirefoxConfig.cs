using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using AngleSharp.Html.Parser;

namespace WebDriverManager.DriverConfigs.Impl
{
    public class FirefoxConfig : IDriverConfig
    {
        public virtual string GetName()
        {
            return "Firefox";
        }

        public virtual string GetUrl32()
        {
            return GetUrl();
        }

        public virtual string GetUrl64()
        {
            return GetUrl();
        }

        private static string GetUrl()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return "https://github.com/mozilla/geckodriver/releases/download/v<version>/geckodriver-v<version>-macos.tar.gz";
            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "https://github.com/mozilla/geckodriver/releases/download/v<version>/geckodriver-v<version>-linux32.tar.gz" : "https://github.com/mozilla/geckodriver/releases/download/v<version>/geckodriver-v<version>-win64.zip";
        }

        public virtual string GetBinaryName()
        {
            return "geckodriver" + (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ".exe" : string.Empty);
        }

        public virtual string GetLatestVersion()
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
