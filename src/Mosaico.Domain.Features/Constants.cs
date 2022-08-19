namespace Mosaico.Domain.Features
{
    public static class Constants
    {
        public const string Schema = "ftr";
        public static class BetaTesterTypes
        {
            public const string Default = "DEFAULT";
        }
        public static class Tables
        {
            public const string Features = "Features";
            public const string BetaTesters = "BetaTesters";
            public const string TestSubmissions = "TestSubmissions";
        }
        public static class ErrorCodes
        {
            public const string FeatureNotFound = "FEATURE_NOT_FOUND";
            public const string BetaTesterNotFound = "BETA_TESTER_NOT_FOUND";
            public const string FeatureMustHaveAName = "FEATURE_MUST_HAVE_A_NAME";
        }
    }
}