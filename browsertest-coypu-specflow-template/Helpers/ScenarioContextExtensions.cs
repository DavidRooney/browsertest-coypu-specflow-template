using System;
using TechTalk.SpecFlow;

namespace browsertest_coypu_specflow_template.Helpers
{
    public static class ScenarioContextExtensions
    {
        private const string ScrenarioStartTimeKey = "ScrenarioStartTimeKey";
        private const string DeviceKey = "currentDeviceKey";

        public static void SetScenarioStartTimeUtc(this ScenarioContext ctx)
        {
            ctx.Set(DateTime.UtcNow, ScrenarioStartTimeKey);
        }

        public static System.DateTime GetScenarioStartTimeUtc(this ScenarioContext ctx)
        {
            return ctx.ValidatedGet<DateTime>(ScrenarioStartTimeKey);
        }

        public static Constants.Device GetDevice(this ScenarioContext ctx)
        {
            return ctx.ContainsKey(DeviceKey) ? ctx.Get<Constants.Device>(DeviceKey) : Constants.Device.Unknown;
        }

        public static void SetDevice(this ScenarioContext ctx, Constants.Device device)
        {
            ctx.Set(device, DeviceKey);
        }

        private static T ValidatedGet<T>(this ScenarioContext ctx, string key)
        {
            if (ctx.ContainsKey(key))
            {
                return ctx.Get<T>(key);
            }

            return default(T);
        }
    }
}
