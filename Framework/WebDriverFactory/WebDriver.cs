using System;
using System.IO;
using Framework.Common;
using Framework.Enums;
using Framework.Extensions;
using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace Framework.WebDriverFactory
{
    public static class WebDriver
    {
        private static Config ConfigInstance => Config.Instance;
        private static string DownloadsDir => $"{ConfigReader.BaseDirectory}{Path.DirectorySeparatorChar}downloads";
        private static readonly ILog Logger = LogManager.GetLogger(typeof(WebDriver));

        public static IWebDriver GetWebDriver(Browser browser)
        {
            IWebDriver driver;
            switch (browser)
            {
                case Browser.Chrome:
                    new DriverManager().SetUpDriver(new ChromeConfig());
                    driver = new ChromeDriver(GetChromeOptions());
                    break;
                case Browser.Edge:
                    new DriverManager().SetUpDriver(new LegacyEdgeConfig());
                    driver = new EdgeDriver(GetEdgeOptions());
                    break;
                case Browser.Firefox:
                    new DriverManager().SetUpDriver(new FirefoxConfig(), architecture: Architecture.X64);
                    driver = new FirefoxDriver(GetFirefoxOptions());
                    break;
                case Browser.IE:
                    new DriverManager().SetUpDriver(new InternetExplorerConfig());
                    driver = new InternetExplorerDriver(GetInternetExplorerOptions());
                    break;
                case Browser.Safari:
                    driver = new SafariDriver(GetSafariOptions());
                    break;
                default:
                    Logger.Error($"Browser '{browser.ToDescription()}' is not supported");
                    throw new ArgumentOutOfRangeException($"Browser '{browser.ToDescription()}' is not supported");
            }

            return driver;
        }

        private static ChromeOptions GetChromeOptions()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddUserProfilePreference("intl.accept_languages", ConfigInstance.Language);
            chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");
            chromeOptions.AddUserProfilePreference("download.prompt_for_download", "false");
            chromeOptions.AddUserProfilePreference("download.default_directory", DownloadsDir);

            return chromeOptions;
        }

        private static FirefoxOptions GetFirefoxOptions()
        {
            var firefoxProfile = new FirefoxProfile();
            firefoxProfile.SetPreference("browser.download.folderList", 2);
            firefoxProfile.AcceptUntrustedCertificates = true;
            firefoxProfile.SetPreference("browser.download.dir", DownloadsDir);
            firefoxProfile.SetPreference("browser.download.useDownloadDir", true);
            firefoxProfile.SetPreference("browser.download.manager.closeWhenDone", true);
            firefoxProfile.SetPreference("browser.download.manager.showAlertOnComplete", false);
            firefoxProfile.SetPreference("browser.download.manager.alertOnEXEOpen", false);
            firefoxProfile.SetPreference("browser.download.manager.focusWhenStarting", false);
            firefoxProfile.SetPreference("browser.download.manager.useWindow", false);
            firefoxProfile.SetPreference("browser.download.panel.shown", false);
            firefoxProfile.SetPreference("pdfjs.disabled", true);
            firefoxProfile.SetPreference("browser.helperApps.neverAsk.saveToDisk",
                "application/excel;charset=utf-8;application/msword; charset=utf-8; application/x-zip-compressed; text/csv");
            firefoxProfile.SetPreference("profile.assume_untrusted_issuer", true);
            firefoxProfile.SetPreference("profile.accept_untrusted_certs", true);
            firefoxProfile.SetPreference("dom.successive_dialog_time_limit", 0);
            firefoxProfile.SetPreference("intl.accept_languages", ConfigInstance.Language);

            var firefoxOptions = new FirefoxOptions { Profile = firefoxProfile };
            firefoxOptions.SetLoggingPreference(LogType.Driver, LogLevel.Off);
            firefoxOptions.SetLoggingPreference(LogType.Browser, LogLevel.Off);
            firefoxOptions.SetLoggingPreference(LogType.Client, LogLevel.Off);
            firefoxOptions.SetLoggingPreference(LogType.Profiler, LogLevel.Off);
            firefoxOptions.SetLoggingPreference(LogType.Server, LogLevel.Off);
            firefoxOptions.LogLevel = FirefoxDriverLogLevel.Debug;

            return firefoxOptions;
        }

        private static EdgeOptions GetEdgeOptions()
        {
            var edgeOptions = new EdgeOptions()
            {
                PageLoadStrategy = PageLoadStrategy.Eager,
                UseInPrivateBrowsing = true,
            };

            return edgeOptions;
        }

        private static InternetExplorerOptions GetInternetExplorerOptions()
        {
            var internetExplorerOptions = new InternetExplorerOptions
            {
                BrowserCommandLineArguments = "-private",
                ElementScrollBehavior = InternetExplorerElementScrollBehavior.Bottom,
                EnableNativeEvents = true,
                EnsureCleanSession = true,
                IgnoreZoomLevel = true,
                IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                PageLoadStrategy = PageLoadStrategy.Eager,
                RequireWindowFocus = true
            };

            return internetExplorerOptions;
        }

        private static SafariOptions GetSafariOptions()
        {
            var options = new SafariOptions();
            options.AddAdditionalCapability(CapabilityType.AcceptSslCertificates, true);
            options.AddAdditionalCapability(CapabilityType.AcceptInsecureCertificates, true);
            options.AddAdditionalCapability("cleanSession", true);
            return options;
        }
    }
}
