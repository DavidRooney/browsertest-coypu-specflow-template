using browsertest_coypu_specflow_template.Drivers;
using Coypu;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Configuration;

namespace browsertest_coypu_specflow_template.Helpers
{
    public static class BrowserSessionHelper
    {
        private const int SessionTimeoutSec = 30;

        private static Constants.BrowserName BrowserConfig
        {
            get
            {
                return ConfigurationManager.AppSettings["browser"].ToBrowserName();
            }
        }

        public static BrowserSession GetLocalBrowserSession()
        {
            Coypu.Drivers.Browser browser;

            switch (BrowserConfig)
            {
                case Constants.BrowserName.Chrome:
                    browser = Coypu.Drivers.Browser.Chrome;
                    break;
                case Constants.BrowserName.Firefox:
                    browser = Coypu.Drivers.Browser.Firefox;
                    break;
                case Constants.BrowserName.IE:
                    browser = Coypu.Drivers.Browser.InternetExplorer;
                    break;
                default:
                    throw new NotSupportedException(string.Format("{0} is not a supported browser", BrowserConfig));
            }

            return new BrowserSession(new SessionConfiguration
            {
                Browser = browser,
                Timeout = TimeSpan.FromSeconds(SessionTimeoutSec)
            });
        }

        public static BrowserSession GetGridBrowserSession()
        {
            DesiredCapabilities desiredCapabilities;
            Coypu.Drivers.Browser browser;

            switch (BrowserConfig)
            {
                case Constants.BrowserName.IE:
                    browser = Coypu.Drivers.Browser.InternetExplorer;
                    desiredCapabilities = DesiredCapabilities.InternetExplorer();
                    break;
                case Constants.BrowserName.Chrome:
                    browser = Coypu.Drivers.Browser.Chrome;
                    desiredCapabilities = DesiredCapabilities.Chrome();
                    break;
                case Constants.BrowserName.Firefox:
                    browser = Coypu.Drivers.Browser.Firefox;
                    desiredCapabilities = DesiredCapabilities.Firefox();
                    break;
                default:
                    throw new NotSupportedException(string.Format("{0} is not a supported browser", BrowserConfig));
            }

            new OpenQA.Selenium.IE.InternetExplorerOptions()
            {
                IgnoreZoomLevel = true
            }.ToCapabilities();

            desiredCapabilities.SetCapability(CapabilityType.AcceptSslCertificates, true);
            desiredCapabilities.SetCapability("ignoreZoomSetting", true);

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["UseProxy"]))
            {
                var proxy = new Proxy();
                proxy.Kind = ProxyKind.Direct;
                desiredCapabilities.SetCapability(CapabilityType.Proxy, proxy);
            }

            var driver = new CustomRemoteWebDriver(browser, desiredCapabilities);

            return new BrowserSession(
                        new SessionConfiguration
                        {
                            Timeout = TimeSpan.FromSeconds(SessionTimeoutSec),
                            Browser = browser,
                            Driver = driver.GetType(),
                        },
                        driver);
        }

        public static void Resize(this BrowserSession browser, Constants.Device device)
        {

            switch (device)
            {
                case Constants.Device.Mobile:
                    browser.ResizeTo(320, 568);
                    break;
                case Constants.Device.Tablet:
                    browser.ResizeTo(768, 1024);
                    break;
                case Constants.Device.Desktop:
                case Constants.Device.Unknown:
                    browser.MaximiseWindow();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("device");
            }
        }
    }
}
