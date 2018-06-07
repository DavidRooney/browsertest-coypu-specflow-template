using System;
using System.Configuration;

namespace browsertest_coypu_specflow_template.Pages
{
    public class TestPage
    {
        private const string PageTitle = "Features · The right tools for the job · GitHub";
        private const string NavigateUrl = "features";
        private readonly string Url;

        public TestPage()
        {
            Url = string.Format("{0}/{1}", ConfigurationManager.AppSettings["HostUrl"], NavigateUrl);
        }

        public TestPage Browse()
        {
            Browser.Session.Visit(Url);
            return this;
        }

        public bool IsShown()
        {
            return String.Equals(Browser.Session.Title, PageTitle, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
