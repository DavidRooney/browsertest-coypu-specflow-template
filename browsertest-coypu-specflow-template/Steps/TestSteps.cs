using browsertest_coypu_specflow_template.Helpers;
using browsertest_coypu_specflow_template.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace browsertest_coypu_specflow_template.Steps
{
    [Binding]
    public sealed class TestSteps
    {
        [Given(@"I can open a browser window")]
        public void GivenICanOpenABrowserWindow()
        {
            var page = new TestPage();
            page.Browse();

            Assert.IsTrue(page.IsShown());
        }

        //TODO: move to Common steps class
        [Given(@"I'm using a (.*)")]
        public void GivenImUsingADevice(Constants.Device device)
        {
            ScenarioContext.Current.SetDevice(device);
            Browser.Session.Resize(device);
        }
    }
}
