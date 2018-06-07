using Coypu.Drivers.Selenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Configuration;

namespace browsertest_coypu_specflow_template.Drivers
{
    public class CustomRemoteWebDriver : SeleniumWebDriver
    {
        public CustomRemoteWebDriver(Coypu.Drivers.Browser browser, ICapabilities capabilities)
            : base(CustomWebDriver(capabilities), browser)
        {
        }

        private static RemoteWebDriver CustomWebDriver(ICapabilities capabilities)
        {
            var remoteAppHost = new Uri(ConfigurationManager.AppSettings.Get("SeleniumGridUrl"));
            return new RemoteWebDriver(remoteAppHost, capabilities);
        }
    }
}
