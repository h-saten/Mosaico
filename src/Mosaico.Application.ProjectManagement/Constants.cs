namespace Mosaico.Application.ProjectManagement
{
    public static class Constants
    {
        //TODO: to project settings
        public const int ProjectMemberLimit = 10;
        public const int FaqLimit = 20;
        public const int SocialMediaLinkLimit = 15;
        public const int ArticleLimit = 50;
        
        public static class DefaultColors
        {
            public const string PrimaryColor = "#494642";
            public const string SecondaryColor = "#FFFFFF";
            public const string CoverColor = "#FFFFFF";
        }
        
        public static class Counters
        {
            public const string Projects = "projects";
            public const string Invitations = "invitations";
        }
        
        public static class ErrorCodes
        {
            public const string FaqLimitExceeded = "FAQ_LIMIT_EXCEEDED";
            public const string NoInvitations = "NO_INVITATIONS";
            public const string InvitationAlreadyAccepted = "INVITATION_ALREADY_ACCEPTED";
            public const string InvitationAlreadyExpired = "INVITATION_ALREADY_EXPIRED";
            public const string UserNotAuthorizedToAcceptInvitation = "INVITATION_UNACCEPTABLE_BY_USER";
            public const string VestingWalletWasNotDeployed = "VESTING_WALLET_NOT_DEPLOYED";
            public const string StageCannotStart = "STAGE_CANNOT_START";
            public const string AnotherStageIsStillActive = "ANOTHER_STAGE_STILL_ACTIVE";
            public const string InvalidStageId = "INVALID_STAGE_ID";
            public const string StageDeployerError = "STAGE_DEPLOYER_ERROR";
            public const string StageNotClosed = "STAGE_NOT_CLOSED";
            public const string CompanyNotExists = "COMPANY_NOT_EXISTS";
            public const string ProjectTeamMemberNotFound = "PROJECT_TEAM_MEMBER_NOT_FOUND";
            public const string ProjectPartnerNotFound = "PROJECT_PARTNER_NOT_FOUND";
            public const string ProjectCrowdSaleNotFound = "PROJECT_CROWDSALE_NOT_FOUND";
            public const string ProjectInvitationAlreadySent = "PROJECT_INVITATION_ALREADY_SENT";
            public const string CannotRemoveProjectCreator = "CANNOT_REMOVE_PROJECT_CREATOR";
            public const string CannotSelfDelete = "CANNOT_REMOVE_YOURSELF";
            public const string ProjectInvitationExpired = "PROJECT_INVITATION_EXPIRED";
            public const string ProjectMemberLimitExceeded = "PROJECT_MEMBER_LIMIT_EXCEEDED";
            public const string ProjectMemberNotFound = "PROJECT_MEMBER_NOT_FOUND";
            public const string CannotChangeOwnRole = "CANNOT_CHANGE_OWN_ROLE";
            public const string AlreadySubscribedToNewsletter = "USER_ALREADY_SUBSRIBED_TO_NEWSLETTER";
            public const string SubscriptionNotFound = "SUBSCRIPTION_NOT_FOUND";
            public const string ArticleNotFound = "ARTICLE_NOT_FOUND";
            public const string UnableToChangeProjectStatus = "UNABLE_TO_CHANGE_PROJECT_STATUS";
            public const string StakingAlreadyEnabled = "STAKING_ALREADY_ENABLED";
            public const string UnsupportedPaymentCurrency = "UNSUPPORTED_PAYMENT_CURRENCY";
            public const string StagesOverlap = "STAGES_OVERLAP_EXCEPTION";
        }

        public static class Jobs
        {
            public const string StageFinalizationJob = "stage-finalization-job";
            public const string StageActivationJob = "stage-activation-job";
        }
        public static class UrlKeys
        {
            public const string BaseUri = "BaseUri";
        }

        public const int MinimumStartDateThresholdInDays = 1;
    }
}