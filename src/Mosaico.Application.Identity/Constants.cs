using System.Collections.Generic;

namespace Mosaico.Application.Identity
{
    public static class Constants
    {
        public static class LockoutParameters
        {
            public const int LoginFailuresThresholdInMinutes = 5;
            public const int LockoutPeriodInMinutes = 15;
            public const int FailAttemptsCountBeforeLockout = 3;
        }
        public static class UrlKeys
        {
            public const string BaseUri = "BaseUri";
            public const string AfterLoginRedirectUrl = "AfterLoginRedirectUrl";

        }

        public static class EvaluationQuestionKeys
        {
            public static List<string> All = new List<string>
            {
                "BLOCKCHAIN",
                "TOKEN_ASSETS",
                "INVESTED_BEFORE",
                "INVESTMENT_RISKS"
            };
        }
        
        public static class ErrorCodes
        {
            public const string EmptyEmail = "EMPTY_EMAIL";
            public const string EmptyFirstName = "EMPTY_FIRST_NAME";
            public const string EmptyLastName = "EMPTY_LAST_NAME";
            public const string InvalidId = "INVALID_ID";
            public const string RegistrationFailed = "REGISTRATION_FAILED";
            public const string LoginFailed = "LOGIN_FAILED";
            public const string EmailConfirmationFailed = "EMAIL_CONFIRMATION_FAILED";
            public const string EmailChangeFailed = "EMAIL_CHANGE_FAILED";
            public const string ConfirmationCodeGenerationFailed = "CONFIRMATION_CODE_GENERATION_FAILED";
            public const string InvalidPhoneNumberConfirmationCode = "INVALID_PHONE_NUMBER_CONFIRMATION_CODE";
            public const string IncorrectPassword = "INCORRECT_PASSWORD";
            public const string UserAccountDeletionJobException = "USER_ACCOUNT_DELETION_JOB_EXCEPTION";
            public const string AMLCompleted = "USER_AML_ALREADY_VERIFIED";
        }

        public static class Jobs
        {
            public const string UserAccountDeletionJob = "user-account-deletion-job";
            public const string UserKycVerificationJob = "user-kyc-verification-job";
        }
        public const string LanguageKey = "lang";

        public static class UserFindFields
        {
            public const string Email = "email";
            public static IEnumerable<string> All => new[] {Email};
        }
    }
}