using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Mosaico.Base
{
    public static class Constants
    {
        public static class RegularExpressions
        {
            public static Regex BasicInputString = new Regex(@"^(?=.*\S).+$", RegexOptions.Compiled);
        }
        public static class ExceptionCodes
        {
            public const string UnhandledError = "UNHANDLED_ERROR";
            public const string InvalidConfiguration = "INVALID_CONFIG";
            public const string Forbidden = "FORBIDDEN";
            public const string LimitExceeded = "LIMIT_EXCEEDED";
            public const string InvalidCaptcha = "INVALID_CAPTCHA";
        }

        //This languages are based on ISO 639-1
        public static class Languages
        {
            public static IReadOnlyList<string> All = new[] {English, Polish};
            public const string English = "en";
            public const string Polish = "pl";
        }
    }
}