namespace Mosaico.Application.BusinessManagement
{
    public static class Constants
    {
        public const int ProposalThresholdInMinutes = 5;
        public const int ProposalLifecycleInMinutes = 7 * 24 * 60;
        
        public static class Counters
        {
            public const string Companies = "companies";
        }
        
        public static class UrlKeys
        {
            public const string BaseUri = "BaseUri";
            public const string AfterLoginRedirectUrl = "AfterLoginRedirectUrl";
        }
        
        public static class RegEx
        {
            public const string UrlRegEx = @"^(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})$";
        }
        
        public static class Jobs
        {
            public const string StartVoting = "start-voting";
        }
    }
}