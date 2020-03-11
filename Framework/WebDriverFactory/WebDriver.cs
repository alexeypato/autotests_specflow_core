using System;
using System.IO;
using Framework.Common;
using Framework.Enums;
using Framework.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace Framework.WebDriverFactory
{
    public static class WebDriver
    {
        private static Config ConfigInstance => Config.Instance;
        private static string DownloadsDir => $"{ConfigReader.BaseDirectory}{Path.DirectorySeparatorChar}downloads";

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
                    new DriverManager().SetUpDriver(new EdgeConfig());
                    driver = new EdgeDriver(GetEdgeOptions());
                    break;
                case Browser.Firefox:
                    new DriverManager().SetUpDriver(new FirefoxConfig());
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
                    Console.WriteLine($"Browser '{browser.ToDescription()}' is not supported");
                    throw new ArgumentOutOfRangeException($"Browser '{browser.ToDescription()}' is not supported");
            }

            return driver;
        }

        private static ChromeOptions GetChromeOptions()
        {
            var options = new ChromeOptions();
            //options.AddArgument("--headless");
            options.AddArguments("--no-sandbox", "--disable-gpu", "--disable-dev-shm-usage");
            options.AddUserProfilePreference("intl.accept_languages", ConfigInstance.Language);
            options.AddUserProfilePreference("disable-popup-blocking", "true");
            options.AddUserProfilePreference("download.prompt_for_download", "false");
            options.AddUserProfilePreference("download.default_directory", DownloadsDir);

            return options;
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

            var options = new FirefoxOptions { Profile = firefoxProfile };
            options.SetLoggingPreference(LogType.Driver, LogLevel.Off);
            options.SetLoggingPreference(LogType.Browser, LogLevel.Off);
            options.SetLoggingPreference(LogType.Client, LogLevel.Off);
            options.SetLoggingPreference(LogType.Profiler, LogLevel.Off);
            options.SetLoggingPreference(LogType.Server, LogLevel.Off);
            options.LogLevel = FirefoxDriverLogLevel.Default;
            //options.AddArgument("-headless");

            return options;
        }

        private static EdgeOptions GetEdgeOptions()
        {
            var options = new EdgeOptions()
            {
                PageLoadStrategy = PageLoadStrategy.Eager,
                UseInPrivateBrowsing = true,
            };

            return options;
        }

        private static InternetExplorerOptions GetInternetExplorerOptions()
        {
            var options = new InternetExplorerOptions
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

            return options;
        }

        private static SafariOptions GetSafariOptions()
        {
            var options = new SafariOptions();
            options.AddAdditionalCapability("cleanSession", true);
            return options;
        }
    }
}
