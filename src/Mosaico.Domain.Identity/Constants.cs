namespace Mosaico.Domain.Identity
{
    public static class Constants
    {
        public const string Schema = "dbo";
        public const string IdentityServerDbConfigurationSection = "IdentitySqlServer";

        //TODO: to settings
        public const int SecurityCodeExpiresInMinutes = 15;
        
        public static class SecurityCodeContexts
        {
            public const string PasswordChange = "PASSWORD_CHANGE";
            public const string AccountStolen = "ACCOUNT_STOLEN";
        }
        
        public static class Tables
        {
            public const string Users = "Users";
            public const string Roles = "Roles";
            public const string Claims = "Claims";
            public const string UserToRole = "UserToRole";
            public const string UserLogin = "UserLogins";
            public const string UserTokens = "UserTokens";
            public const string RoleClaims = "RoleClaims";
            public const string Permissions = "Permissions";
            public const string UserToPermission = "UserToPermission";
            public const string SecurityCodes = "SecurityCodes";
            public const string DeletionRequests = "DeletionRequests";
            public const string PhoneNumberConfirmationCodes = "PhoneNumberConfirmationCodes";
            public const string LoginAttempts = "LoginAttempts";
            public const string AuthorizedDevices = "AuthorizedDevices";
            public const string KangaUser = "KangaUsers";
            public const string NewsletterSubscribers = "NewsletterSubscribers";
            public const string UserEvaluationQuestions = "UserEvaluationQuestion";
            public const string KycVerifications = "KycVerifications";
        }

        public static class ErrorCodes
        {
            public const string UserNotFound = "USER_NOT_FOUND";
            public const string EmptyEmail = "EMPTY_EMAIL";
            public const string UserAlreadyExists = "USER_ALREADY_EXISTS";
            public const string PermissionNotFound = "PERMISSION_NOT_FOUND";
            public const string LackOfPermissions = "PERMISSION_INSUFFICIENCY";
            public const string SecurityCodeInvalid = "INVALID_SECURITY_CODE";
            public const string PhoneNumberConfirmationCodeInvalid = "INVALID_PHONE_NUMBER_CONFIRMATION_CODE";
            public const string InvalidPhoneNumber = "INVALID_PHONE_NUMBER";
            public const string EmptyPhoneNumber = "EMPTY_PHONE_NUMBER";
        }

        public static class Permissions
        {
            public const string CanLogin = "CAN_LOGIN";
            public const string CanUpdateEmail = "CAN_UPDATE_EMAIL";
            public const string CanUpdatePhone = "CAN_UPDATE_PHONE";
        }

        public static class Roles
        {
            public const string Admin = "admin";
        }
    }
}