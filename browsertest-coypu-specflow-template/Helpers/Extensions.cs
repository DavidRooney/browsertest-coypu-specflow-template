using System;
using System.Text.RegularExpressions;

namespace browsertest_coypu_specflow_template.Helpers
{
    public static class Extensions
    {
        public static string GetFriendlyPageName(this Type type)
        {
            var name = type.Name.ToSentenceCase();
            if (name.EndsWith("page", StringComparison.InvariantCultureIgnoreCase))
            {
                name = name.Substring(0, name.Length - 4).Trim();
            }

            return name;
        }

        public static Constants.BrowserName ToBrowserName(this string name)
        {
            switch (name.ToLower())
            {
                case "chrome":
                    return Constants.BrowserName.Chrome;
                case "ie":
                case "internet explorer":
                    return Constants.BrowserName.IE;
                case "firefox":
                    return Constants.BrowserName.Firefox;
                default:
                    throw new Exception(string.Format("Browser '{0}' is not configured for use", name));
            }
        }

        public static string ToSentenceCase(this string str)
        {
            return Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
        }
    }
}
