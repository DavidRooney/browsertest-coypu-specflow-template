using Coypu;
using System;
using System.Linq;
using System.Configuration;
using TechTalk.SpecFlow;
using TechTalk.SpecRun;
using log4net;
using System.Reflection;
using browsertest_coypu_specflow_template.Helpers;
using browsertest_coypu_specflow_template.Steps;

namespace browsertest_coypu_specflow_template
{
    [Binding]
    public static class Browser
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static string UniqueThreadIdentifier
        {
            get
            {
                var id = ConfigurationManager.AppSettings["testThreadId"];
                var specrunProfileName = ConfigurationManager.AppSettings["specrunProfileName"];
                if (string.IsNullOrWhiteSpace(id))
                {
                    id = "1";
                }

                return string.Concat(specrunProfileName, "_TestThread_", id, "_").ToLower();
            }
        }

        public static BrowserSession Session { get; set; }

        [BeforeTestRun]
        public static void BeforeRun()
        {
            log4net.Config.XmlConfigurator.Configure();

            Log.Debug($"BeforeTestRun for thread {UniqueThreadIdentifier}");

            Session = CreateNewSession(ConfigurationManager.AppSettings["UseSeleniumGrid"].Equals("false", StringComparison.InvariantCultureIgnoreCase));

            Log.Debug($"BeforeTestRun complete for thread {UniqueThreadIdentifier}");
        }

        [BeforeScenario]
        public static void BeforeScenario()
        {
            CheckBrowserStillAlive();
        }

        [Scope(Tag = "Pending")]
        [Scope(Tag = "pending")]
        [BeforeScenario(Order = 0)]
        public static void PendingBeforeScenario()
        {
            SpecRunner.TestPending("Tagged as pending");
        }

        [BeforeScenario(Order = 0)]
        public static void BeforeScenarioSetStartTime()
        {
            ScenarioContext.Current.SetScenarioStartTimeUtc();
            new TestSteps().GivenImUsingADevice(Constants.Device.Desktop);
        }

        [AfterScenario]
        public static void AfterScenario()
        {
            if (ScenarioContext.Current.ScenarioInfo.Tags.Select(t => t.ToLower()).Contains("pending"))
            {
                return;
            }

            //Log.Debug($"BeforeTestRun for thread {UniqueThreadIdentifier}");
        }

        [AfterTestRun]
        public static void AfterRun()
        {
            Session.Dispose();
        }

        public static BrowserSession CreateNewSession(bool useLocalSession)
        {
            return useLocalSession ? GetLocalBrowserSession() : GetSeleniumGridBrowserSession();
        }

        private static BrowserSession GetLocalBrowserSession()
        {
            return BrowserSessionHelper.GetLocalBrowserSession();
        }

        private static BrowserSession GetSeleniumGridBrowserSession()
        {
            return BrowserSessionHelper.GetGridBrowserSession();
        }

        private static void CheckBrowserStillAlive()
        {
            if (Session == null || Session.Browser == null)
            {
                Session = ConfigurationManager.AppSettings["UseSeleniumGrid"].Equals("false",
                    StringComparison.InvariantCultureIgnoreCase)
                    ? GetLocalBrowserSession()
                    : GetSeleniumGridBrowserSession();
            }
        }
    }
}
