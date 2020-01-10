using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Framework.Extensions;
using OpenQA.Selenium;

namespace Framework.Common
{
    public static class Utils
    {
        private const string PossibleChars = "abcdefghijklmnopqrstuvwxyz1234567890";

        public static string DecodeBase64(string encodedString)
        {
            var data = Convert.FromBase64String(encodedString.Replace("-", "+").Replace("_", "/"));
            return Encoding.UTF8.GetString(data);
        }

        public static string EncodeBase64(string stringToEncode)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(stringToEncode));
        }

        public static string GetRandomString(int length = 20)
        {
            var random = new Random();
            return new string(Enumerable.Repeat(PossibleChars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string MakeScreenShot(IWebDriver driver, string testName = "screen", ScreenshotImageFormat imageFormat = ScreenshotImageFormat.Png)
        {
            var projectPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var screen = ((ITakesScreenshot)driver).GetScreenshot();
            var fileLocation = $"{projectPath}{Path.DirectorySeparatorChar}{testName}.{imageFormat.ToString().ToLower()}";
            screen.SaveAsFile(fileLocation, imageFormat);
            return fileLocation;
        }

        public static bool WaitUntil(Func<bool> condition, TimeoutValue timeout = TimeoutValue.High, double delay = 0.5)
        {
            var watch = DateTime.Now.ToEpoch();

            while (watch + (int)timeout > DateTime.Now.ToEpoch())
            {
                if (condition()) return true;
                Thread.Sleep(TimeSpan.FromSeconds(delay));
            }

            return false;
        }
    }
}
