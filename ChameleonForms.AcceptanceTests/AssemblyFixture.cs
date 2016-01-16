using System;
using System.IO;
using System.Reflection;
using OpenQA.Selenium.Remote;
using TestStack.Seleno.Configuration;

namespace ChameleonForms.AcceptanceTests
{
    public static class Host
    {
        private static readonly RemoteWebDriver Browser = BrowserFactory.FireFox();
        public static readonly SelenoHost Instance = new SelenoHost();

        static Host()
        {
            Instance.Run("ChameleonForms.Example", 12345, c => c
                .WithMinimumWaitTimeoutOf(TimeSpan.FromSeconds(1))
                .UsingCamera(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", "")), "screenshots"))
                .WithRemoteWebDriver(() => Browser)
            );
        }
    }
}